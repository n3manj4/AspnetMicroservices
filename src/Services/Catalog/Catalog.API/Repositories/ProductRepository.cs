﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext _context;

		public ProductRepository(ICatalogContext context)
		{
			_context = context;
		}

		public async Task CreateProduct(Product product)
		{
			await _context.Products.InsertOneAsync(product);
		}

		public async Task<bool> DeleteProduct(string id)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

			DeleteResult deleteResult = await _context
												.Products
												.DeleteOneAsync(filter);

			return deleteResult.IsAcknowledged
				&& deleteResult.DeletedCount > 0;
		}

		public async Task<Product> GetProductById(string id)
		{
			return await _context.
				Products
				.Find(prop => prop.Id == id)
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await _context
				.Products
				.Find(prop => true)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategory(string name)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, name);

			return await _context
				.Products
				.Find(filter)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByName(string name)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

			return await _context
				.Products
				.Find(filter)
				.ToListAsync();
		}

		public async Task<bool> UpdateProduct(Product product)
		{
			var updateResult = await _context
									   .Products
									   .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

			return updateResult.IsAcknowledged
					&& updateResult.ModifiedCount > 0;
		}
	}
}
