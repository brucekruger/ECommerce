using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        private ProductsProvider productsProvider;

        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            Setup(nameof(GetProductsReturnsAllProducts));

            var products = await productsProvider.GetProductsAsync();

            products.IsSuccess.Should().BeTrue();
            products.Products.Should().NotBeEmpty();
            products.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task GetProductReturnsValidId()
        {
            Setup(nameof(GetProductReturnsValidId));

            var products = await productsProvider.GetProductAsync(1);

            products.IsSuccess.Should().BeTrue();
            products.Product.Should().NotBeNull();
            products.Product.Id.Should().Be(1);
            products.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public async Task GetProductReturnsInvalidId()
        {
            Setup(nameof(GetProductReturnsInvalidId));

            var products = await productsProvider.GetProductAsync(-1);

            products.IsSuccess.Should().BeFalse();
            products.Product.Should().BeNull();
            products.ErrorMessage.Should().NotBeNull();
        }

        private void Setup(string DatabaseName)
        {
            if (string.IsNullOrEmpty(DatabaseName))
            {
                throw new ArgumentNullException(nameof(DatabaseName));
            }

            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(DatabaseName)
                .Options;

            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            productsProvider = new ProductsProvider(dbContext, null, mapper);
        }

        private static void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
