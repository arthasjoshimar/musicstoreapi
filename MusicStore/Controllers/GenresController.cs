using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Dto.Response;
using MusicStore.Entities;

namespace MusicStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController: ControllerBase
{
    private readonly MusicStoreDbContext _context;
    private readonly ILogger<GenresController> _logger;

    public GenresController(MusicStoreDbContext context, ILogger<GenresController> logger)
	{
        _context = context;
        _logger = logger;
    }

    //get: api/genres
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponseGeneric<List<GenreDtoResponse>>), 200)]
    public async Task<IActionResult> GetGenres()
    {
        var response = new BaseResponseGeneric<IEnumerable<GenreDtoResponse>>();
        try
        {
            response.Data = await _context.Set<Genre>()
                .Select(p => new GenreDtoResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status
                })
                .ToListAsync();
            response.Success= true;
        }
        catch (Exception ex) 
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }
        return Ok(response);
    }

    //get:api/genres/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenre(int id)
    {
        var genre = await _context.Set<Genre>().FindAsync(id);

        if(genre == null)
        {
            return NotFound();
        }
        return Ok(genre);
    }
}

