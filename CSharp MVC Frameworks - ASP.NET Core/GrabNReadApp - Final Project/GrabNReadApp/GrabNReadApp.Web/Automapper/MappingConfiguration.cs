using AutoMapper;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Web.Areas.Evaluation.Models.Comments;
using GrabNReadApp.Web.Areas.Products.Models.Books;
using GrabNReadApp.Web.Areas.Products.Models.Genres;
using GrabNReadApp.Web.Areas.Store.Models.Orders;
using GrabNReadApp.Web.Areas.Store.Models.Purchases;
using GrabNReadApp.Web.Areas.Store.Models.Rentals;

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

            //Rental
            this.CreateMap<RentalViewModel, Rental>();

            //Order
            this.CreateMap<Order, OrderBaseViewModel>();
            this.CreateMap<Order, OrderDetailsViewModel>();
            this.CreateMap<OrderDetailsViewModel, Order>();

            //Comment
            this.CreateMap<CommentViewModel, Comment>();
        }
    }
}
