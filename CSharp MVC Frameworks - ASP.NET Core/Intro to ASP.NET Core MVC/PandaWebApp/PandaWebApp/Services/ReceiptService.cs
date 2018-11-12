namespace PandaWebApp.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using PandaWebApp.Data;
    using PandaWebApp.Models;
    using PandaWebApp.Services.Contracts;
    using PandaWebApp.ViewModels.Receipts;

    public class ReceiptService : IReceiptService
    {
        private readonly ApplicationDbContext context;

        public ReceiptService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Receipt CreateReceipt(Package package, ApplicationUser user)
        {
            var receipt = new Receipt()
            {
                Recipient = user,
                Package = package,
                IssuedOn = DateTime.UtcNow,
                Fee = package.Weight * (decimal)2.67
            };
            this.context.Receipts.Add(receipt);
            this.context.SaveChanges();

            return receipt;
        }

        public IList<BaseReceiptViewModel> GetReceiptsByUserId(string userId)
        {
            var receipts = this.context.Receipts
                .Where(x => x.RecipientId == userId)
                .Select(p => new BaseReceiptViewModel()
                {
                    Id = p.Id,
                    Fee = p.Fee,
                    IssuedOn = p.IssuedOn,
                    RecipientName = p.Recipient.UserName
                })
                .ToList();

            return receipts;
        }

        public DetailsReceiptViewModel GetReceiptDetailsById(int id)
        {
            var receipt = this.context.Receipts
                .Where(x => x.Id == id)
                .Select(p => new DetailsReceiptViewModel()
                {
                    Id = p.Id,
                    Fee = p.Fee,
                    IssuedOn = p.IssuedOn,
                    RecipientName = p.Recipient.UserName,
                    Address = p.Package.ShippingAddress,
                    Weight = p.Package.Weight,
                    Description = p.Package.Description
                })
                .FirstOrDefault();

            return receipt;
        }

    }
}
