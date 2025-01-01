using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string CommentContent { get; set; } = null!;

    public int ArticleId { get; set; }

    public int UserId { get; set; }

    public DateTime? CommentCreatedAt { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
