using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface ICompanyRepository : IRepositroy<Company>
{
    void Update(Company company);
}
