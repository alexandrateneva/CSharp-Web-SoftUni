using AutoMapper;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Web.Areas.Products.Models.Books;
using GrabNReadApp.Web.Areas.Products.Models.Genres;
using GrabNReadApp.Web.Areas.Store.Models.Purchases;

namespace GrabNReadApp.Web.Automapper
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            //Genre
            this.CreateMap<GenreViewModel, Genre>();
            this.CreateMap<Genre, GenreBaseViewModel>();
            this.CreateMap<Genre, GenreEditViewModel>();
            this.CreateMap<GenreEditViewModel, Genre>();
            this.CreateMap<Genre, GenreDeleteViewModel>();

            //Book
            this.CreateMap<BookViewModel, Book>();
            this.CreateMap<Book, BookBaseViewModel>();
            this.CreateMap<Book, BookEditViewModel>();
            this.CreateMap<BookEditViewModel, Book>();
            this.CreateMap<Book, BookDetailsViewModel>();

            //Purchase
            this.CreateMap<PurchaseViewModel, Purchase>();
        }
    }
}
