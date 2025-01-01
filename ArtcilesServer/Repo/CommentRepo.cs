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

        public async Task<ICollection<Comment>> GetCommentByArticle(int articleId)
        {
            return await _context.Comments.Where(c=>c.ArticleId == articleId).ToListAsync();
        }

        public async Task<ICollection<Comment>> GetCommentByUser(int userId)
        {
            return await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
