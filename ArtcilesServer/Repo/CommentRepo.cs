using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtcilesServer.Repo
{
    public class CommentRepo : IComment
    {

        private readonly DbConn _context;

        public CommentRepo(DbConn context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> getAllLikesOfaComment(int CommentId)
        {
            var comment = await _context.Comments
              .Include(a => a.Users)
              .FirstOrDefaultAsync(a => a.CommentId == CommentId);

            if (comment == null)
            {
                throw new ArgumentException("comment Not found");
            }



            return comment.Users;
        }

        public async Task RemoveLike(int commentId, int userId)
        {
            var comment = await _context.Comments
              .Include(a => a.Users)
              .FirstOrDefaultAsync(a => a.CommentId== commentId);
            if (comment == null)
            {
                throw new ArgumentException("comment Not found");
            }

            var user = await _context.Users
                .Include(u => u.CommentsNavigation)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentException("user not found");
            }

            user.CommentsNavigation.Remove(comment);

            await SaveAsync();
        }

        public async Task<ICollection<Comment>> GetCommentByArticle(int articleId)
        {
            return await _context.Comments.Where(c=>c.ArticleId == articleId).ToListAsync();
        }

        public async Task<ICollection<Comment>> GetCommentByUser(int userId)
        {
            return await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        }



     

        public async Task addLike(CommentLikeDTO commentLike)
        {
            var user = await _context.Users
             .Include(u => u.CommentsNavigation)
             .FirstOrDefaultAsync(u => u.UserId == commentLike.UserId);

            var comment = await _context.Comments.FindAsync(commentLike.CommentId);

            if (user == null || comment == null)
            {
                throw new ArgumentException("User or comment not found.");
            }

            if (user.CommentsNavigation.Any(a => a.ArticleId == commentLike.CommentId))
            {
                throw new InvalidOperationException("User already liked this comment.");
            }

            user.CommentsNavigation.Add(comment);

            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task addLike(CommentDTO comment)
        {
            throw new NotImplementedException();
        }
    }
}
