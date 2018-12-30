namespace GrabNReadApp.Web.Constants.Store
{
    public static class RentalsConstants
    {
        public const int DefaultCount = 1;

        public const int DefaultPeriodInDays = 1;

        public const int MaximumPeriodInDays = 30;

        public const string SuccessMessageTitle = "Success!";

        public const string ErrorMessageTitle = "Error!";

        public const string RedirectedMessageTitle = "You were redirected!";

        public const string RedirectedMessage = "To make order, please first Login!";

        public const string PastTimeErrorMessage = "You can not rent a book for a past times.";

        public const string StartDateGreaterThanEndDateErrorMessage = "End date must be greater than the start date.";

        public const string MaximumPeriodErrorMessage = "You can rent the book for a maximum period of {0} days.";

        public const string SuccessfullyAddedToCartMessage = "Тhe book has been successfully added to your cart.";

        public const string NotAccessMessage = "Insufficient access rights to perform the operation.";

        public const string ErrorMessageForNotFound = "There is no rental with id - {0}.";

        public const string SuccessMessageForDelete = "Тhe rental has been successfully removed.";
    }
}
