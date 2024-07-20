using EcommerceWEB.Models.Models;

namespace EcommerceWEB.Models.ViewModels;

public class ShoppingCartVM
{
    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public double OrderTotal { get; set; }
}
