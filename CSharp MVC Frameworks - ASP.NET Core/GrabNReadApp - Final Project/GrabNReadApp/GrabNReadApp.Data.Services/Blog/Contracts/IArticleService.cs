using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Blog;

namespace GrabNReadApp.Data.Services.Blog.Contracts
{
    public interface IArticleService
    {
        Task<Article> Create(Article article);

        IEnumerable<Article> GetAllArticles();

        Article GetArticleById(int id);

        Task<Article> Edit(Article article);

        bool Delete(int id);
    }
}
