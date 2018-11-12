namespace PandaWebApp.Models
{
    using System;

    public class Receipt
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}
