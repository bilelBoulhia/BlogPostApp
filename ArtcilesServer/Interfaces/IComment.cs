using ArtcilesServer.DTO;
using ArtcilesServer.Models;

namespace ArtcilesServer.Interfaces
{
    public interface IComment
    {
 
        Task<ICollection<Comment>> GetCommentByUser(int userId);


        Task RemoveLike(int articleId, int userId);
        Task addLike(CommentLikeDTO commentLike);



    }
}
