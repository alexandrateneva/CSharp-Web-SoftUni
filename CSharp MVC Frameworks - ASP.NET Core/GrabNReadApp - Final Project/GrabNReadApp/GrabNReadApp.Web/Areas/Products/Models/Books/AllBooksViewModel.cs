using X.PagedList;

namespace GrabNReadApp.Web.Areas.Products.Models.Books
{
    public class AllBooksViewModel
    {
        public IPagedList<BookBaseViewModel> Books { get; set; }

        public int CurrentPage { get; set; }
        
        public int GenreId { get; set; }

        public string BookTitle { get; set; }
    }
}
