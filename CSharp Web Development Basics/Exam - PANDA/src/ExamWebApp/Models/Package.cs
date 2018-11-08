﻿namespace ExamWebApp.Models
{
    using System;

    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public int RecipientId { get; set; }
        public User Recipient { get; set; }

        public int? ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
    }
}
