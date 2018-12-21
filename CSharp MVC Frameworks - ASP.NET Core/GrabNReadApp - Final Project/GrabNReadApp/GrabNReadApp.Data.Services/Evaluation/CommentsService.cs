using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GrabNReadApp.Data.Services.Evaluation
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> commentRepository;

        public CommentsService(IRepository<Comment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<Comment> Create(Comment comment)
        {
            await this.commentRepository.AddAsync(comment);
            await this.commentRepository.SaveChangesAsync();

            return comment;
        }

        public IEnumerable<Comment> GetAllCommentsByBookId(int id)
        {
            var comments = this.commentRepository.All()
                .Where(c => c.BookId == id)
                .OrderByDescending(z => z.Id)
                .Include(x => x.Creator)
                .ToList();

            return comments;
        }
    }
}
