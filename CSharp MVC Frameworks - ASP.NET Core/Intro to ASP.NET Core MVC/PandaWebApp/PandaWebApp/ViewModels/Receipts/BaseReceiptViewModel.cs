namespace PandaWebApp.ViewModels.Receipts
{
    using System;
    using System.Globalization;

    public class BaseReceiptViewModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        public string RecipientName { get; set; }

        public string IssuedOnDate => this.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }
}
