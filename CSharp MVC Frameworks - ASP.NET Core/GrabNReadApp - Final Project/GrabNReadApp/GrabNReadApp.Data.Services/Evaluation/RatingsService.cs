using System;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation.Contracts;

namespace GrabNReadApp.Data.Services.Evaluation
{
    public class RatingsService : IRatingService
    {
        private const int DefaultRatingValue = 0;

        private readonly IRepository<Vote> ratingsRepository;

        public RatingsService(IRepository<Vote> ratingsRepository)
        {
            this.ratingsRepository = ratingsRepository;
        }

        public async Task<Vote> Create(Vote vote)
        {
            await this.ratingsRepository.AddAsync(vote);
            await this.ratingsRepository.SaveChangesAsync();

            return vote;
        }

        public int GetVoteValueByUserIdAndBookId(string userId, int bookId)
        {
            var vote = this.ratingsRepository.All()
                .FirstOrDefault(r => r.BookId == bookId && r.CreatorId == userId);

            var voteValue = DefaultRatingValue;
            if (vote != null)
            {
                voteValue = vote.VoteValue;
            }

            return voteValue;
        }

        public Vote GetVoteByUserIdAndBookId(string userId, int bookId)
        {
            var vote = this.ratingsRepository.All()
                .FirstOrDefault(r => r.BookId == bookId && r.CreatorId == userId);

            return vote;
        }

        public async Task<Vote> ChangeVote(Vote vote)
        {
            this.ratingsRepository.Update(vote);
            await this.ratingsRepository.SaveChangesAsync();

            return vote;
        }

        public int GetAverageBookRatingByBookId(int bookId)
        {
            var votes = this.ratingsRepository.All()
                .Where(r => r.BookId == bookId);

            var vote = (double)DefaultRatingValue;
            if (votes.Any())
            {
                vote = votes.Average(x => x.VoteValue);
            }
            var result = Math.Round(vote);

            return (int)result;
        }
    }
}
