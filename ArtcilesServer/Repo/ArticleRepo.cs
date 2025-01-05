using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task addLike(ArticleLikeDTO articleLike)
        {
            var user = await _context.Users
            .Include(u => u.ArticlesNavigation)
            .FirstOrDefaultAsync(u => u.UserId == articleLike.UserId);

            var article = await _context.Articles.FindAsync(articleLike.ArticleId);

            if (user == null || article == null)
            {
                throw new ArgumentException("User or article not found.");
            }

            if (user.ArticlesNavigation.Any(a => a.ArticleId == articleLike.ArticleId))
            {
                throw new InvalidOperationException("User already liked this article.");
            }

            user.ArticlesNavigation.Add(article);

            await SaveAsync();
        }


        public async Task<ICollection<Article>> getAllArticlesLikedByUser(int userId)
        {
            var user = await _context.Users
            .Include(u => u.ArticlesNavigation)
            .FirstOrDefaultAsync(u => u.UserId == userId);

            if(user == null)
            {
                throw new ArgumentException("user not found");
            }

            if (user.ArticlesNavigation == null) {

                throw new ArgumentException("article not found");
            }
            

            return user.ArticlesNavigation;
            

        }

        public async Task<ICollection<User>> getAllLikesOfanArticle(int articleId)
        {
            var article = await _context.Articles
              .Include(a => a.Users)
              .FirstOrDefaultAsync(a => a.ArticleId == articleId);

            if (article == null)
            {
                throw new ArgumentException("article Not found");
            }



            return article.Users;
        }


        public async Task RemoveLike(int articleId,int userId)
        {
            var article = await _context.Articles
              .Include(a => a.Users)
              .FirstOrDefaultAsync(a => a.ArticleId == articleId);
            if (article == null)
            {
                throw new ArgumentException("article Not found");
            }

            var user = await _context.Users
                .Include(u => u.ArticlesNavigation)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentException("user not found");
            }

            user.ArticlesNavigation.Remove(article);

            await SaveAsync();

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
