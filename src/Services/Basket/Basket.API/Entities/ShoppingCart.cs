using System.Collections.Generic;

namespace Basket.API.Entities
{
	public class ShoppingCart
	{
		public string UserName { get; set; }

		public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

		public decimal TotalPrice
		{
			get
			{
				decimal totalPrice = 0;
				for (short i = 0; i < Items.Count; ++i)
				{
					ShoppingCartItem item = Items[i];
					totalPrice += item.Price * item.Quantity;
				}
				return totalPrice;
			}
		}

		public ShoppingCart()
		{

		}

		public ShoppingCart(string UserName)
		{
			this.UserName = UserName;
		}
	}
}
