using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Web.Constants.Blog;

namespace GrabNReadApp.Web.Areas.Blog.Models.Articles
{
    public class ArticleViewModel
    {
        [Required]
        [StringLength(ArticleConstants.TitleMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ArticleConstants.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ArticleConstants.ContentMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = ArticleConstants.ContentMinLength)]
        public string Content { get; set; }
    }
}
