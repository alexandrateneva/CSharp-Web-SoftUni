using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Data.Services.Products.Contracts
{
    public interface IGenreService
    {
        Task<Genre> Create(Genre genre);

        IEnumerable<Genre> GetAllGenres();
    }
}
