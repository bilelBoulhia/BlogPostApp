using System;
using ArtcilesServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtcilesServer.Data
{
    public class SeedData
    {
        public static void Initialize(DbConn context)
        {


            if (!context.Users.Any())
            {
                var users = new List<User>
        {
            new User
            {
               
                UserName = "John",
                UserFamilyName = "Doe",
                UserPhoneNumber = 123456789,
                UserBirthDay = (new DateTime(1990, 5, 14)),
                UserEmail = "john.doe@example.com",
                UserHash = "hashedpassword123",
                UserSalt = "saltvalue123"
            },
            new User
            {
           
                UserName = "Jane",
                UserFamilyName = "Smith",
                UserPhoneNumber = 987654321,
                UserBirthDay = (new DateTime(1992, 3, 20)),
                UserEmail = "jane.smith@example.com",
                UserHash = "hashedpassword456",
                UserSalt = "saltvalue456"
            },
            new User
            {
          
                UserName = "Alice",
                UserFamilyName = "Johnson",
                UserPhoneNumber = 555555555,
                UserBirthDay = (new DateTime(1988, 8, 30)),
                UserEmail = "alice.johnson@example.com",
                UserHash = "hashedpassword789",
                UserSalt = "saltvalue789"
            }
        };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
            if (!context.ReportTypes.Any())
            {
                var reportTypes = new List<ReportType>
    {
        new ReportType
        {
            ReportType1 = "Spam"
        },
        new ReportType
        {
            ReportType1 = "Harassment"
        },
        new ReportType
        {
            ReportType1 = "Inappropriate Content"
        },
        new ReportType
        {
            ReportType1 = "False Information"
        }
    };

                context.ReportTypes.AddRange(reportTypes);
                context.SaveChanges();
            }
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
    {
        new Category
        {
           
            CategoryName = "Technology"
        },
        new Category
        {
          
            CategoryName = "Science"
        },
        new Category
        {
          
            CategoryName = "Health"
        },
        new Category
        {
         
            CategoryName = "Lifestyle"
        },
        new Category
        {
            CategoryName = "Education"
        },
        new Category
        {
           
            CategoryName = "Travel"
        },
        new Category
        {
       
            CategoryName = "Business"
        },
        new Category
        {
          
            CategoryName = "Entertainment"
        }
    };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
            if (!context.Hobbies.Any())
            {
                var hobbies = new List<Hobby>
    {
        new Hobby
        {
            HobbyName = "Reading"
        },
        new Hobby
        {
            HobbyName = "Traveling"
        },
        new Hobby
        {
            HobbyName = "Cooking"
        },
        new Hobby
        {
            HobbyName = "Photography"
        },
        new Hobby
        {
            HobbyName = "Gardening"
        },
        new Hobby
        {
            HobbyName = "Painting"
        },
        new Hobby
        {
            HobbyName = "Writing"
        },
        new Hobby
        {
            HobbyName = "Sports"
        }
    };

                context.Hobbies.AddRange(hobbies);
                context.SaveChanges();
            }
            if (!context.Articles.Any())
            {
                var articles = new List<Article>
        {
            new Article
            {
               
                ArticleTitle = "Getting Started with EF Core",
                ArticleCreatedAt = DateTime.Now.AddDays(-10),
                ArticleModifiedAt = DateTime.Now.AddDays(-5),
                ArticleContent = "This article introduces Entity Framework Core and its features.",
                UserId = 1,
                CategoryId = 1
            },
            new Article
            {
                
                ArticleTitle = "Mastering LINQ Queries",
                ArticleCreatedAt = DateTime.Now.AddDays(-8),
                ArticleModifiedAt = DateTime.Now.AddDays(-4),
                ArticleContent = "A comprehensive guide to writing efficient LINQ queries in C#.",
                UserId = 2,
                CategoryId = 2
            },
            new Article
            {
               
                ArticleTitle = "Building Relationships in EF Core",
                ArticleCreatedAt = DateTime.Now.AddDays(-6),
                ArticleModifiedAt = DateTime.Now.AddDays(-2),
                ArticleContent = "Learn how to manage relationships between entities effectively.",
                UserId = 3,
                CategoryId = 3
            },
            new Article
            {
               
                ArticleTitle = "Optimizing EF Core Performance",
                ArticleCreatedAt = DateTime.Now.AddDays(-12),
                ArticleModifiedAt = DateTime.Now.AddDays(-7),
                ArticleContent = "Best practices for optimizing performance when using EF Core.",
                UserId = 1,
                CategoryId = 4
            }
        };

                context.Articles.AddRange(articles);
                context.SaveChanges();
            }
            if (!context.Comments.Any())
            {
                var comments = new List<Comment>
    {
        new Comment
        {
            CommentContent = "This is a great article!",
            ArticleId = 1, 
            UserId = 1,   
            CommentCreatedAt = DateTime.Now
        },
        new Comment
        {
            CommentContent = "I learned so much from this post.",
            ArticleId = 1,
            UserId = 2,
            CommentCreatedAt = DateTime.Now
        },
        new Comment
        {
            CommentContent = "Can you elaborate on the second point?",
            ArticleId = 2,
            UserId = 3,
            CommentCreatedAt = DateTime.Now
        },
        new Comment
        {
            CommentContent = "This is very informative, thank you!",
            ArticleId = 2,
            UserId = 1,
            CommentCreatedAt = DateTime.Now
        }
    };

                context.Comments.AddRange(comments);
                context.SaveChanges();
            }
            if (!context.Reports.Any())
            {
                var reports = new List<Report>
    {
        new Report
        {
            ReportTitle = "Spam Content",
            ReportCreatedAt = DateTime.Now,
            ReportTypeId = 1, // Reference a valid ReportTypeId
            ReportExplaining = "The article contains repetitive spam content.",
            ArticleId = 1,    // Reference a valid ArticleId
            UserId = 1        // Reference a valid UserId
        },
        new Report
        {
            ReportTitle = "Harassment in Comments",
            ReportCreatedAt = DateTime.Now,
            ReportTypeId = 2,
            ReportExplaining = "The comment includes offensive language.",
            CommentId = 1,   
            UserId = 2
        },
        new Report
        {
            ReportTitle = "Inappropriate Content",
            ReportCreatedAt = DateTime.Now,
            ReportTypeId = 3,
            ReportExplaining = "This content is inappropriate for the platform.",
            ArticleId = 2,
            UserId = 3
        },
        new Report
        {
            ReportTitle = "False Information",
            ReportCreatedAt = DateTime.Now,
            ReportTypeId = 4,
            ReportExplaining = "The article contains false or misleading information.",
            ArticleId = 3,
            UserId = 1
        }
    };

                context.Reports.AddRange(reports);
                context.SaveChanges();
            }


        }






    }

}