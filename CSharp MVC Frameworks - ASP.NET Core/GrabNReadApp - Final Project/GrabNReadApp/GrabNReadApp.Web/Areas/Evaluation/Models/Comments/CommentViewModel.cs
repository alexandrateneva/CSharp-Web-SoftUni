using System.ComponentModel.DataAnnotations;

namespace GrabNReadApp.Web.Areas.Evaluation.Models.Comments
{
    public class CommentViewModel
    {
        [Required]
        public int BookId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Content { get; set; }
    }
}
