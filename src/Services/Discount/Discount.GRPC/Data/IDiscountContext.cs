using Npgsql;

namespace Discount.GRPC.Data
{
	public interface IDiscountContext
	{
	    NpgsqlConnection Connection { get; }
	}
}
