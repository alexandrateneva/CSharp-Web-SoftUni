using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Services.Blog;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Blog
{
    public class ArticlesServiceTest
    {
        public ArticlesServiceTest()
        {
            TestInitializer.Initialize();
        }

        [Fact]
        public void GetAllArticlesMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Articles.AddRange(GetArticles());
            db.SaveChanges();

            var repository = new DbRepository<Article>(db);
            var articleService = new ArticleService(repository);

            //Act
            var articlesCount = articleService.GetAllArticles().ToList().Count;
            var firstArticleInDb = db.Articles.FirstOrDefault();
            var firstArticle = articleService.GetAllArticles().ToList().FirstOrDefault();

            //Assert
            Assert.Equal(3, articlesCount);
            Assert.NotNull(firstArticle);
            Assert.Same(firstArticleInDb, firstArticle);
        }

        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Articles.AddRange(GetArticles());
            db.SaveChanges();

            var repository = new DbRepository<Article>(db);
            var articleService = new ArticleService(repository);

            var newArticle = new Article()
            {
                Title = "Lorem Ipsum",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas viverra semper sagittis...",
                PublishedOn = DateTime.UtcNow,
                AuthorId = "1001",
                IsApprovedByAdmin = false
            };

            //Act
            var enteredArticle = await articleService.Create(newArticle);

            var articlesCount = db.Articles.Count();

            //Assert
            Assert.Equal(4, articlesCount);
            Assert.NotNull(enteredArticle);
            Assert.Same(newArticle, enteredArticle);
        }

        [Fact]
        public void GetByIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Articles.AddRange(GetArticles());
            var newArticle = new Article
            {
                Id = 167282,
                Title = "Lorem Ipsum",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas viverra semper sagittis...",
                PublishedOn = DateTime.UtcNow,
                AuthorId = "1001",
                IsApprovedByAdmin = false
            };
            db.Articles.Add(newArticle);
            db.SaveChanges();

            var repository = new DbRepository<Article>(db);
            var articleService = new ArticleService(repository);

            //Act
            var article = articleService.GetArticleById(167282);
            var notExistingArticle = articleService.GetArticleById(62891);

            //Assert
            Assert.NotNull(article);
            Assert.Null(notExistingArticle);
            Assert.Same(newArticle, article);
        }

        [Fact]
        public async void EditMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Articles.AddRange(GetArticles());
            var article = new Article
            {
                Id = 167282,
                Title = "Lorem Ipsum",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas viverra semper sagittis...",
                PublishedOn = DateTime.UtcNow,
                AuthorId = "1001",
                IsApprovedByAdmin = false
            };
            db.Articles.Add(article);
            db.SaveChanges();

            var repository = new DbRepository<Article>(db);
            var articleService = new ArticleService(repository);

            article.Title = "Lorem Ipsum Changed";
            article.Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            article.IsApprovedByAdmin = true;
            
            //Act
            var articleInDb = await articleService.Edit(article);
            var articlesCount = db.Articles.Count();

            //Assert
            Assert.NotNull(articleInDb);
            Assert.Same(article, articleInDb);
            Assert.Equal(4, articlesCount);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Articles.AddRange(GetArticles());
            var article = new Article
            {
                Id = 167282,
                Title = "Lorem Ipsum",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas viverra semper sagittis...",
                PublishedOn = DateTime.UtcNow,
                AuthorId = "1001",
                IsApprovedByAdmin = false
            };
            db.Articles.Add(article);
            db.SaveChanges();

            var repository = new DbRepository<Article>(db);
            var articleService = new ArticleService(repository);
            
            //Act
            var isDeletedRealArticle = articleService.Delete(167282);
            var isDeletedNotExistingArticle = articleService.Delete(167282);
            var articlesCount = db.Articles.Count();

            //Assert
            Assert.True(isDeletedRealArticle);
            Assert.False(isDeletedNotExistingArticle);
            Assert.Equal(3, articlesCount);
        }

        private Article[] GetArticles()
        {
            return new Article[]
            {
                new Article
                {
                    Title = "Lorem Ipsum 1",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas viverra semper sagittis...",
                    PublishedOn = DateTime.UtcNow,
                    AuthorId = "1001",
                    IsApprovedByAdmin = false
                },
                new Article
                {
                    Title = "Lorem Ipsum 2",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                              "Maecenas viverra semper sagittis. Vestibulum ante ipsum primis in " +
                              "faucibus orci luctus et ultrices posuere cubilia Curae; Aenean eleifend quis massa sit amet interdum. " +
                              "Nullam tempor ornare augue eget efficitur. Donec molestie tristique egestas. Aliquam " +
                              "consectetur nunc quis tellus feugiat, vel viverra ligula mollis. Morbi elit enim, scelerisque eu" +
                              " consequat eget, auctor in ipsum. Morbi blandit tristique finibus. Integer suscipit velit non maximus" +
                              " molestie. Duis efficitur lacinia massa, a fringilla magna accumsan nec. In sit amet nisl sed dui " +
                              "volutpat semper. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis" +
                              " egestas. Nam malesuada lacinia nunc, sed egestas velit. Proin vestibulum, orci eget interdum efficitur," +
                              " turpis eros scelerisque ipsum, imperdiet ornare lectus augue id libero. Sed id rutrum sapien, quis vestibulum augue.",
                    PublishedOn = DateTime.UtcNow,
                    AuthorId = "1002",
                    IsApprovedByAdmin = false
                },
                new Article
                {
                    Title = "Lorem Ipsum 3",
                    Content = "Donec eu eros in enim ultricies ullamcorper nec ac libero. In pulvinar sapien risus, ut accumsan erat egestas et. " +
                              "Fusce rhoncus purus non malesuada blandit. Nullam fermentum justo eget enim porta, ut ultrices orci " +
                              "porta. Duis hendrerit, magna nec bibendum malesuada, tellus mauris iaculis mi, eu dignissim mi enim egestas nibh. " +
                              "Sed eget eros magna. Vestibulum accumsan congue erat, a volutpat justo fringilla ac. Integer tempor ornare elit at gravida. " +
                              "Sed fermentum venenatis tortor, sed congue enim commodo sit amet.",
                    PublishedOn = DateTime.UtcNow,
                    AuthorId = "1003",
                    IsApprovedByAdmin = true
                }
            };
        }

        private GrabNReadAppContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<GrabNReadAppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GrabNReadAppContext(dbOptions);
        }
    }
}
