﻿using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Models.ViewModels;
using EcommerceWEB.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    public UserController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult RoleManagment(string userId)
    {

        RoleManagmentVM RoleVM = new RoleManagmentVM()
        {
            ApplicationUser = _unitOfWork.ApplicationUserRepository.GetById(u => u.Id == userId, includeProperties: "Company"),
            RoleList = _roleManager.Roles.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            }),
            CompanyList = _unitOfWork.CompanyRepository.GetAll(null, null).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };

        RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUserRepository.GetById(u => u.Id == userId, null))
                .GetAwaiter().GetResult().FirstOrDefault();
        return View(RoleVM);
    }

    [HttpPost]
    public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM)
    {

        string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUserRepository.GetById(u => u.Id == roleManagmentVM.ApplicationUser.Id, null))
                .GetAwaiter().GetResult().FirstOrDefault();

        ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.GetById(u => u.Id == roleManagmentVM.ApplicationUser.Id, null);


        if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
        {
            //a role was updated
            if (roleManagmentVM.ApplicationUser.Role == SD.Role_User_Comp)
            {
                applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
            }
            if (oldRole == SD.Role_User_Comp)
            {
                applicationUser.CompanyId = null;
            }
            _unitOfWork.ApplicationUserRepository.Update(applicationUser);
            _unitOfWork.Save();

            _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

        }
        else
        {
            if (oldRole == SD.Role_User_Comp && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
            {
                applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                _unitOfWork.ApplicationUserRepository.Update(applicationUser);
                _unitOfWork.Save();
            }
        }

        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        List<ApplicationUser> objUserList = _unitOfWork.ApplicationUserRepository.GetAll(includeProperties: "Company", null).ToList();

        foreach (var user in objUserList)
        {

            user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

            if (user.Company == null)
            {
                user.Company = new Company()
                {
                    Name = ""
                };
            }
        }

        return Json(new { data = objUserList });
    }


    [HttpPost]
    public IActionResult LockUnlock([FromBody] string id)
    {

        var objFromDb = _unitOfWork.ApplicationUserRepository.GetById(u => u.Id == id, null);
        if (objFromDb == null)
        {
            return Json(new { success = false, message = "Error while Locking/Unlocking" });
        }

        if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
        {
            //user is currently locked and we need to unlock them
            objFromDb.LockoutEnd = DateTime.Now;
        }
        else
        {
            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }
        _unitOfWork.ApplicationUserRepository.Update(objFromDb);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Operation Successful" });
    }


}

