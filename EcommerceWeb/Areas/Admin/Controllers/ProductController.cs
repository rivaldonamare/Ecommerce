using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers;

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }


    #region Fetch Product
    public IActionResult Index()
    {
        List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().OrderBy(x => x.Title).ToList();
        return View(objProductList);
    }
    #endregion

    #region Update Insert Product
    public IActionResult Upsert(Guid? id)
    {
        ProductVM productVM = new()
        {
            CategoryList = _unitOfWork.CategoryRepository
            .GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }),

            Product = new Product()
        };

        if (id == null)
        {
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.ProductRepository.GetById(x => x.Id == id);
            return View(productVM);
        }
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (_unitOfWork.ProductRepository.GetAll().Any(x => x.Title.Equals(obj.Product.Title, StringComparison.CurrentCultureIgnoreCase)))
        {
            ModelState.AddModelError("Title", "Title already exists");
        }

        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product\");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                obj.Product.ImageUrl = @"images\product\" + fileName;
            }

            _unitOfWork.ProductRepository.Add(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product Created Successfully!";
            return RedirectToAction("Index");
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
