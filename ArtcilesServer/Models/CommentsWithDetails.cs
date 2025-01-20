namespace ArtcilesServer.Models
{
    public class CommentsWithDetails
    {
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int NumberOfLikes { get; set; }
    }
}
