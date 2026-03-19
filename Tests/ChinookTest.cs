using System.Text.Json;
using EntityFrameworkCoreSeminar.Database;
using EntityFrameworkCoreSeminar.Database.Models.Chinook;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests;

public class ChinookTest(ITestOutputHelper helper) : ChinookTestBase
{
    [Fact]
    public async Task TestGetArtists()
    {
        await using var context = getContext(helper);
        var result = await context.Artists.Select(p => new { p.Name, p.Albums }).OrderBy(p => p.Name).Skip(2).Take(10).ToListAsync();

        Assert.NotNull(result);
        foreach (var artist in result)
        {
            helper.WriteLine($"{artist.Name} hat {artist.Albums.Count} Alben.");
        }

    }

    [Theory]
    [InlineData("Aerosmith")]
    public async Task TestGetAlbums(string artistName)
    {
        await using var context = getContext(helper);

        var result = await context.Albums.Where(p => p.Artist.Name == artistName).Select(p => p.Title).ToListAsync();
        result.ForEach(p => helper.WriteLine($"Album: {p}"));
        
    }

    [Theory]
    [InlineData("Eric Clapton")]
    public async Task TestGetAlbumsWithTracks(string artistName)
    {
        await using var context = getContext(helper);

        var result = await context.Albums.Where(p => p.Artist.Name == artistName).Select(p => new { p.Title, p.Tracks }).ToListAsync();
        result.ForEach(p => helper.WriteLine($"{p.Title} hat die Tracks {string.Join(", ", p.Tracks.Select(q => q.Name))}."));
    }

    [Theory]
    [InlineData("Blues")]
    public async Task GetArtistsOfGenre(string genreName)
    {
        await using var context = getContext(helper);
        
        var artists = await context.Genres.Where(p => p.Name == genreName).SelectMany(p => p.Tracks.Where(q => q.Album != null).Select(q => q.Album!.Artist )).Select(p => p.Name).ToHashSetAsync();
        helper.WriteLine($"Künstler: {string.Join(", ", artists)}");
    }

    [Fact]
    public async Task TestUpdateAcDc()
    {
        const string bandName = "AC/DC";
        const string newName = "DC (Battery Powered)";
        
        await using var context = getContext(helper);

        var acdc = await context.Artists.FirstOrDefaultAsync(p => p.Name == bandName);
        Assert.NotNull(acdc);
                   
        acdc.Name = newName;
        await context.SaveChangesAsync();
        
        await using var context2 = getContext(helper);
        var dc = await context2.Artists.FirstOrDefaultAsync(p => p.Name == newName);
        Assert.NotNull(dc);

        dc.Name = bandName;
        await context2.SaveChangesAsync();
    }


    [Theory]
    [MemberData(nameof(AddArtistTestCases))]
    public async Task TestAddArtist(string artistName, IEnumerable<Album> albums)
    {
        await using var context = getContext(helper);

        var artist = new Artist { Albums = albums.ToList(), Name = artistName };
        await context.AddAsync(artist);
        await context.SaveChangesAsync();

        helper.WriteLine("Artist angelegt mit {0}.", JsonSerializer.Serialize(new { artist.ArtistId, Albums = artist.Albums.Select(p => new { p.AlbumId, TrackIds = p.Tracks.Select(q => q.TrackId) }) }));
        
        context.Remove(artist);
        await context.SaveChangesAsync();
    }

    public static TheoryData<string, IEnumerable<Album>> AddArtistTestCases { get; } = new()
    {
        { 
            "Ton Steine Scherben", 
            new List<Album> 
            { 
                new()
                { 
                    Title = "Keine Macht für Niemand", 
                    Tracks = new List<Track> 
                    {
                        new() { Name = "Wir müssen hier raus", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Feierabend", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Die letzte Schlacht gewinnen wir", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Paul Panzers Blues", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Menschenjäger", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Allein machen sie dich ein", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Schritt für Schritt ins Paradies", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Der Traum ist aus", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Mensch Meier", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Rauch-Haus-Song", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Keine Macht für Niemand", GenreId = 1, MediaTypeId = 5 },
                        new() { Name = "Komm schlaf bei mir", GenreId = 1, MediaTypeId = 5 }
                    } 
                } 
            } 
        }
    };
}