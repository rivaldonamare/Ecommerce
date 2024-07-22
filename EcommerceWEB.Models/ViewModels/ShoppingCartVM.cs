using EcommerceWEB.Models.Models;

namespace EcommerceWEB.Models.ViewModels
{
	public class ShoppingCartVM
	{
		public ShoppingCartVM()
		{
			ShoppingCartList = new List<ShoppingCart>();
			OrderHeader = new OrderHeader();
		}

		public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
		public OrderHeader OrderHeader { get; set; }
	}
}
