using System;
using System.Collections.Generic;

namespace EntityFrameworkCoreSeminar.Database.Models.Chinook;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? Name { get; set; }

    public string? Management { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
