using ArtcilesServer.DTO;
using ArtcilesServer.Models;

namespace ArtcilesServer.Interfaces
{
    public interface IArticle
    {
        Task<ICollection<Article>> GetArticleByUser(int userId);
        Task<ICollection<Article>> GetArticleByCategory(int CategoryId);
        Task<ICollection<Article>> SearchAsync(string searchQuery);


    }
}
