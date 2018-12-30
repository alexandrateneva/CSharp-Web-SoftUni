namespace GrabNReadApp.Web.Constants.Store
{
    public static class OrdersConstants
    {
        // Controller
        public const int FirstPageNumber = 1;

        public const int OrdersPerPage = 5;

        public const string ErrorMessageForDelete = "Delete failed.";

        public const string OrderSuccessMessageTitle = "Thank you!";

        public const string SuccessMessageForOrder = "Your order was successful.";

        public const string SuccessMessageTitle = "Success!";

        public const string SuccessMessageForDelete = "The order was successfully deleted.";

        public const string ErrorMessageForNotFound = "There is no order with id - {0}.";

        //Models
        public const int AddressMinLength = 10;

        public const int AddressMaxLength = 200;

        public const int RecipientNameMinLength = 5;

        public const int RecipientNameMaxLength = 100;

        public const double DeliveryMinValue = 0.01;

        public const double DeliveryMaxValue = 1000.00;
    }
}
