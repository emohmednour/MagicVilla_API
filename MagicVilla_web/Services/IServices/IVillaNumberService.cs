using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IVillaNumberService
    {
         Task<T> GetAllAsync<T>();
         Task<T> GetAsync<T>(int id);
         Task<T> CreateAsync<T>(VillaNumberCreateDTO createDto);
         Task<T> UpdateAsync<T>(VillaNumberUpdateDTO updateDto);
         Task<T> DeleteAsync<T>(int id);
    }
}
