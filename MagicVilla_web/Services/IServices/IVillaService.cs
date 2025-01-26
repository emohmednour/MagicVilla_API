using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IVillaService
    {
         Task<T> GetAllAsync<T>();
         Task<T> GetAsync<T>(int id);
         Task<T> CreateAsync<T>(VillaCreateDto createDto);
         Task<T> UpdateAsync<T>(VillaUpdateDto updateDto);
         Task<T> DeleteAsync<T>(int id);
    }
}
