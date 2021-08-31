using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var (IsSuccess, Orders, _) = await ordersService.GetOrdersAsync(customerId);
            if (IsSuccess)
            {
                var productsResult = await productsService.GetProductsAsync();

                foreach(var order in Orders)
                {
                    foreach(var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId).Name :
                            "Product information is not available";
                    }
                }

                var customerResult = await customersService.GetCustomerAsync(customerId);

                var result = new
                {
                    Customer = customerResult.IsSuccess ?
                                customerResult.Customer :
                                new Customer { Name = "Customer information is not available" },
                    Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
