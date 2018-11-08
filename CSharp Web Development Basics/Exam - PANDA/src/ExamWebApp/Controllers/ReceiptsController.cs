namespace ExamWebApp.Controllers
{
    using System.Linq;
    using ExamWebApp.ViewModels.Receipts;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class ReceiptsController : BaseController
    {
        [Authorize]
        public IHttpResponse Index(int id)
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            var receipts = this.Db.Receipts
                .Where(x => x.RecipientId == user.Id)
                .Select(p => new BaseReceiptViewModel()
                {
                    Id = p.Id,
                    Fee = p.Fee,
                    IssuedOn = p.IssuedOn,
                    RecipientName = p.Recipient.Username
                })
                .ToList();

            var model = new IndexViewModel
            {
                Receipts = receipts
            };

            return this.View(model);
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var receipt = this.Db.Receipts
                .Where(x => x.Id == id)
                .Select(p => new DetailsReceiptViewModel()
                {
                    Id = p.Id,
                    Fee = p.Fee,
                    IssuedOn = p.IssuedOn,
                    RecipientName = p.Recipient.Username,
                    Address = p.Package.ShippingAddress,
                    Weight = p.Package.Weight,
                    Description = p.Package.Description
                })
                .FirstOrDefault();

            if (receipt == null)
            {
                return this.BadRequestError("Invalid receipt id.");
            }

            return this.View(receipt);
        }
    }
}
