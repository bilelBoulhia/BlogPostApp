using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserFamilyName { get; set; } = null!;

    public int UserPhoneNumber { get; set; }

    public DateTime UserBirthDay { get; set; }

    public string UserHash { get; set; } = null!;

    public string UserSalt { get; set; } = null!;

    public string UserImage { get; set; }

    public string? UserEmail { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<Article> Articles1 { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticlesNavigation { get; set; } = new List<Article>();

    public virtual ICollection<Comment> CommentsNavigation { get; set; } = new List<Comment>();

    public virtual ICollection<User> Follwers { get; set; } = new List<User>();

    public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
