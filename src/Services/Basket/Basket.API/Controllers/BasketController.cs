using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BasketController : Controller
	{
		private readonly IBasketRepository _repository;

		public BasketController(IBasketRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), 200)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			var basket = await _repository.GetBasket(userName);	
			return Ok(basket ?? new ShoppingCart(userName));
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), 200)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
		{
			return Ok(await _repository.UpdateBasket(basket));
		}

		[HttpDelete("{userName}")]
		[ProducesResponseType(typeof(void), 200)]
		public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
		{
			await _repository.DeleteBasket(userName);
			return Ok();
		}
	}
}
