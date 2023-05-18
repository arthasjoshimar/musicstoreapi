using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStoreServices;
public interface IGenreService
{
    Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListAsync();
    Task<BaseResponseGeneric<GenreDtoResponse>> GetAsync(int id);
    Task<BaseResponseGeneric<int>> AddAsync(GenreDtoRequest resquest);
    Task<BaseResponse> UpdateAsync(int id, GenreDtoRequest request);
    Task <BaseResponse> DeleteAsync(int id);
}
