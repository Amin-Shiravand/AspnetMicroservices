using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API
{
	public interface ICatalogContext
	{
		IMongoCollection<Product> Products { get; }
	}
}
