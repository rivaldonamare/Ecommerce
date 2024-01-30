using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EcommerceWeb.Controllers;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork) 
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    #region Fetch Category
    public IActionResult Index()
    {
        List<Category> objCatergoryList = _unitOfWork.CategoryRepository.GetAll().OrderBy(x => x.DisplayOrder).ToList();
        return View(objCatergoryList);
    }
    #endregion

    #region Create Category
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (_unitOfWork.CategoryRepository.GetAll().Any(x => x.DisplayOrder == obj.DisplayOrder))
        {
            ModelState.AddModelError("DisplayOrder", "Display Order already exists");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepository.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Created Successfully!";
            return RedirectToAction("Index");
        }

        return View();

    }
    #endregion

    #region Update Category
    public ActionResult Update(Guid id)
    {
        Category category = _unitOfWork.CategoryRepository.GetById(x => x.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    [HttpPost]
    public IActionResult Update(Category obj)
    {
        var existingCategory = _unitOfWork.CategoryRepository.GetById(x => x.Id == obj.Id);

        if (existingCategory != null)
        {
            if (_unitOfWork.CategoryRepository.GetAll().Any(x => x.DisplayOrder == obj.DisplayOrder && x.Id != obj.Id))
            {
                ModelState.AddModelError("DisplayOrder", "Display Order already exists");
            }

            if (ModelState.IsValid)
            {
                existingCategory.CatergoryName = obj.CatergoryName;
                existingCategory.DisplayOrder = obj.DisplayOrder;
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully!";
                return RedirectToAction("Index");
            }
        }

        return View();
    }
    #endregion

    #region Delete Category
    public  ActionResult Delete(Guid id)
    {
        Category category = _unitOfWork.CategoryRepository.GetById(x => x.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    [HttpPost]
    public IActionResult Delete(Category obj)
    {
        _unitOfWork.CategoryRepository.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category Deleted Successfully!";
        return RedirectToAction("Index");
    }
    #endregion
}
