
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;

namespace MusicStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConcertController : ControllerBase
{
    private readonly MusicStoreDbContext _context;
    private readonly ILogger<ConcertController> _logger;

    public ConcertController(MusicStoreDbContext context, ILogger<ConcertController> logger)
    {
        _context = context;
        _logger = logger;
    }

    //get: api/concerts
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponseGeneric<List<ConcertDtoResponse>>), 200)]
    public async Task<IActionResult> GetConcerts()
    {
        var response = new BaseResponseGeneric<IEnumerable<ConcertDtoResponse>>();

        try
        {
            response.Data = await _context.Set<Concert>()
                .Where(p => p.Status)
                .OrderBy(x => x.DateEvent)
                .AsNoTracking()
                .Select(p => new ConcertDtoResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Place = p.Place,
                    DateEvent = p.DateEvent.ToString("yyyy-MM-dd"), 
                    TimeEvent = p.DateEvent.ToString("HH:mm:ss"),
                    Genre = p.Genre.Name,
                    ImageUrl = p.ImageUrl ?? string.Empty,
                    Description = p.Description,
                    TicketsQuantity = p.TicketsQuantity,
                    UnitPrice = p.UnitPrice,
                    Status = p.Status? "Activo": "Inactivo"
                })                
                .ToListAsync();

            response.Success = true;
        }
        catch (Exception ex) 
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al obtener los registros";
            _logger.LogError(ex, ex.Message);
        }

        return Ok(response);
    }

    //get:api/concerts/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BaseResponseGeneric<ConcertDtoResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponseGeneric<ConcertDtoResponse>), 404)]
    public async Task<IActionResult> GetConcert(int id)
    {
        var response = new BaseResponseGeneric<ConcertDtoResponse>();

        try
        {
            var entity = await _context.Set<Concert>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);                        

            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el registro";
                return NotFound(response);
            }

            var entityGenre = await _context.Set<Genre>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == entity!.GenreId);

            response.Data = new ConcertDtoResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Place = entity.Place,
                DateEvent = entity.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = entity.DateEvent.ToString("HH:mm:ss"),
                Genre = entityGenre!.Name ?? string.Empty,
                ImageUrl = entity.ImageUrl ?? string.Empty,
                Description = entity.Description,
                TicketsQuantity = entity.TicketsQuantity,
                UnitPrice = entity.UnitPrice,
                Status = entity.Status ? "Activo" : "Inactivo"
            };

            response.Success = true;

        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al obtener el registro";
            _logger.LogError(ex, ex.Message);
        }


        return Ok(response);
    }

    // PUT: api/concerts/1

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> PutConcert(int id, ConcertDtoRequest concertDtoRequest)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _context.Set<Concert>()
                .FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el registro";

                return NotFound(response);
            }
           
            entity.Title = concertDtoRequest.Title;
            entity.Description = concertDtoRequest.Description;
            entity.GenreId = concertDtoRequest.IdGenre;
            entity.DateEvent = DateTime.Parse($"{concertDtoRequest.DateEvent} {concertDtoRequest.TimeEvent}");
            entity.Place = concertDtoRequest.Place;
            entity.TicketsQuantity = concertDtoRequest.TicketsQuantity;
            entity.UnitPrice = concertDtoRequest.UnitPrice;

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
    //POST: api/Concerts
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    public async Task<IActionResult> PostConcert(ConcertDtoRequest concertDtoRequest)
    {
        var response = new BaseResponse();

        try
        {
            var entity = new Concert
            {
                Title = concertDtoRequest.Title,
                Description = concertDtoRequest.Description,
                GenreId = concertDtoRequest.IdGenre,
                DateEvent = DateTime.Parse($"{concertDtoRequest.DateEvent} {concertDtoRequest.TimeEvent}"),
                Place = concertDtoRequest.Place,
                TicketsQuantity = concertDtoRequest.TicketsQuantity,
                UnitPrice = concertDtoRequest.UnitPrice,
            };
       

            _context.Set<Concert>().Add(entity);
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
    //DELETE: api/Concert/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> DeleteConcert(int id)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _context.Set<Concert>()
                .FindAsync(id);

            if (entity == null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el registro";

                return NotFound(response);
            }
         
            entity.Status = false;
         
            await _context.SaveChangesAsync();

            response.Success = true;

        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Hubo un error al eliminar el registro";
            _logger.LogError(ex, ex.Message);
        }

        return Ok(response);
    }


}

