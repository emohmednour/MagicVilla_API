using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    
    public class VillaNumberAPIController : Controller
    {
        private readonly IVillaNumberRepository _DbVillaNumber;
        private readonly IVillaRepository _villa;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberAPIController(IVillaNumberRepository DbVillaNumber, IVillaRepository villa, IMapper mapper)
        {
            _DbVillaNumber = DbVillaNumber;
            _villa = villa;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "string2" };
        }


        //GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {

                IEnumerable<VillaNumber> VillaNumbersList = await _DbVillaNumber.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(VillaNumbersList);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

       

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                VillaNumber VillaNumber = await _DbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (VillaNumber == null)
                {
                    _response.HttpStatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDTO>(VillaNumber);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if (await _DbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _villa.GetAsync(u => u.Id == createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

                await _DbVillaNumber.CreateAsync(villaNumber);


                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.HttpStatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }


        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                VillaNumber VillaNumber = await _DbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (VillaNumber == null)
                {
                    _response.HttpStatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _DbVillaNumber.RemoveAsync(VillaNumber);
                _response.HttpStatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }
                if (await _villa.GetAsync(u => u.Id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(updateDTO);
                await _DbVillaNumber.UpdateAsync(villaNumber);
                _response.HttpStatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }





    }
}
