using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Data.Services.Products.Contracts
{
    public interface IBookService
    {
        Task<Book> Create(Book book);

        IEnumerable<Book> GetAllBooks();
    }
}
