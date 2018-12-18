using AutoMapper;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Web.Areas.Products.Models.Books;
using GrabNReadApp.Web.Areas.Products.Models.Genres;

namespace GrabNReadApp.Web.Automapper
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            this.CreateMap<GenreViewModel, Genre>();
            this.CreateMap<BookViewModel, Book>();
        }
    }
}
