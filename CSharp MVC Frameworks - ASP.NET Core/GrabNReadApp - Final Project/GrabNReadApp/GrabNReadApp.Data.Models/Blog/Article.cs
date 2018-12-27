using System;
using System.Security.AccessControl;

namespace GrabNReadApp.Data.Models.Blog
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
        public GrabNReadAppUser Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public bool IsApprovedByAdmin { get; set; }
    }
}
