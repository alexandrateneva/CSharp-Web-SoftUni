using System.ComponentModel.DataAnnotations;

namespace GrabNReadApp.Web.Areas.Evaluation.Models.Comments
{
    public class CommentViewModel
    {
        public string CreatorId { get; set; }
        
        [Required]
        public int BookId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Content { get; set; }
    }
}
