﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
	private readonly IProductRepository _repository;
	private readonly ILogger<CatalogController> _logger;

	public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
	{
		var products = await _repository.GetProducts();
		return Ok(products);
	}

	[HttpGet("{id:length(24)}", Name = "GetProduct")]
	[ProducesResponseType((int)HttpStatusCode.NotFound)]
	[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
	public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
	{
		var product = await _repository.GetProductById(id);

		if (product is null)
		{
			_logger.LogError($"Product with id: {id} not found.");
			return NotFound();
		}

		return Ok(product);
	}

	[HttpGet]
	[Route("[action]/{category}", Name = "GetProductByCategory")]
	[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
	public async Task<ActionResult<IEnumerable<Product>>> GetProductByCat(string category)
	{
		var products = await _repository.GetProductsByCategory(category);
		return Ok(products);

	}

	[HttpPost]
	[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
	public async Task<ActionResult<IEnumerable<Product>>> CreateProduct([FromBody] Product product)
	{
		await _repository.CreateProduct(product);

		return CreatedAtRoute("GetProduct", new { id = product.Id}, product);
	}

	[HttpPut]
	[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> UpdateProduct([FromBody] Product product)
	{
		return Ok(await _repository.UpdateProduct(product));
	}

	[HttpPut("{id:length(24)}", Name = "DeleteProduct")]
	[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> DeleteProduct(string id)
	{
		return Ok(await _repository.DeleteProduct(id));
	}
}
