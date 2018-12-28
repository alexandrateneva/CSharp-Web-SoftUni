using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Evaluation;

namespace GrabNReadApp.Data.Services.Evaluation.Contracts
{
    public interface IRatingService
    {
        Task<Vote> Create(Vote vote);

        int GetVoteValueByUserIdAndBookId(string userId, int bookId);

        Task<Vote> ChangeVote(Vote vote);

        Vote GetVoteByUserIdAndBookId(string userId, int bookId);

        int GetAverageBookRatingByBookId(int bookId);
    }
}
