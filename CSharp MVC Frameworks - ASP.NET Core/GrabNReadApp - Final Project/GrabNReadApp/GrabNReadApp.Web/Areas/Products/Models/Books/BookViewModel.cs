using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Data.Models.Enums;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Web.Constants.Products;
using Microsoft.AspNetCore.Http;

namespace GrabNReadApp.Web.Areas.Products.Models.Books
{
    public class BookViewModel
    {
        [Required]
        [StringLength(BooksConstants.TitleMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = BooksConstants.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(BooksConstants.AuthorMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = BooksConstants.AuthorMinLength)]
        public string Author { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(BooksConstants.PriceMinValue, BooksConstants.PriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(BooksConstants.PriceMinValue, BooksConstants.PriceMaxValue)]
        [Display(Name = "Price per Day")]
        public decimal PricePerDay { get; set; }

        [Required]
        [Range(BooksConstants.NumberOfPagesMinValue, BooksConstants.NumberOfPagesMaxValue)]
        [Display(Name = "Number of Pages")]
        public int Pages { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public CoverType CoverType { get; set; }

        [Required]
        [StringLength(BooksConstants.DescriptionMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = BooksConstants.DescriptionMinLength)]
        public string Description { get; set; }
        
        [Required]
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
