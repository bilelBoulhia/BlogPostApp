namespace ArtcilesServer.DTO
{
    public class CommentDTO
    {
        public string CommentContent { get; set; } = null!;

        public int ArticleId { get; set; }

        public int UserId { get; set; }
    }
}
