using ArtcilesServer.Data;
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

        public async Task<ICollection<FullArticleWithDetails>> GetFullArticleDetails(int articleId)
        {

            using var connection = _context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "EXECUTE GetArticleDetailsAndComments @ArticleId";


            var parameter = command.CreateParameter();
            parameter.ParameterName = "@ArticleId";
            parameter.Value = articleId;
            command.Parameters.Add(parameter);


            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();


            using var reader = await command.ExecuteReaderAsync();

            var articles = new List<FullArticleWithDetails>();


            while (await reader.ReadAsync())
            {
                var article = new FullArticleWithDetails
                {
                    ArticleId = reader.GetInt32(reader.GetOrdinal("ArticleId")),
                    ArticleTitle = reader.GetString(reader.GetOrdinal("ArticleTitle")),
                    ArticleContent = reader.GetString(reader.GetOrdinal("ArticleContent")),
                    ArticleCreatedAt = reader.GetDateTime(reader.GetOrdinal("ArticleCreatedAt")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                    NumberOfComments = reader.GetInt32(reader.GetOrdinal("NumberOfComments")),
                    NumberOfLikes = reader.GetInt32(reader.GetOrdinal("NumberOfLikes")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    Comments = new List<CommentsWithDetails>()
                };
                articles.Add(article);
            }

    
            if (articles.Any() && await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    var comment = new CommentsWithDetails
                    {
                        CommentId = reader.GetInt32(reader.GetOrdinal("CommentId")),
                        CommentContent = reader.GetString(reader.GetOrdinal("CommentContent")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        NumberOfLikes = reader.GetInt32(reader.GetOrdinal("NumberOfLikes"))
                    };

                    articles[0].Comments.Add(comment);
                }
            }

            return articles;
        }
        public async Task<ICollection<BasicArticleWithDetails>> SearchAsync(string searchQuery)
        {
            return await _context.Set<BasicArticleWithDetails>()
                .FromSqlInterpolated($"EXECUTE searchArticles @SearchTerm = {searchQuery}")
                .ToListAsync();
        }

        public async Task<ICollection<BasicArticleWithDetails>> GetAllArticles()
        {
            return await _context.Set<BasicArticleWithDetails>()
                .FromSqlInterpolated($"SELECT * FROM v_AllArticlesWithDetails")
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

        public async Task saveArticle(SaveArticleDTO saveArticleDTO)
        {
            var user = await _context.Users
            .Include(u => u.Articles1)
            .FirstOrDefaultAsync(u => u.UserId == saveArticleDTO.UserId);

            var article = await _context.Articles.FindAsync(saveArticleDTO.ArticleId);

            if (user == null || article == null)
            {
                throw new ArgumentException("User or article not found.");
            }

            if (user.ArticlesNavigation.Any(a => a.ArticleId == saveArticleDTO.ArticleId))
            {
                throw new InvalidOperationException("User already liked this article.");
            }

            user.Articles1.Add(article);

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

        public async Task<ICollection<BasicArticleWithDetails>> getAllArticlesByUser(int userId)
        {


            return await _context.Set<BasicArticleWithDetails>()
               .FromSqlInterpolated($"EXECUTE GetArticlesByUserId @UserId = {userId}")
               .ToListAsync();

        }


        public async Task<ICollection<BasicArticleWithDetails>> getAllSavedArticlesByUser(int userId)
        {
           

            return await _context.Set<BasicArticleWithDetails>()
               .FromSqlInterpolated($"EXECUTE GetSavedArticlesByUserId @UserId = {userId}")
               .ToListAsync();

        }
    

       


        public async Task RemoveLike(ArticleLikeDTO articleLike)
        {
            var article = await _context.Articles
              .Include(a => a.Users)
              .FirstOrDefaultAsync(a => a.ArticleId == articleLike.ArticleId);
            if (article == null)
            {
                throw new ArgumentException("article Not found");
            }

            var user = await _context.Users
                .Include(u => u.ArticlesNavigation)
                .FirstOrDefaultAsync(u => u.UserId == articleLike.UserId);

            if (user == null)
            {
                throw new ArgumentException("user not found");
            }

            user.ArticlesNavigation.Remove(article);

            await SaveAsync();

        }


        public async Task RemoveSave(SaveArticleDTO saveArticleDTO)
        {
            var article = await _context.Articles
              .Include(a => a.Users)
              .FirstOrDefaultAsync(a => a.ArticleId == saveArticleDTO.ArticleId);
            if (article == null)
            {
                throw new ArgumentException("article Not found");
            }

            var user = await _context.Users
                .Include(u => u.Articles1)
                .FirstOrDefaultAsync(u => u.UserId == saveArticleDTO.UserId);

            if (user == null)
            {
                throw new ArgumentException("user not found");
            }

            user.Articles1.Remove(article);

            await SaveAsync();

        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<ICollection<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
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
