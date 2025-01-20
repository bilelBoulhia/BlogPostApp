namespace ArtcilesServer.Models
{
    public class BasicArticleWithDetails
    {
    
            public int ArticleId { get; set; }
            public string ArticleTitle { get; set; }
            public string ArticleContent { get; set; }
            public DateTime ArticleCreatedAt { get; set; }
            public int NumberOfComments { get; set; }
            public int NumberOfLikes { get; set; }
            public string CategoryName { get; set; }
            public string UserName { get; set; }
            public int UserId { get; set; }
          
        

    }
}
