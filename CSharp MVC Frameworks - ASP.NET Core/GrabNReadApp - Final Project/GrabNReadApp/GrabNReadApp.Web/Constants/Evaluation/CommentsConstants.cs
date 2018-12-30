namespace GrabNReadApp.Web.Constants.Evaluation
{
    public static class CommentsConstants
    {
        public const int ContentMinLength = 5;

        public const int ContentMaxLength = 500;

        public const string BookNotFoundMessage = "There is no book with this id.";

        public const string CommentNotFoundMessage = "There is no comment with this id.";

        public const string CommentLengthValidationMessage = "Comment must contain between {0} and {1} symbols.";
    }
}
