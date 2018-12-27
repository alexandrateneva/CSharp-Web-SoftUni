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
            var books = this.articleRepository.All().Include(a => a.Author);

            return books;
        }

        public Article GetArticleById(int id)
        {
            var article = this.articleRepository.All().Include(a => a.Author).FirstOrDefault(a => a.Id == id);

            return article;
        }

        public async Task<Article> Edit(Article article)
        {
            this.articleRepository.Update(article);
            await this.articleRepository.SaveChangesAsync();

            return article;
        }

        public bool Delete(int id)
        {
            var article = this.articleRepository.All().FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                this.articleRepository.Delete(article);
                this.articleRepository.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
