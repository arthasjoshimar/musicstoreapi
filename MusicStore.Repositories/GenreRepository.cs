using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;

namespace MusicStore.Repositories;
public class GenreRepository : IGenreRepository
{
    private readonly MusicStoreDbContext _musicStoreDbContext;

    public GenreRepository(MusicStoreDbContext musicStoreDbContext)
    {
        _musicStoreDbContext = musicStoreDbContext;
    }
    public async Task<int> AddAsync(Genre genre)
    {
        _musicStoreDbContext.Set<Genre>()
            .Add(genre);
        await _musicStoreDbContext.SaveChangesAsync();
        return genre.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var genre = await _musicStoreDbContext.Set<Genre>().FindAsync(id);
        if (genre != null)
        {
            genre.Status = false;
            await _musicStoreDbContext.SaveChangesAsync();
        }
            
    }

    public async Task<Genre?> GetAsync(int id)
    {
        return await _musicStoreDbContext.Set<Genre>()
           .FindAsync(id);
    }

    public async Task<ICollection<Genre>> ListAsync()
    {
        return await _musicStoreDbContext.Set<Genre>()
           .Where(p => p.Status)
           .AsNoTracking()
           .ToListAsync();
    }

    public async Task UpdateAsync()
    {        
        await _musicStoreDbContext.SaveChangesAsync();
    }
}

