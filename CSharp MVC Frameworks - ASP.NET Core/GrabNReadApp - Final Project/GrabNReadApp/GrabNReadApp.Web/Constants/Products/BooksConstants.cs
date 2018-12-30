using System;

namespace GrabNReadApp.Web.Constants.Products
{
    public static class BooksConstants
    {
        // Controller
        public const int FirstPageNumber = 1;

        public const int BooksPerPage = 4;

        public const string ErrorMessageForDelete = "Delete failed.";

        public const string SuccessMessageTitle = "Success!";

        public const string SuccessMessageForCreate = "The book was successfully created.";

        public const string SuccessMessageForEdit = "The book was successfully edited.";

        public const string SuccessMessageForDelete = "The book was successfully deleted.";

        public const string ErrorMessageForNotFound = "There is no book with id - {0}.";
        
        public const string ErrorMessageTitleForSearch = "Book title is required!";

        public const string ErrorMessageForSearch = "To search, please enter a book title.";
        
        //Models
        public const int TitleMinLength = 5;

        public const int TitleMaxLength = 100;

        public const int AuthorMinLength = 5;

        public const int AuthorMaxLength = 100;

        public const double PriceMinValue = 0.01;

        public const double PriceMaxValue = 1000000.00;

        public const int NumberOfPagesMinValue = 1;

        public const int NumberOfPagesMaxValue = Int32.MaxValue;

        public const int DescriptionMinLength = 20;

        public const int DescriptionMaxLength = 1000;

        public const string EmptyCollectionMessage = "Sorry, there are no books.";

        public const string NoBooksByGenreMessage = "There are no books of this genre.";

        public const string NoBooksByTitleSearchMessage = "There are no books with this title.";
    }
}
