namespace ArtcilesServer.DTO
{
    public class ArticleDTO
    {
        public string ArticleTitle { get; set; } = null!;

        public string ArticleContent { get; set; } = null!;

        public DateTime ArticleCreatedAt { get; set; }

        public int UserId { get; set; }

        public DateTime ArticleModifiedAt { get; set; }

        public int CategoryId { get; set; }
    }
}
