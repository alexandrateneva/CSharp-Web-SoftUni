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

        public async Task<Book> Edit(Book book)
        {
            this.bookRepository.Update(book);
            await this.bookRepository.SaveChangesAsync();

            return book;
        }

        public bool Delete(int id)
        {
            var book = this.bookRepository.All().FirstOrDefault(g => g.Id == id);
            if (book != null)
            {
                this.bookRepository.Delete(book);
                this.bookRepository.SaveChanges();

                return true;
            }
            return false;
        }
        
        public async Task<Book> GetBookById(int id)
        {
            var book = await this.bookRepository.GetByIdAsync(id);

            return book;
        }
    }
}
