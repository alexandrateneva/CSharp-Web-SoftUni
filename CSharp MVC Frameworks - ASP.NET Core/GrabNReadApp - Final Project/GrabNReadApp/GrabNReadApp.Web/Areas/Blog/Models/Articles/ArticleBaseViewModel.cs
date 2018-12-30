using System;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Web.Constants.Blog;

namespace GrabNReadApp.Web.Areas.Blog.Models.Articles
{
    public class ArticleBaseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
        public GrabNReadAppUser Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public bool IsApprovedByAdmin { get; set; }

        public string ShortContent => this.Content.Substring(0, ArticleConstants.ShortContentLength) + ArticleConstants.EndOfShortContentString;
    }
}
