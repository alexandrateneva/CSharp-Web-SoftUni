using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Evaluation
{
    public class CommentsServiceTest
    {
        public CommentsServiceTest()
        {
            TestInitializer.Initialize();
        }

        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Comments.AddRange(GetComments());
            db.SaveChanges();

            var repository = new DbRepository<Comment>(db);
            var commentsService = new CommentsService(repository);

            var newComment = new Comment()
            {
                BookId = 1,
                CreatorId = "AJOEBA829H39JQN7",
                Content = "Some random test comment's content..."
            };

            //Act
            var comment = await commentsService.Create(newComment);

            var commentsCount = db.Comments.Count();

            //Assert
            Assert.Equal(4, commentsCount);
            Assert.NotNull(comment);
            Assert.Same(newComment, comment);
        }

        [Fact]
        public void GetAllCommentsByBookIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Comments.AddRange(GetComments());
            db.SaveChanges();

            var repository = new DbRepository<Comment>(db);
            var commentsService = new CommentsService(repository);

            //Act
            var comments = commentsService.GetAllCommentsByBookId(1).ToList();
            var commentInDb = db.Comments.LastOrDefault(c => c.BookId == 1); // ORDER BY DESC

            var commentsOfNotExistingBook = commentsService.GetAllCommentsByBookId(62891).ToList();

            //Assert
            Assert.Equal(2, comments.Count);
            Assert.Equal(commentInDb.Id, comments.FirstOrDefault().Id);
            Assert.Empty(commentsOfNotExistingBook);
        }

        [Fact]
        public void GetCommentByIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Comments.AddRange(GetComments());
            var newComment = new Comment()
            {
                Id = 17829,
                BookId = 1,
                CreatorId = "HSUA629SHJA619A",
                Content = "Some random test comment's content..."
            };
            db.Comments.Add(newComment);
            db.SaveChanges();

            var repository = new DbRepository<Comment>(db);
            var commentsService = new CommentsService(repository);

            //Act
            var comment = commentsService.GetCommentById(17829);
            var notExistingComment = commentsService.GetCommentById(-67196);

            //Assert
            Assert.Same(newComment, comment);
            Assert.Null(notExistingComment);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Comments.AddRange(GetComments());
            var newComment = new Comment()
            {
                Id = 17829,
                BookId = 1,
                CreatorId = "HSUA629SHJA619A",
                Content = "Some random test comment's content..."
            };
            db.Comments.Add(newComment);
            db.SaveChanges();

            var repository = new DbRepository<Comment>(db);
            var commentsService = new CommentsService(repository);

            //Act
            var isDeletedRealComment = commentsService.Delete(17829);
            var isDeletedNotExistingComment = commentsService.Delete(17829);
            var commentCount = db.Comments.Count();

            //Assert
            Assert.True(isDeletedRealComment);
            Assert.False(isDeletedNotExistingComment);
            Assert.Equal(3, commentCount);
        }

        private Comment[] GetComments()
        {
            return new Comment[]
            {
                new Comment()
                {
                    BookId = 1,
                    CreatorId = "HSUA629SHJA619A",
                    Content =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas viverra semper sagittis.",
                },
                new Comment()
                {
                    BookId = 2,
                    CreatorId = "DALH86SHJA656YO",
                    Content =
                        "Donec maximus condimentum nisi, a facilisis urna finibus at. Vestibulum urna lorem, blandit vitae aliquet vitae, porta id augue.",
                },
                new Comment()
                {
                    BookId = 1,
                    CreatorId = "SUA6189SHJA6JA",
                    Content =
                        "Duis efficitur lacinia massa, a fringilla magna accumsan nec. In sit amet nisl sed dui volutpat semper.",
                },
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
