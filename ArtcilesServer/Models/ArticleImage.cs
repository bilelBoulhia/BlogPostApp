using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class ArticleImage
{
    public int ImageId { get; set; }

    public string ImageLink { get; set; } = null!;

    public int ArticleId { get; set; }

    public virtual Article Article { get; set; } = null!;
}
