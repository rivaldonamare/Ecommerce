using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using EcommerceWEB.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
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
        List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").OrderBy(x => x.Title).ToList();
        return View(objProductList);
    }
    #endregion

    #region Update Insert Product
    public IActionResult Upsert(Guid? id)
    {
        ProductVM productVM = new()
        {
            CategoryList = _unitOfWork.CategoryRepository.GetAll(null).Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }),

            Product = new Product()
        };

        if (id.HasValue)
        {
            productVM.Product = _unitOfWork.ProductRepository.GetById(x => x.Id == id, includeProperties: "Category");
        }

        return View(productVM);
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM obj, IFormFile file)
    {
        if (_unitOfWork.ProductRepository.GetAll(null).Any(x => x.Id != obj.Product.Id && x.Title.Equals(obj.Product.Title, StringComparison.CurrentCultureIgnoreCase)))
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

                if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                obj.Product.ImageUrl = @"\images\product\" + fileName;
            }

            if (obj.Product.Id == Guid.Empty)
            {
                _unitOfWork.ProductRepository.Add(obj.Product);
                TempData["success"] = "Product Created Successfully!";
            }
            else
            {
                _unitOfWork.ProductRepository.Update(obj.Product);
                TempData["success"] = "Product Updated Successfully!";
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

       /* obj.CategoryList = _unitOfWork.CategoryRepository.GetAll(includeProperties: "Category").Select(x => new SelectListItem
        {
            Text = x.CategoryName,
            Value = x.Id.ToString()
        });*/

        return View(obj);
    }
    #endregion

    

    #region API Calls
    [HttpGet]
    public ActionResult GetAll()
    {
        List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").OrderBy(x => x.Title).ToList();
        return Json(new { data = objProductList });
    }

    [HttpDelete]
    public ActionResult Delete(Guid id)
    {
        var entity = _unitOfWork.ProductRepository.GetById(x => x.Id == id, includeProperties: "Category");

        if (entity == null)
        {
            return Json(new { success = false, message = "Error while deleting product." });
        }


        string wwwRootPath = _webHostEnvironment.WebRootPath;
        var oldImagePath = Path.Combine(wwwRootPath, entity.ImageUrl.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.ProductRepository.Remove(entity);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Successfully deleting product." });

    }
    #endregion
}
