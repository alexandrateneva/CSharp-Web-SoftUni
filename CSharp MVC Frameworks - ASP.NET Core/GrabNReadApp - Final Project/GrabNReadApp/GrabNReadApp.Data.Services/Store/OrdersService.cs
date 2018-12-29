using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GrabNReadApp.Data.Services.Store
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IPurchasesService purchasesService;
        private readonly IRentalsServices rentalsServices;

        public OrdersService(IRepository<Order> orderRepository,
             IPurchasesService purchasesService,
             IRentalsServices rentalsServices)
        {
            this.orderRepository = orderRepository;
            this.purchasesService = purchasesService;
            this.rentalsServices = rentalsServices;
        }

        public Order GetCurrentOrderByUserIdWithPurchasesAndRentals(string id)
        {
            var currentOrder = this.orderRepository.All().FirstOrDefault(o => o.CustomerId == id && o.IsFinished == false);
            if (currentOrder != null)
            {
                currentOrder.Purchases = purchasesService.GetAllOrderedPurchasesByOrderId(currentOrder.Id).ToList();
                currentOrder.Rentals = rentalsServices.GetAllOrderedRentalsByOrderId(currentOrder.Id).ToList();
            }

            return currentOrder;
        }

        public Order GetOrderByIdWithPurchasesAndRentals(int id)
        {
            var order = this.orderRepository.All().AsNoTracking().FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Purchases = this.purchasesService.GetAllOrderedPurchasesByOrderId(id).ToList();
                order.Rentals = this.rentalsServices.GetAllOrderedRentalsByOrderId(id).ToList();
            }

            return order;
        }

        public async Task<Order> Create(Order order)
        {
            await this.orderRepository.AddAsync(order);
            await this.orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task EmptyCurrentUserOrder(int orderId, Order order)
        {
            order.OrderedOn = DateTime.UtcNow;
            order.IsFinished = true;
            await this.Update(order);

            var newOrder = new Order()
            {
                CustomerId = order.CustomerId,
                Address = order.Address,
                Phone = order.Phone,
                RecipientName = order.RecipientName
            };
            await this.Create(newOrder);
        }

        public async Task<Order> Update(Order order)
        {
            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();

            return order;
        }

        public IEnumerable<Order> GetAllFinishedOrders()
        {
            var orders = this.orderRepository
                .All()
                .Where(p => p.IsFinished == true);

            return orders;
        }

        public bool Delete(int id)
        {
            var order = this.orderRepository.All().FirstOrDefault(g => g.Id == id);
            if (order != null)
            {
                this.orderRepository.Delete(order);
                this.orderRepository.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
