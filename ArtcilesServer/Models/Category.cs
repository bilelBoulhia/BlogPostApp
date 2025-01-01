using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
