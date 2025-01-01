namespace ArtcilesServer.DTO
{
    public class ArticleDTO
    {
        public string ArticleTitle { get; set; } = null!;

        public string ArticleContent { get; set; } = null!;

        public DateOnly ArticleCreatedAt { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }
    }
}
