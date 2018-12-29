using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Data.Services.Products.Contracts
{
    public interface IBookService
    {
        Task<Book> Create(Book book);
        
        Task<Book> Edit(Book book);

        bool Delete(int id);

        IEnumerable<Book> GetAllBooks();

        Task<Book> GetBookById(int id);

        IEnumerable<Book> GetBooksByTitle(string title);
    }
}
