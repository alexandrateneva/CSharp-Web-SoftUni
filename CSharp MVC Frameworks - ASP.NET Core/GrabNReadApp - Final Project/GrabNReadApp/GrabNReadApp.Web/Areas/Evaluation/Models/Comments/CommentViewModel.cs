using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Web.Constants.Evaluation;

namespace GrabNReadApp.Web.Areas.Evaluation.Models.Comments
{
    public class CommentViewModel
    {
        [Required]
        public int BookId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(CommentsConstants.ContentMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = CommentsConstants.ContentMinLength)]
        public string Content { get; set; }
    }
}
