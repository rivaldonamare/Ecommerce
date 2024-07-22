using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IOrderHeaderRepository : IRepositroy<OrderHeader>
{
    void Update(OrderHeader obj);
}
