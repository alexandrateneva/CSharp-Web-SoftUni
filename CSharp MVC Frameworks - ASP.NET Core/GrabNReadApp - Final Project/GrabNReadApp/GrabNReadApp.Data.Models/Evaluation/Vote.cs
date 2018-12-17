using GrabNReadApp.Data.Models.Books;

namespace GrabNReadApp.Data.Models.Evaluation
{
    public class Vote
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        public GrabNReadAppUser Creator { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int VoteValue { get; set; }
    }
}
