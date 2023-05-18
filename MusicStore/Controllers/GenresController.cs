using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStoreServices;

namespace MusicStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController: ControllerBase
{
    private readonly IGenreService _service;

    public GenresController(IGenreService service)
	{
        _service = service;
    }

    //get: api/genres
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponseGeneric<List<GenreDtoResponse>>), 200)]
    public async Task<IActionResult> GetGenres()
    {       
        return Ok(await _service.ListAsync());
    }

    //get:api/genres/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BaseResponseGeneric<GenreDtoResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponseGeneric<GenreDtoResponse>), 404)]
    public async Task<IActionResult> GetGenre(int id)
    {
        return Ok(await _service.GetAsync(id));
    }

    //PUT: api/Genres/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse),200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> PutGenre(int id, GenreDtoRequest genreDtoRequest)
    {
        return Ok(await _service.UpdateAsync(id,genreDtoRequest));
    }

    //POST: api/Genres
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse),200)]
    public async Task<IActionResult> PostGenre(GenreDtoRequest genreDtoRequest)
    {       
        return Ok(await _service.AddAsync(genreDtoRequest));
    }

    //DELETE: api/Genre/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> DeleteGenre(int id)
    {       
        return Ok(await _service.DeleteAsync(id));
    }
}

