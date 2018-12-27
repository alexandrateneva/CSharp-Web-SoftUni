using GrabNReadApp.Data.Models;

namespace GrabNReadApp.Web.Areas.Blog.Models.Articles
{
    public class ArticleDeleteViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }
        public GrabNReadAppUser Author { get; set; }
    }
}
