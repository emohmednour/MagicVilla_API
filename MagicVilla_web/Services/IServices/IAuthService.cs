using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO obj);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO obj);
    }
}
