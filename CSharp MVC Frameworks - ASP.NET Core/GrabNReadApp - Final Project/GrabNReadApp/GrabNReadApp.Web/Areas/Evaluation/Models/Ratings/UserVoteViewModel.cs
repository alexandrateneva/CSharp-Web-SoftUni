namespace GrabNReadApp.Web.Areas.Evaluation.Models.Ratings
{
    public class UserVoteViewModel
    {
        public int BookId { get; set; }

        public int VoteValue { get; set; }

        public decimal AverageRating { get; set; }
    }
}
