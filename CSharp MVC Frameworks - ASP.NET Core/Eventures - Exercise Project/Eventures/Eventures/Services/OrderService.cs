namespace Eventures.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;
    using global::AutoMapper;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Order CreateOrder(CreateOrderViewModel model)
        {
            var order = this.mapper.Map<Order>(model);

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return order;
        }

        public int GetTotalBoughtTicketsCountByEventIdFromCurrentUser(string eventId, string userId)
        {
            var ticketsCount = this.context.Orders
                .Where(o => o.EventId == eventId && o.CustomerId == userId)
                .Sum(x => x.TicketsCount);

            return ticketsCount;
        }

        public IEnumerable<Order> GetAllOrdersByUserId(string id)
        {
            var orders = this.context.Orders.Where(o => o.CustomerId == id);               

            return orders;
        }

        public IEnumerable<BaseOrderViewModel> GetAllOrders()
        {
            var orders = this.context.Orders
                .Include(o => o.Event)
                .Include(o => o.Customer)
                .Select(e => this.mapper.Map<BaseOrderViewModel>(e));

            return orders;
        }
    }
}
