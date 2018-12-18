using System.Collections.Generic;
using System.Linq;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Products.Contracts;
using System.Threading.Tasks;

namespace GrabNReadApp.Data.Services.Products
{
    public class GenresService : IGenreService
    {
        private readonly IRepository<Genre> genreRepository;

        public GenresService(IRepository<Genre> genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<Genre> Create(Genre genre)
        {
            await this.genreRepository.AddAsync(genre);
            await this.genreRepository.SaveChangesAsync();

            return genre;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            var genres = this.genreRepository.All().ToList();

            return genres;
        }
    }
}
