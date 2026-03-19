using EntityFrameworkCoreSeminar.Database.Models.Chinook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controller;

[ApiController]
[Route("api/albums/{albumId:min(1)}/[controller]")]
public class TracksController(ChinookContext context) : ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> GetTracks([FromQuery] int? index, [FromQuery] int? limit, int albumId)
   {
       IQueryable<Track> query = context.Tracks.AsNoTracking().Where(p => p.AlbumId == albumId).OrderBy(p => p.TrackId);
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

   [HttpGet("{trackId:min(1)}")]
   public async Task<IActionResult> GetSingleTrack(int trackId)
   {
       var result = await context.Tracks.AsNoTracking().FirstOrDefaultAsync(p => p.TrackId == trackId);
       return result != null
           ? Ok(result)
           : NotFound();
   }
}