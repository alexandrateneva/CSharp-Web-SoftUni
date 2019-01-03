using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Evaluation
{
    public class RatingsServiceTest
    {
        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Votes.AddRange(GetVotes());
            db.SaveChanges();

            var repository = new DbRepository<Vote>(db);
            var ratingsService = new RatingsService(repository);

            var newVote = new Vote()
            {
                BookId = 1,
                CreatorId = "AJOEBA829H39JQN7",
                VoteValue = 5
            };

            //Act
            var vote = await ratingsService.Create(newVote);
            var votesCount = db.Votes.Count();

            //Assert
            Assert.Equal(4, votesCount);
            Assert.NotNull(vote);
            Assert.Same(newVote, vote);
        }

        [Fact]
        public void GetVoteValueByUserIdAndBookIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Votes.AddRange(GetVotes());
            db.SaveChanges();

            var repository = new DbRepository<Vote>(db);
            var ratingsService = new RatingsService(repository);

            //Act
            var vote = ratingsService.GetVoteValueByUserIdAndBookId("HSUA629SHJA619A", 1);
            var voteWithNotExistingBookId = ratingsService.GetVoteValueByUserIdAndBookId("HSUA629SHJA619A", 6829);
            var voteWithNotExistingUserId = ratingsService.GetVoteValueByUserIdAndBookId("XMJK629SHJA628B", 1);

            //Assert
            Assert.Equal(3, vote);
            Assert.Equal(0, voteWithNotExistingBookId);
            Assert.Equal(0, voteWithNotExistingUserId);
        }

        [Fact]
        public void GetVoteByUserIdAndBookIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Votes.AddRange(GetVotes());
            db.SaveChanges();

            var repository = new DbRepository<Vote>(db);
            var ratingsService = new RatingsService(repository);

            //Act
            var vote = ratingsService.GetVoteByUserIdAndBookId("HSUA629SHJA619A", 1);
            var voteWithNotExistingBookId = ratingsService.GetVoteByUserIdAndBookId("HSUA629SHJA619A", 6829);
            var voteWithNotExistingUserId = ratingsService.GetVoteByUserIdAndBookId("XMJK629SHJA628B", 1);

            //Assert
            Assert.Equal(3, vote.VoteValue);
            Assert.Null(voteWithNotExistingBookId);
            Assert.Null(voteWithNotExistingUserId);
        }

        [Fact]
        public async void ChangeVoteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Votes.AddRange(GetVotes());
            var newVote = new Vote()
            {
                BookId = 1,
                CreatorId = "AJOEBA829H39JQN7",
                VoteValue = 5
            };
            db.Votes.Add(newVote);
            db.SaveChanges();

            var repository = new DbRepository<Vote>(db);
            var ratingsService = new RatingsService(repository);

            //Act
            newVote.VoteValue = 3;
            var vote = await ratingsService.ChangeVote(newVote);
            var votesCount = db.Votes.Count();

            //Assert
            Assert.NotNull(vote);
            Assert.Same(newVote, vote);
            Assert.Equal(4, votesCount);
        }

        [Fact]
        public void GetAverageBookRatingByBookIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Votes.AddRange(GetVotes());
            db.SaveChanges();

            var repository = new DbRepository<Vote>(db);
            var ratingsService = new RatingsService(repository);

            //Act
            var averageBookRatingWithManyVotes = ratingsService.GetAverageBookRatingByBookId(1);
            var averageBookRatingWithOneVote = ratingsService.GetAverageBookRatingByBookId(2);
            var averageRatingOfNotExistingBook = ratingsService.GetAverageBookRatingByBookId(6859);

            //Assert
            Assert.Equal(4, averageBookRatingWithManyVotes);
            Assert.Equal(5, averageBookRatingWithOneVote);
            Assert.Equal(0, averageRatingOfNotExistingBook);
        }

        private Vote[] GetVotes()
        {
            return new Vote[]
            {
               new Vote()
               {
                   BookId = 1,
                   CreatorId = "HSUA629SHJA619A",
                   VoteValue = 3
               },
               new Vote()
               {
                   BookId = 2,
                   CreatorId = "DALH86SHJA656YO",
                   VoteValue = 5
               },
               new Vote()
               {
                   BookId = 1,
                   CreatorId = "SUA6189SHJA6JA",
                   VoteValue = 4
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
