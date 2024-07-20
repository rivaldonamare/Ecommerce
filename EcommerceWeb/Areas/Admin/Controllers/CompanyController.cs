using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }
    public IActionResult Index()
    {
        List<Company> objCompanyList = _unitOfWork.CompanyRepository.GetAll(null).OrderBy(x => x.Name).ToList();
        return View(objCompanyList);
    }

    #region Update Insert Company
    public IActionResult Upsert(Guid? id)
    {
        CompanyVM companyVM = new()
        {
            Company = new Company()
        };

        if (id.HasValue)
        {
            companyVM.Company = _unitOfWork.CompanyRepository.GetById(x => x.Id == id, null);
        }

        return View(companyVM);
    }

    [HttpPost]
    public IActionResult Upsert(CompanyVM obj)
    {
        if (_unitOfWork.ProductRepository.GetAll(null).Any(x => x.Id != obj.Company.Id && x.Title.Equals(obj.Company.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            ModelState.AddModelError("Title", "Title already exists");
        }

        if (ModelState.IsValid)
        {
            if (obj.Company.Id == Guid.Empty)
            {
                _unitOfWork.CompanyRepository.Add(obj.Company);
                TempData["success"] = "Company Created Successfully!";
            }
            else
            {
                _unitOfWork.CompanyRepository.Update(obj.Company);
                TempData["success"] = "Company Updated Successfully!";
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(obj);
    }
    #endregion

    #region API Calls
    [HttpGet]
    public ActionResult GetAll()
    {
        List<Company> objCompanyList = _unitOfWork.CompanyRepository.GetAll(null).OrderBy(x => x.Name).ToList();
        return Json(new { data = objCompanyList });
    }

    [HttpDelete]
    public ActionResult Delete(Guid id)
    {
        var entity = _unitOfWork.CompanyRepository.GetById(x => x.Id == id, null);

        if (entity == null)
        {
            return Json(new { success = false, message = "Error while deleting company." });
        }

        _unitOfWork.CompanyRepository.Remove(entity);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Successfully deleting company." });

    }
    #endregion
}
