using MusicStore.Entities;

namespace MusicStore.Repositories
{
    public interface IGenreRepository
    {
        Task<ICollection<Genre>> ListAsync();
        Task<Genre?> GetAsync(int id);
        Task<int> AddAsync(Genre genre);
        Task DeleteAsync(int id);
        Task UpdateAsync();       
    }
}