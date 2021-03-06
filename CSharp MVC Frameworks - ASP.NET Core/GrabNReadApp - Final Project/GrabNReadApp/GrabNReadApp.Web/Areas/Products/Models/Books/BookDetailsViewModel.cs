﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Data.Models.Enums;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Web.Areas.Evaluation.Models.Comments;

namespace GrabNReadApp.Web.Areas.Products.Models.Books
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Price per Day")]
        public decimal PricePerDay { get; set; }

        [Display(Name = "Number of Pages")]
        public int Pages { get; set; }

        [Display(Name = "Cover Type")]
        public CoverType CoverType { get; set; }

        public string Description { get; set; }

        public string CoverImage { get; set; }

        public int GenreId { get; set; }

        public int UserVote { get; set; }

        public decimal AverageRating { get; set; }

        public CommentViewModel CommentViewModel { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
