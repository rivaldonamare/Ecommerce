using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IOrderDetailsRepository : IRepositroy<OrderDetails>
{
    void Update(OrderDetails obj);
}
