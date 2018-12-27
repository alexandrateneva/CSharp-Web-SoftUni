using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Blog;

namespace GrabNReadApp.Data.Services.Blog.Contracts
{
    public interface IArticleService
    {
        Task<Article> Create(Article article);
    }
}
