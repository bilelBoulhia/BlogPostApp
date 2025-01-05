using ArtcilesServer.DTO;
using ArtcilesServer.Models;

namespace ArtcilesServer.Interfaces
{
    public interface IArticle
    {
        Task<ICollection<Article>> GetArticleByUser(int userId);
        Task<ICollection<Article>> GetArticleByCategory(int CategoryId);

        Task RemoveLike(int articleId, int userId);
        Task<ICollection<Article>> getAllArticlesLikedByUser(int userId);

        Task<ICollection<User>> getAllLikesOfanArticle(int articleId);
       
        Task<ICollection<Article>> SearchAsync(string searchQuery);
        Task addLike(ArticleLikeDTO articleLike);


    }
}
