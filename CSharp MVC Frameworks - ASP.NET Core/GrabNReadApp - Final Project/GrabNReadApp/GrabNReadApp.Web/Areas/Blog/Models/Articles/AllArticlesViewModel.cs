using X.PagedList;

namespace GrabNReadApp.Web.Areas.Blog.Models.Articles
{
    public class AllArticlesViewModel
    {
        public IPagedList<ArticleBaseViewModel> Articles { get; set; }

        public int CurrentPage { get; set; }
    }
}
