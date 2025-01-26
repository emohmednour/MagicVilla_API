using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagicVilla_web.Services.IServices;
using MagicVilla_Web.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory httpClient;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            this.httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public  Task<T> CreateAsync<T>(VillaNumberCreateDTO createDto)
        {
            return  SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.POST,
                Data = createDto,
                Url = villaUrl + "/api/VillaNumberAPI"


            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return  SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.DELETE,
               
                Url = villaUrl + "/api/VillaNumberAPI/" + id


            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.GET,
                Url = villaUrl + "/api/VillaNumberAPI"


            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.GET,
                Url = villaUrl + "/api/VillaNumberAPI/" + id


            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO updateDto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APItype.PUT,
                Data = updateDto,
                Url = villaUrl + "/api/VillaNumberAPI/" + updateDto.VillaNo


            });
        }
    }
}
