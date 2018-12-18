using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Products.Contracts;

namespace GrabNReadApp.Data.Services.Products
{
    public class BooksService : IBookService
    {
        private readonly IRepository<Book> bookRepository;

        public BooksService(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<Book> Create(Book book)
        {
            await this.bookRepository.AddAsync(book);
            await this.bookRepository.SaveChangesAsync();

            return book;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            var books = this.bookRepository.All().ToList();

            return books;
        }
    }
}
