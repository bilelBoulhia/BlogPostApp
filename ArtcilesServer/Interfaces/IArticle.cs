using ArtcilesServer.DTO;
using ArtcilesServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtcilesServer.Interfaces
{
    public interface IArticle
    {
        Task<ICollection<Article>> GetArticleByUser(int userId);
        Task<ICollection<Article>> GetArticleByCategory(int CategoryId);

        Task RemoveLike(ArticleLikeDTO articleLike);
        Task<ICollection<Article>> getAllArticlesLikedByUser(int userId);

        Task<ICollection<FullArticleWithDetails>> GetFullArticleDetails(int articleId);
        Task<ICollection<BasicArticleWithDetails>> GetAllArticles();
        Task<ICollection<BasicArticleWithDetails>> getAllSavedArticlesByUser(int userId);
        Task<ICollection<BasicArticleWithDetails>> getAllArticlesByUser(int userId);
        Task<ICollection<Category>> GetAllCategories();

        Task saveArticle(SaveArticleDTO saveArticleDTO);

        Task RemoveSave(SaveArticleDTO saveArticleDTO);

        Task<ICollection<BasicArticleWithDetails>> SearchAsync(string searchQuery);
        Task addLike(ArticleLikeDTO articleLike);


    }
}
