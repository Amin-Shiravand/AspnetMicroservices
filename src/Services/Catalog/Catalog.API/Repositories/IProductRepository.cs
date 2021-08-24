using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
	public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetProducts();
		Task<Product> GetProduct(string Id);
		Task<IEnumerable<Product>> GetProductByName(string Name);
		Task<IEnumerable<Product>> GetProductByCategory(string CategoryName);
		Task CreateProduct(Product Product);
		Task<bool> UpdateProduct(Product product);
		Task<bool> DeleteProduct(string Id);
	}
}
