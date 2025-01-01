using ArtcilesServer.DTO;
using ArtcilesServer.Models;

namespace ArtcilesServer.Interfaces
{
    public interface IComment
    {
        Task<ICollection<CommentDTO>> GetCommentByArticle(int articleId);
        Task<ICollection<CommentDTO>> GetCommentByUser(int userId);
        
    }
}
