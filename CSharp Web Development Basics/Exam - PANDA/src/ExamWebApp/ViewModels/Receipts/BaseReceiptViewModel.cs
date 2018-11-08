namespace ExamWebApp.ViewModels.Receipts
{
    using System;

    public class BaseReceiptViewModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }
        
        public string RecipientName { get; set; }

        public string IssuedOnDate => String.Format("{0:dd/MM/yyyy}", this.IssuedOn);
    }
}
