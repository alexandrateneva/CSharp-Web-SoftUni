namespace ChushkaWebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChushkaWebApp.Data;
    using ChushkaWebApp.Models;
    using ChushkaWebApp.Services.Contracts;
    using ChushkaWebApp.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IList<BaseOrderViewModel> GetAllOrders()
        {
            var orders = this.context.Orders
                .Select(p => new BaseOrderViewModel()
                {
                    Id = p.Id,
                    Username = p.User.UserName,
                    ProductName = p.Product.Name,
                    OrderedOn = p.OrderedOn
                }).ToList();

            return orders;
        }

        public Order Create(int productId, string userId)
        {
            var order = new Order()
            {
                OrderedOn = DateTime.UtcNow,
                UserId = userId,
                ProductId = productId
            };

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return order;
        }
    }
}
