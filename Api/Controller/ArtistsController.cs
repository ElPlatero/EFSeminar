using EntityFrameworkCoreSeminar.Database.Models.Chinook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController(ChinookContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetArtists([FromQuery] int? index, [FromQuery] int? limit)
    {
        IQueryable<Artist> query = context.Artists.AsNoTracking().OrderBy(p => p.Name);
        if (index > 0)
        {
            query = query.Skip(index.Value);
        }

        if (limit > 0)
        {
            query = query.Take(limit.Value);
        }

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{artistId:min(1)}")]
    public async Task<IActionResult> GetSingleArtist(int artistId)
    {
        var result = await context.Artists.AsNoTracking().FirstOrDefaultAsync(p => p.ArtistId == artistId);
        return result != null
            ? Ok(result)
            : NotFound();
    }
}