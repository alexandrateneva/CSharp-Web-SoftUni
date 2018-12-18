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

        public async Task<Genre> Edit(Genre genre)
        {
            this.genreRepository.Update(genre);
            await this.genreRepository.SaveChangesAsync();

            return genre;
        }

        public async Task<bool> Delete(int id)
        {
            var genre = GetAllGenres().FirstOrDefault(g => g.Id == id);
            if (genre != null)
            {
                this.genreRepository.Delete(genre);
                await this.genreRepository.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            var genres = this.genreRepository.All().ToList();

            return genres;
        }

        public async Task<Genre> GetGenreById(int id)
        {
            var genre = await this.genreRepository.GetByIdAsync(id);

            return genre;
        }
    }
}
