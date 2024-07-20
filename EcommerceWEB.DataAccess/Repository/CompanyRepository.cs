using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
        _context = applicationDbContext;
    }
    public void Update(Company company)
    {
        var objFromDB = _context.Companies.FirstOrDefault(x => x.Id == company.Id);

        if (objFromDB != null)
        {
            objFromDB.Id = company.Id;
            objFromDB.Name = company.Name;
            objFromDB.StreetAddress = company.StreetAddress;
            objFromDB.City = company.City;
            objFromDB.State = company.State;
            objFromDB.PostalCode = company.PostalCode;
            objFromDB.PhoneNumber = company.PhoneNumber;
        }
    }
}
