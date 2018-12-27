using System.ComponentModel.DataAnnotations;

namespace GrabNReadApp.Web.Areas.Blog.Models.Articles
{
    public class ArticleViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(100000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 250)]
        public string Content { get; set; }
    }
}
