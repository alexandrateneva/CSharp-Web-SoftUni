namespace PandaWebApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PandaWebApp.Services.Contracts;
    using PandaWebApp.ViewModels;
    using PandaWebApp.ViewModels.Receipts;

    public class ReceiptsController : Controller
    {
        private readonly IReceiptService receiptService;
        private readonly IUserService userService;

        public ReceiptsController(
            IReceiptService receiptService,
            IUserService userService)
        {
            this.receiptService = receiptService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Index(int id)
        {
            var user = this.userService.GetUserByUsername(this.User.Identity.Name);

            var receipts = this.receiptService.GetReceiptsByUserId(user.Id);

            var model = new IndexViewModel
            {
                Receipts = receipts
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var receipt = this.receiptService.GetReceiptDetailsById(id);

            if (receipt == null)
            {
                return this.View("SimpleError", new SimpleErrorViewModel()
                {
                    Message = "Receipt not found or matched."
                });
            }

            return this.View(receipt);
        }
    }
}