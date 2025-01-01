using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportTitle { get; set; } = null!;

    public DateTime? ReportCreatedAt { get; set; }

    public int ReportTypeId { get; set; }

    public string? ReportExplaining { get; set; }

    public int? ArticleId { get; set; }

    public int? CommentId { get; set; }

    public int UserId { get; set; }

    public virtual Article? Article { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual ReportType ReportType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
