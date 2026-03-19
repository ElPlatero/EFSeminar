using System;
using System.Collections.Generic;

namespace EntityFrameworkCoreSeminar.Database.Models.Chinook;

public partial class Album
{
    public int AlbumId { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public TimeSpan? TotalPlaytime { get; set; }
    
    public decimal? TotalPrice { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
