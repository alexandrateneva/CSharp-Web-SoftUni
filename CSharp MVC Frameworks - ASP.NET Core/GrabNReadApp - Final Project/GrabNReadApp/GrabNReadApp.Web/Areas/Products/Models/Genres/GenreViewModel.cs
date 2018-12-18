using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GrabNReadApp.Web.Areas.Products.Models.Genres
{
    public class GenreViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Genre Image")]
        public IFormFile ImageFile { get; set; }

        [Url]
        public string Image { get; set; }
    }
}
