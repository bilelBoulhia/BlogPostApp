using System;
using System.Collections.Generic;

namespace ArtcilesServer.Models;

public partial class Hobby
{
    public int HobbyId { get; set; }

    public string? HobbyName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
