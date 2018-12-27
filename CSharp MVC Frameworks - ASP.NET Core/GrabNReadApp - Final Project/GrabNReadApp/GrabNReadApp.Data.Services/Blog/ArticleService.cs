using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Services.Blog.Contracts;

namespace GrabNReadApp.Data.Services.Blog
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> articleRepository;

        public ArticleService(IRepository<Article> articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public async Task<Article> Create(Article article)
        {
            await this.articleRepository.AddAsync(article);
            await this.articleRepository.SaveChangesAsync();

            return article;
        }
    }
}
