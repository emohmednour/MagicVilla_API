using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagicVilla_web.Services.IServices;
using MagicVilla_Web.Services;

namespace MagicVilla_web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory httpClient;
        private string villaUrl;
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            this.httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.POST,
                Data = obj,
                Url= villaUrl + "/api/v1/UsersAuth/login",
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.POST,
                Data = obj,
                Url = villaUrl + "/api/v1/UsersAuth/register",
            });
        }
    }
}