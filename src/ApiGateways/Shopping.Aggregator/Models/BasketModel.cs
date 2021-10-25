using System.Collections.Generic;

namespace Shopping.Aggregator.Models
{
	public class BasketModel
	{
		public string UserName { get; set; }
		public List<BasketExtendedModel> Items { get; set; } = new List<BasketExtendedModel>();
		public decimal TotalPrice { get; set; }
		
	}
}
