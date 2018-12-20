using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GrabNReadApp.Data.Services.Store
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IPurchasesService purchasesService;
        private readonly IRentalsServices rentalsServices;

        public OrdersService(IRepository<Order> orderRepository, IPurchasesService purchasesService, IRentalsServices rentalsServices)
        {
            this.orderRepository = orderRepository;
            this.purchasesService = purchasesService;
            this.rentalsServices = rentalsServices;
        }

        public Order GetCurrentOrderByUserIdWithPurchasesAndRentals(string id)
        {
            var currentOrder = this.orderRepository.All().FirstOrDefault(o => o.CustomerId == id);
            if (currentOrder != null)
            {
                currentOrder.Purchases = purchasesService.GetAllNotOrderedPurchasesByOrderId(currentOrder.Id).ToList();
                currentOrder.Rentals = rentalsServices.GetAllNotOrderedRentalsByOrderId(currentOrder.Id).ToList();
            }

            return currentOrder;
        }

        public Order GetOrderById(int id)
        {
            var order = this.orderRepository.All().AsNoTracking().FirstOrDefault(o => o.Id == id);

            return order;
        }

        public async Task<Order> Create(Order order)
        {
            await this.orderRepository.AddAsync(order);
            await this.orderRepository.SaveChangesAsync();

            return order;
        }

        public async Task EmptyCurrentOrder(int orderId)
        {
            var purchases = purchasesService.GetAllNotOrderedPurchasesByOrderId(orderId).ToList();
            var rentals = rentalsServices.GetAllNotOrderedRentalsByOrderId(orderId).ToList();

            foreach (var purchase in purchases)
            {
                await this.purchasesService.MakePurchaseOrdered(purchase);
            }

            foreach (var rental in rentals)
            {
                await this.rentalsServices.MakeRentalOrdered(rental);
            }
        }

        public async Task<Order> Update(Order order)
        {
            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();

            return order;
        }
    }
}
