namespace ArtcilesServer.DTO
{
    public class ReportDTO
    {
        public string ReportTitle { get; set; } = null!;
        public int ReportTypeId { get; set; }

        public string? ReportExplaining { get; set; }

        public int? ArticleId { get; set; }

        public int? CommentId { get; set; }

        public int UserId { get; set; }


    }
}
