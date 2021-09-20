using Npgsql;

namespace Discount.API.Data
{
	public interface IDiscountContext
	{
	    NpgsqlConnection Connection { get; }
	}
}
