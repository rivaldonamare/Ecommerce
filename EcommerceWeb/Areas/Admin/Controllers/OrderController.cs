using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using EcommerceWEB.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty]
    public OrderVM OrderVM { get; set; }

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(Guid orderId)
    {
        OrderVM = new OrderVM()
        {
            OrderHeader = _unitOfWork.OrderHeaderRepository.GetById(x => x.Id == orderId, includeProperties: "ApplicationUser"),
            OrderDetails = _unitOfWork.OrderDetailsRepository.GetAll(includeProperties: "Product", x => x.OrderHeaderId == orderId)
        };
        return View(OrderVM);
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Employee+","+SD.Role_Admin)]
    public IActionResult UpdateOrderDetail(Guid orderId)
    {
        var orderHeader = _unitOfWork.OrderHeaderRepository.GetById(x => x.Id == OrderVM.OrderHeader.Id, null);
        orderHeader.Name = OrderVM.OrderHeader.Name;
        orderHeader.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
        orderHeader.StreetAddress = OrderVM.OrderHeader.StreetAddress;
        orderHeader.City = OrderVM.OrderHeader.City;
        orderHeader.State = OrderVM.OrderHeader.State;
        orderHeader.PostalCode = OrderVM.OrderHeader.PostalCode;
        orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
        orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
        

        _unitOfWork.OrderHeaderRepository.Update(orderHeader);
        _unitOfWork.Save();

        TempData["Success"] = "Order Details Updated Succsessfully";

        return RedirectToAction(nameof(Details), new {orderId = orderHeader.Id});
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public IActionResult StartProcessing()
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess, null);
        _unitOfWork.Save();
        TempData["Success"] = "Order Details Updated Successfully.";
        return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public IActionResult ShipOrder()
    {

        var orderHeader = _unitOfWork.OrderHeaderRepository.GetById(u => u.Id == OrderVM.OrderHeader.Id, null);
        orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
        orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
        orderHeader.OrderStatus = SD.StatusShipped;
        orderHeader.ShippingDate = DateTime.Now;
        if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
        {
            orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
        }

        _unitOfWork.OrderHeaderRepository.Update(orderHeader);
        _unitOfWork.Save();
        TempData["Success"] = "Order Shipped Successfully.";
        return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
    }
    [HttpPost]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public IActionResult CancelOrder()
    {

        var orderHeader = _unitOfWork.OrderHeaderRepository.GetById(u => u.Id == OrderVM.OrderHeader.Id, null);

        if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeader.PaymentIntentId
            };

            var service = new RefundService();
            Refund refund = service.Create(options);

            _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
        }
        else
        {
            _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
        }
        _unitOfWork.Save();
        TempData["Success"] = "Order Cancelled Successfully.";
        return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

    }

    [ActionName("Details")]
    [HttpPost]
    public IActionResult Details_PAY_NOW()
    {
        OrderVM.OrderHeader = _unitOfWork.OrderHeaderRepository
            .GetById(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
        OrderVM.OrderDetails = _unitOfWork.OrderDetailsRepository
            .GetAll(includeProperties: "Product", u => u.OrderHeaderId == OrderVM.OrderHeader.Id);

        //stripe logic
        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions
        {
            SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
            CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        foreach (var item in OrderVM.OrderDetails)
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
        _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
        _unitOfWork.Save();
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    public IActionResult PaymentConfirmation(Guid orderHeaderId)
    {

        OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.GetById(u => u.Id == orderHeaderId, null);
        if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
        {
            //this is an order by company

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(orderHeaderId, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }


        }


        return View(orderHeaderId);
    }



    #region API Calls
    [HttpGet]
    public ActionResult GetAll(string status)
    {
        IEnumerable<OrderHeader> objOrderHeaderList = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser", null).OrderBy(x => x.OrderDate).ToList();

        if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
        {
            objOrderHeaderList = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser", null).ToList();
        }
        else
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            objOrderHeaderList = _unitOfWork.OrderHeaderRepository
                .GetAll(includeProperties: "ApplicationUser", u => u.ApplicationUserId == userId);
        }


        switch (status)
        {
            case "pending":
                objOrderHeaderList = objOrderHeaderList.Where(u => u.OrderStatus == SD.PaymentStatusPending);
                break;
            case "inprocess":
                objOrderHeaderList = objOrderHeaderList.Where(u => u.OrderStatus == SD.StatusInProcess);
                break;
            case "completed":
                objOrderHeaderList = objOrderHeaderList.Where(u => u.OrderStatus == SD.StatusShipped);
                break;
            case "approved":
                objOrderHeaderList = objOrderHeaderList.Where(u => u.OrderStatus == SD.StatusApproved);
                break;
            default:
                break;
        }


        return Json(new { data = objOrderHeaderList });
    }
    #endregion
}
