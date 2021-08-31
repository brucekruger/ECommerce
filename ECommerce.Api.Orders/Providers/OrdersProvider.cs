using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                var orders = new List<Order>()
                {
                    new Db.Order
                    {
                        Id = 1,
                        CustomerId = 1,
                        OrderDate = DateTime.Now,
                        Total = 1400,
                        Items = new List<OrderItem>{
                            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 100 },
                            new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 200 },
                            new OrderItem { Id = 3, OrderId = 1, ProductId = 3, Quantity = 3, UnitPrice = 300 }
                        }
                    },
                    new Db.Order
                    {
                        Id = 2,
                        CustomerId = 2,
                        OrderDate = DateTime.Now.AddDays(-1),
                        Total = 900,
                        Items = new List<OrderItem>{
                             new OrderItem { Id = 4, OrderId = 2, ProductId = 1, Quantity = 1, UnitPrice = 100 },
                             new OrderItem { Id = 5, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 200 },
                             new OrderItem { Id = 6, OrderId = 2, ProductId = 3, Quantity = 2, UnitPrice = 300 }
                        }    
                    },
                    new Db.Order
                    {
                        Id = 3,
                        CustomerId = 3,
                        OrderDate = DateTime.Now.AddDays(-2),
                        Total = 2000,
                        Items = new List<OrderItem>{
                            new OrderItem { Id = 7, OrderId = 3, ProductId = 1, Quantity = 3, UnitPrice = 100 },
                            new OrderItem { Id = 8, OrderId = 3, ProductId = 2, Quantity = 4, UnitPrice = 200 },
                            new OrderItem { Id = 9, OrderId = 3, ProductId = 3, Quantity = 3, UnitPrice = 300 }
                        }
                    }
                };

                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
