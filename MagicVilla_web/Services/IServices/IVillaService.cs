using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IVillaService
    {
         Task<T> GetAllAsync<T>(string token);
         Task<T> GetAsync<T>(int id, string token);
         Task<T> CreateAsync<T>(VillaCreateDto createDto, string token);
         Task<T> UpdateAsync<T>(VillaUpdateDto updateDto, string token);
         Task<T> DeleteAsync<T>(int id, string token);
    }
}
