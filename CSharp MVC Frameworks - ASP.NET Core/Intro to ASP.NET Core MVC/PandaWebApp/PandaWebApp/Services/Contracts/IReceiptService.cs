namespace PandaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using PandaWebApp.Models;
    using PandaWebApp.ViewModels.Receipts;

    public interface IReceiptService
    {
        Receipt CreateReceipt(Package package, ApplicationUser user);

        IList<BaseReceiptViewModel> GetReceiptsByUserId(string userId);

        DetailsReceiptViewModel GetReceiptDetailsById(int id);
    }
}
