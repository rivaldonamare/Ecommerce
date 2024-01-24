using EcommerceWeb.Data;
using EcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext db)
    {
        _context = db ?? throw new ArgumentNullException(nameof(db));
    }

    #region Fetch Category
    public IActionResult Index()
    {
        List<Category> objCatergoryList = _context.Categories.OrderBy(x => x.DisplayOrder).ToList();
        return View(objCatergoryList);
    }
    #endregion

    #region Create Category
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Category obj)
    {
        if (await _context.Categories.AnyAsync(x => x.DisplayOrder == obj.DisplayOrder))
        {
            ModelState.AddModelError("DisplayOrder", "Display Order already exists");
        }

        if (ModelState.IsValid)
        {
            await _context.Categories.AddAsync(obj);
            await _context.SaveChangesAsync();
            TempData["success"] = "Category Created Successfully!";
            return RedirectToAction("Index");
        }

        return View();

    }
    #endregion

    #region Update Category
    public async Task<ActionResult> Update(Guid id)
    {
        Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    [HttpPost]
    public async Task<IActionResult> Update(Category obj)
    {
        if (await _context.Categories.AnyAsync(x => x.DisplayOrder == obj.DisplayOrder))
        {
            ModelState.AddModelError("DisplayOrder", "Display Order already exists");
        }

        if (ModelState.IsValid)
        {
            _context.Categories.Update(obj);
            await _context.SaveChangesAsync();
            TempData["success"] = "Category Updated Successfully!";
            return RedirectToAction("Index");
        }

        return View();

    }
    #endregion

    #region Delete Category
    public async Task<ActionResult> Delete(Guid id)
    {
        Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Category obj)
    {
        _context.Categories.Remove(obj);
        await _context.SaveChangesAsync();
        TempData["success"] = "Category Deleted Successfully!";
        return RedirectToAction("Index");
    }
    #endregion
}
