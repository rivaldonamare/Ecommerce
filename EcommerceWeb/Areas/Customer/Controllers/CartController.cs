using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Customer.Controllers;
[Area("customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
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
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(includeProperties:"Product", x => x.ApplicationUserId == userId)
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderTotal += (cart.Price + cart.Count);
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        return View();
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
