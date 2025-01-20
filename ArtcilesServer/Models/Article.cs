using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public string ArticleTitle { get; set; } = null!;

    public DateTime ArticleCreatedAt { get; set; }

    public DateTime ArticleModifiedAt { get; set; }

    public int CategoryId { get; set; }

    public string ArticleContent { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<User> UsersNavigation { get; set; } = new List<User>();
}
