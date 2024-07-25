using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using EcommerceWEB.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Customer.Controllers;
[Area("customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
	[BindProperty]
	public ShoppingCartVM ShoppingCartVM { get; set; }

	public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(includeProperties:"Product", x => x.ApplicationUserId == userId),
            OrderHeader = new()
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        return View(ShoppingCartVM);
    }

	public IActionResult Summary()
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

		var shoppingCartVM = new ShoppingCartVM
		{
			ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(includeProperties: "Product", x => x.ApplicationUserId == userId).ToList(),
			OrderHeader = new OrderHeader()
		};

		shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetById(x => x.Id == userId, null);
		shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
		shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
		shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
		shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
		shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
		shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

		foreach (var cart in shoppingCartVM.ShoppingCartList)
		{
			cart.Price = GetPriceBasedOnQuantity(cart);
			shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
		}

		return View(shoppingCartVM);
	}
    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPost()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(includeProperties: "Product", x => x.ApplicationUserId == userId).ToList();
        ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
        ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

        ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.GetById(x => x.Id == userId, null);

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        if (applicationUser.CompanyId == null)
        {
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
        }
        else
        {
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
        }

        _unitOfWork.OrderHeaderRepository.Add(ShoppingCartVM.OrderHeader);

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            OrderDetails orderDetail = new()
            {
                ProductId = cart.ProductId,
                OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };
            _unitOfWork.OrderDetailsRepository.Add(orderDetail);
        }

        _unitOfWork.Save();

        if(applicationUser.CompanyId == null)
        {
            var domain = "https://localhost:7084/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/cart/OrderConfrimation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "idr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        return RedirectToAction(nameof(OrderConfrimation), new { id = ShoppingCartVM.OrderHeader.Id });
    }

    public IActionResult OrderConfrimation(Guid id)
    {
        OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.GetById(u => u.Id == id, includeProperties: "ApplicationUser");

        if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
        {
            // This is an order by customer
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                _unitOfWork.Save();

                List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCartRepository.GetAll(null, u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
                _unitOfWork.Save();
            }
            HttpContext.Session.Clear();
        }

        return View(id);
    }

	public IActionResult Plus(Guid cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetById(x => x.Id == cartId, null);
        cart.Count += 1;
        _unitOfWork.ShoppingCartRepository.Update(cart);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(Guid cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetById(x => x.Id == cartId, null);
        if(cart.Count <= 1)
        {
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCartRepository.GetAll(null, x => x.ApplicationUserId == cart.ApplicationUserId).Count() - 1);
            _unitOfWork.ShoppingCartRepository.Remove(cart);
        }
        else
        {
            cart.Count -= 1;
            _unitOfWork.ShoppingCartRepository.Update(cart);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(Guid cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetById(x => x.Id == cartId, null);

        HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCartRepository.GetAll(null, x => x.ApplicationUserId == cart.ApplicationUserId).Count() -1 );
       
        _unitOfWork.ShoppingCartRepository.Remove(cart);
        
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }





    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Count <= 50)
        {
            return shoppingCart.Product.Price;
        }
        else
        {
            if(shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
    }
}
