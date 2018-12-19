using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace GrabNReadApp.Web.Areas.Products.Models.Books
{
    public class BookEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Author { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(0.01, 1000000.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(0.01, 1000000.00)]
        [Display(Name = "Price per Day")]
        public decimal PricePerDay { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "Number of Pages")]
        public int Pages { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public CoverType CoverType { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 20)]
        public string Description { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImageFile { get; set; }

        [Url]
        public string CoverImage { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }
    }
}
