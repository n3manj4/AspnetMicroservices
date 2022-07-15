using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{

		private readonly IConfiguration _configuration;

		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DbSettings:ConnectionString"));

			var affectedNum = 
				await connection.ExecuteAsync
				("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
					new { coupon.ProductName, coupon.Description, coupon.Amount });

			return affectedNum != 0;
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DbSettings:ConnectionString"));

			var affectedNum =
				await connection.ExecuteAsync
				("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

			return affectedNum != 0;
		}

		public async Task<Coupon> GetDiscount(string productName)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DbSettings:ConnectionString"));

			var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
				("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

			return coupon ?? new Coupon();
		}

		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DbSettings:ConnectionString"));

			var affectedNum =
				await connection.ExecuteAsync
				("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
					new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

			return affectedNum != 0;
		}
	}
}
