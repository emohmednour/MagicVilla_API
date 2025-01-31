using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : Controller
    {
        private readonly IVillaNumberRepository _DbVillaNumber;
        private readonly IVillaRepository _villa;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberAPIController(IVillaNumberRepository DbVillaNumber,IVillaRepository villa, IMapper mapper)
        {
            _DbVillaNumber = DbVillaNumber;
            _villa = villa;
            _mapper = mapper;
            _response = new APIResponse();
        }
        
        
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Bhrugen", "DotNetMastery" };
        }






    }
}
