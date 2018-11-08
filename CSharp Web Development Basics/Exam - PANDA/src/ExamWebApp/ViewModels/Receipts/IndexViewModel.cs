namespace ExamWebApp.ViewModels.Receipts
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Receipts = new HashSet<BaseReceiptViewModel>();
        }

        public ICollection<BaseReceiptViewModel> Receipts { get; set; }
    }
}
