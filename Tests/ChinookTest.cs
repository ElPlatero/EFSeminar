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


    private static partial ChinookContext getContext();
}