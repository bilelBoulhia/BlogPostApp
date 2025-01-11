using System;
using System.Collections.Generic;
using ArtcilesServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtcilesServer.Data;

public partial class DbConn : DbContext
{
    public DbConn()
    {
    }

    public DbConn(DbContextOptions<DbConn> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleImage> ArticleImages { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Hobby> Hobbies { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportType> ReportTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__articles__75D3D37EEF707041");

            entity.ToTable("articles");

            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.ArticleContent)
                .IsUnicode(false)
                .HasColumnName("articleContent");
            entity.Property(e => e.ArticleCreatedAt).HasColumnName("articleCreatedAt");
            entity.Property(e => e.ArticleModifiedAt).HasColumnName("articleModifiedAt");
            entity.Property(e => e.ArticleTitle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("articleTitle");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Category).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__articles__catego__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Articles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__articles__userId__4316F928");
        });

        modelBuilder.Entity<ArticleImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__articleI__7516F70CFB1204E1");

            entity.ToTable("articleImages");

            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.ImageLink).IsUnicode(false);

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleImages)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__articleIm__artic__797309D9");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BD43D95A0");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comments__CDDE919D8DB4E515");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId).HasColumnName("commentId");
            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.CommentContent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("commentContent");
            entity.Property(e => e.CommentCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("commentCreatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Article).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comments__articl__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comments__userId__47DBAE45");
        });

        modelBuilder.Entity<Hobby>(entity =>
        {
            entity.HasKey(e => e.HobbyId).HasName("PK__Hobbies__0ABE0BCFBE44D557");

            entity.ToTable("hobbies");

            entity.Property(e => e.HobbyId).HasColumnName("hobbyId");
            entity.Property(e => e.HobbyName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("hobbyName");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__reports__1C9B4E2D3530A87F");

            entity.ToTable("reports");

            entity.Property(e => e.ReportId).HasColumnName("reportId");
            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.CommentId).HasColumnName("commentId");
            entity.Property(e => e.ReportCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reportCreatedAt");
            entity.Property(e => e.ReportExplaining)
                .IsUnicode(false)
                .HasColumnName("reportExplaining");
            entity.Property(e => e.ReportTitle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("reportTitle");
            entity.Property(e => e.ReportTypeId).HasColumnName("reportTypeId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Article).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__reports__article__4CA06362");

            entity.HasOne(d => d.Comment).WithMany(p => p.Reports)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK__reports__comment__4D94879B");

            entity.HasOne(d => d.ReportType).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ReportTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reports__reportT__4BAC3F29");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reports__userId__4E88ABD4");
        });

        modelBuilder.Entity<ReportType>(entity =>
        {
            entity.HasKey(e => e.ReportTypeId).HasName("PK__reportTy__30EA6CAD631C7EBF");

            entity.ToTable("reportTypes");

            entity.Property(e => e.ReportTypeId).HasColumnName("reportTypeId");
            entity.Property(e => e.ReportType1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("reportType");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__CB9A1CFF7C51B958");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserBirthDay).HasColumnName("userBirthDay");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("userEmail");
            entity.Property(e => e.UserFamilyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userFamilyName");
            entity.Property(e => e.UserHash)
                .IsUnicode(false)
                .HasColumnName("userHash");
            entity.Property(e => e.UserImage)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("userImage");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UserPhoneNumber).HasColumnName("userPhoneNumber");
            entity.Property(e => e.UserSalt)
                .IsUnicode(false)
                .HasColumnName("userSalt");

            entity.HasMany(d => d.ArticlesNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "LikesForArticle",
                    r => r.HasOne<Article>().WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__likesForA__artic__5AEE82B9"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__likesForA__userI__59FA5E80"),
                    j =>
                    {
                        j.HasKey("UserId", "ArticleId").HasName("PK__likesFor__3CC721C85547ECED");
                        j.ToTable("likesForArticles");
                        j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        j.IndexerProperty<int>("ArticleId").HasColumnName("articleId");
                    });

            entity.HasMany(d => d.CommentsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "LikesForComment",
                    r => r.HasOne<Comment>().WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__likesForC__comme__534D60F1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__likesForC__userI__52593CB8"),
                    j =>
                    {
                        j.HasKey("UserId", "CommentId").HasName("PK__likesFor__0747F5E67207DF19");
                        j.ToTable("likesForComments");
                        j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        j.IndexerProperty<int>("CommentId").HasColumnName("commentId");
                    });

            entity.HasMany(d => d.Follwers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFollower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollwerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_follows"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user"),
                    j =>
                    {
                        j.HasKey("UserId", "FollwerId").HasName("PK__user_fol__D44A47C44D9A1A2D");
                        j.ToTable("userFollowers");
                        j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        j.IndexerProperty<int>("FollwerId").HasColumnName("follwerId");
                    });

            entity.HasMany(d => d.Hobbies).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserHobby",
                    r => r.HasOne<Hobby>().WithMany()
                        .HasForeignKey("HobbyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__userHobbi__hobby__693CA210"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__userHobbi__userI__68487DD7"),
                    j =>
                    {
                        j.HasKey("UserId", "HobbyId").HasName("PK__userHobb__AB2481EA40614279");
                        j.ToTable("userHobbies");
                        j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        j.IndexerProperty<int>("HobbyId").HasColumnName("hobbyId");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Follwers)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFollower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollwerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_follows"),
                    j =>
                    {
                        j.HasKey("UserId", "FollwerId").HasName("PK__user_fol__D44A47C44D9A1A2D");
                        j.ToTable("userFollowers");
                        j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        j.IndexerProperty<int>("FollwerId").HasColumnName("follwerId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
