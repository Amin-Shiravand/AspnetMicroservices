using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext context;

		public ProductRepository(ICatalogContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await this.context.Products.Find(Product => true).ToListAsync();
		}

		public async Task<Product> GetProduct(string Id)
		{
			return await this.context.Products.Find(product => product.Id == Id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByName(string Name)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Name, Name);
			return await this.context.Products.Find(filter).ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByCategory(string CategoryName)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Category, CategoryName);
			return await this.context.Products.Find(filter).ToListAsync();
		}

		public async Task CreateProduct(Product Product)
		{
			await this.context.Products.InsertOneAsync(Product);
		}

		public async Task<bool> UpdateProduct(Product Product)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Id, Product.Id);
			ReplaceOneResult updateResult = await this.context.Products.ReplaceOneAsync(filter, replacement: Product);
			return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
		}

		public async Task<bool> DeleteProduct(string Id)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Id, Id);
			DeleteResult deleteResult = await this.context.Products.DeleteOneAsync(filter);
			return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
		}
	}
}
