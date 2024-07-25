using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.Models.Models;
using EcommerceWEB.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWEB.DataAccess.DBInitializer;

public class DBInitializer : IDBInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _applicationDbContext;

    public DBInitializer(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _applicationDbContext = applicationDbContext;
    }
    public void Intializer()
    {
        try
        {
            if (_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
            {
                _applicationDbContext.Database.Migrate();
            }
        }
        catch (Exception ex) { }

        if (!_roleManager.RoleExistsAsync(SD.Role_User_Cust).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Cust)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "administration@gmail.com",
                Email = "administration@gmail.com",
                Name = "Admin",
                PhoneNumber = "0217532033",
                StreetAddress = "Jakarta",
                State = "JKT",
                PostalCode = "12345",
                City = "Jaksel"
            }, "Test123?").GetAwaiter().GetResult();

            ApplicationUser user = _applicationDbContext.ApplicationUsers.FirstOrDefault(x => x.Email == "administration@gmail.com");
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }

        return;
    }
}
