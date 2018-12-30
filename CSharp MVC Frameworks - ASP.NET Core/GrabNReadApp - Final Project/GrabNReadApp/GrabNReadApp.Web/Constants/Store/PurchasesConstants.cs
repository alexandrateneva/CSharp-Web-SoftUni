namespace GrabNReadApp.Web.Constants.Store
{
    public static class PurchasesConstants
    {
        //Controller
        public const int DefaultCount = 1;

        public const string SuccessMessageTitle = "Success!";

        public const string RedirectedMessageTitle = "You were redirected!";

        public const string RedirectedMessage = "To make order, please first Login!";

        public const string SuccessfullyAddedToCartMessage = "Тhe book has been successfully added to your cart.";

        public const string NotAccessMessage = "Insufficient access rights to perform the operation.";

        public const string ErrorMessageForNotFound = "There is no purchase with id - {0}.";

        public const string SuccessMessageForDelete = "Тhe purchase has been successfully removed.";

        //Models
        public const int BooksCountMinValue = 1;

        public const int BooksCountMaxValue = 20;

        public const string ErrorMessageForBookCount = "Уou can buy from 1 to 20 pieces of this book.";
    }
}
