using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Services.Blog.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Article> GetAllArticles()
        {
            var books = this.articleRepository.All();

            return books;
        }

        public Article GetArticleById(int id)
        {
            var article = this.articleRepository.All().Include(a => a.Author).FirstOrDefault(a => a.Id == id);

            return article;
        }
    }
}
