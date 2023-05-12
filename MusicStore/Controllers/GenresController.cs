using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
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
                .AsNoTracking()
                .Select(p => new GenreDtoResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = p.Status
                })
                .Where(p=> p.Status)
                .ToListAsync();
            response.Success= true;
        }
        catch (Exception ex) 
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al obtener los registros";
            _logger.LogError(ex, ex.Message);
        }
        return Ok(response);
    }

    //get:api/genres/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BaseResponseGeneric<GenreDtoResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponseGeneric<GenreDtoResponse>), 404)]
    public async Task<IActionResult> GetGenre(int id)
    {
        var response = new BaseResponseGeneric<GenreDtoResponse>();

        try
        {
            var entity = await _context.Set<Genre>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if(entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el registro";
                return NotFound(response);
            }

            response.Data = new GenreDtoResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status
            };

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }

        return Ok(response);
    }

    //PUT: api/Genres/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse),200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> PutGenre(int id, GenreDtoRequest genreDtoRequest)
    {
        var response = new BaseResponse();

        try 
        {
            var entity = await _context.Set<Genre>()
                .FindAsync(id);
            if(entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el registro";

                return NotFound(response);
            }

            entity.Name= genreDtoRequest.Name;
            entity.Status = genreDtoRequest.Status;

            //_context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            response.Success = true;
        }
        catch (Exception ex) 
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al actualizar el registro";
            _logger.LogError(ex, ex.Message);
        }

        return Ok(response);
    }

    //POST: api/Genres
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse),200)]
    public async Task<IActionResult> PostGenre(GenreDtoRequest genreDtoRequest)
    {
        var response = new BaseResponse();

        try
        {
            var entity = new Genre
            {
                Name= genreDtoRequest.Name,
                Status = genreDtoRequest.Status,
            };

            _context.Set<Genre>().Add(entity);
            await _context.SaveChangesAsync();
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al realizar el registro";
            _logger.LogError(ex, ex.Message);
        }

        return Ok(response);
    }

    //DELETE: api/Genre/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var response = new BaseResponse();

        try 
        {
            var entity = await _context.Set<Genre>()
                .FindAsync(id);
            
            if(entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el registro";

                return NotFound(response);
            }
            // _context.Set<Genre>().Remove(entity); //con esto eliminamos de BD
            // pero modificaremos el status para hacer eliminacion logica:
            entity.Status = false;
            // ya no es necesario para entity 7
            //_context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            response.Success = true;

        }
        catch(Exception ex) 
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al eliminar el registro";
            _logger.LogError(ex, ex.Message);
        }

        return Ok(response);
    }
}

