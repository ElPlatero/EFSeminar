using EntityFrameworkCoreSeminar.Database;
using EntityFrameworkCoreSeminar.Database.Models.Chinook;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests;

public partial class ChinookTest(ITestOutputHelper helper)
{
    [Fact]
    public async Task TestGetArtists()
    {
        await using var context = getContext();
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
        await using var context = getContext();

        var result = await context.Albums.Where(p => p.Artist.Name == artistName).Select(p => p.Title).ToListAsync();
        result.ForEach(p => helper.WriteLine($"Album: {p}"));
        
    }

    [Theory]
    [InlineData("Eric Clapton")]
    public async Task TestGetAlbumsWithTracks(string artistName)
    {
        await using var context = getContext();

        var result = await context.Albums.Where(p => p.Artist.Name == artistName).Select(p => new { p.Title, p.Tracks }).ToListAsync();
        result.ForEach(p => helper.WriteLine($"{p.Title} hat die Tracks {string.Join(", ", p.Tracks.Select(q => q.Name))}."));
    }

    [Theory]
    [InlineData("Blues")]
    public async Task GetArtistsOfGenre(string genreName)
    {
        await using var context = getContext();
        
        var artists = await context.Genres.Where(p => p.Name == genreName).SelectMany(p => p.Tracks.Where(q => q.Album != null).Select(q => q.Album!.Artist )).Select(p => p.Name).ToHashSetAsync();
        helper.WriteLine($"Künstler: {string.Join(", ", artists)}");
    }

    

    private partial ChinookContext getContext();
}