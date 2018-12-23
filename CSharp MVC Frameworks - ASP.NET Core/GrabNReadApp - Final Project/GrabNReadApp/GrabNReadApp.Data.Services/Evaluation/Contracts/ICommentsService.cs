using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Evaluation;

namespace GrabNReadApp.Data.Services.Evaluation.Contracts
{
    public interface ICommentsService
    {
        Task<Comment> Create(Comment comment);

        IEnumerable<Comment> GetAllCommentsByBookId(int id);

        Comment GetCommentById(int id);

        bool Delete(int id);
    }
}
