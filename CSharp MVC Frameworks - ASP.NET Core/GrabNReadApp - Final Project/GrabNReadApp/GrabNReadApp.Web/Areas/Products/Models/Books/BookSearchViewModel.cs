using System.ComponentModel.DataAnnotations;

namespace GrabNReadApp.Web.Areas.Products.Models.Books
{
    public class BookSearchViewModel
    {
        [Required]
        public string BookTitle { get; set; }
    }
}
