using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IShoppingCartRepository : IRepositroy<ShoppingCart>
{
    void Update(ShoppingCart obj);
}
