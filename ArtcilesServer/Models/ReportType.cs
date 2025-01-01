using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class ReportType
{
    public int ReportTypeId { get; set; }

    public string ReportType1 { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
