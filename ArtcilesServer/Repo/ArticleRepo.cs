using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtcilesServer.Repo
{
    public class ArticleRepo : IArticle
    {

        private readonly DbConn _context;

        public ArticleRepo(DbConn context)
        {
            _context = context;
        }
       

        public async Task<ICollection<Article>> SearchAsync(string searchQuery)
        {
  
            return await _context.Articles
                .FromSqlInterpolated($"Execute searchArticles @SearchTerm = {searchQuery}")
                .ToListAsync();
        }



        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Article>> GetArticleByUser(int userId)
        {
           return await _context.Articles.Where(article=> article.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<Article>> GetArticleByCategory(int CategoryId)
        {
            return await _context.Articles.Where(article => article.CategoryId == CategoryId).ToListAsync();
        }

       
    }
}
