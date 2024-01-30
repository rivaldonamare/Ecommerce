using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers;

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    #region Fetch Product
    public IActionResult Index()
    {
        List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().OrderBy(x => x.Title).ToList();
        return View(objProductList);
    }
    #endregion

    #region Create Product
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Product obj)
    {
        if (_unitOfWork.ProductRepository.GetAll().Any(x => x.Title.Equals(obj.Title, StringComparison.CurrentCultureIgnoreCase)))
        {
            ModelState.AddModelError("Title", "Title already exists");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Created Successfully!";
            return RedirectToAction("Index");
        }

        return View();
    }
    #endregion

    #region Update Product
    public ActionResult Update(Guid id)
    {
        Product product = _unitOfWork.ProductRepository.GetById(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    [HttpPost]
    public IActionResult Update(Product obj)
    {
        var existingProduct = _unitOfWork.ProductRepository.GetById(x => x.Id == obj.Id);

        if (existingProduct != null)
        {
            if (_unitOfWork.ProductRepository.GetAll().Any(x => x.Title.Equals(obj.Title, StringComparison.CurrentCultureIgnoreCase) && x.Id != obj.Id))
            {
                ModelState.AddModelError("Title", "Title already exists");
            }

            if (ModelState.IsValid)
            {
                existingProduct.Title = obj.Title;
                existingProduct.Description = obj.Description;
                existingProduct.ISBN = obj.ISBN;
                existingProduct.Author = obj.Author;
                existingProduct.ListPrice = obj.ListPrice;
                existingProduct.Price = obj.Price;
                existingProduct.Price50 = obj.Price50;
                existingProduct.Price100 = obj.Price100;


                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully!";
                return RedirectToAction("Index");
            }
        }

        return View();
    }
    #endregion

    #region Delete Product
    public ActionResult Delete(Guid id)
    {
        Product product = _unitOfWork.ProductRepository.GetById(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    [HttpPost]
    public IActionResult Delete(Product obj)
    {
        _unitOfWork.ProductRepository.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product Deleted Successfully!";
        return RedirectToAction("Index");
    }
    #endregion
}
