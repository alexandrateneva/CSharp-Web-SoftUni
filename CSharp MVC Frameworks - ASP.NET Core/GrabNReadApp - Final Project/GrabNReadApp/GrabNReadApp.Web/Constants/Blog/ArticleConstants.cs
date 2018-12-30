namespace GrabNReadApp.Web.Constants.Blog
{
    public static class ArticleConstants
    {
        // Controller
        public const int FirstPageNumber = 1;

        public const int ArticlesPerPage = 6;

        public const string SuccessMessageTitle = "Success!";
        
        public const string RedirectedMessageTitle = "You were redirected!";

        public const string NotAccessMessage = "Please login, you don't have access rights.";

        public const string SuccessMessageForCreate = "The article was successfully created.";

        public const string SuccessMessageForEdit = "The article was successfully edited.";

        public const string SuccessMessageForDelete = "The article was successfully deleted.";

        public const string SuccessMessageForChangeStatus = "You changed the article status successfully.";

        public const string ErrorMessageForNotFound = "There is no article with id - {0}.";

        //Models
        public const int ShortContentLength = 50;

        public const string EndOfShortContentString = "...";

        public const int TitleMinLength = 5;

        public const int TitleMaxLength = 100;

        public const int ContentMinLength = 250;

        public const int ContentMaxLength = 100000;

        public const string EmptyCollectionMessage = "Sorry, there are no articles.";
    }
}
