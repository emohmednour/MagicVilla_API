using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;


namespace MagicVilla_VillaAPI.Controllers.v1
{
    //[Route("api/[controller]")] 
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepository _DbVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(IVillaRepository DbVilla, IMapper mapper)
        {
            _DbVilla = DbVilla;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = "Default30")]
        //[ResponseCache(Duration = 30)]
        public async Task<ActionResult<APIResponse>> GetVillas(
            [FromQuery(Name ="FilterOcuupancy")] int? Occupancy,
            [FromQuery] string? Search, int sizepage = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Villa> VillaList;
                if (Occupancy > 0)
                {
                    VillaList = await _DbVilla.GetAllAsync( u => u.Occupancy == Occupancy , sizepage: sizepage,pageNumber:  pageNumber);
                }
                else
                {
                    VillaList =  await _DbVilla.GetAllAsync( sizepage: sizepage,pageNumber:  pageNumber);
                }
                if (!string.IsNullOrEmpty(Search)) {

                    VillaList = VillaList.Where(U => U.Name.ToLower().Contains(Search));
                }
                Pagination pagination = new Pagination { PageNumber = pageNumber , PageSize = sizepage };
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<VillaDto>>(VillaList);
                _response.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }



        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ResponseCache( Location = ResponseCacheLocation.None ,NoStore = true )]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _DbVilla.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    _response.HttpStatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.HttpStatusCode = HttpStatusCode.OK;
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (await _DbVilla.GetAsync(u => u.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    // The name is not unique, add a custom error to ModelState
                    ModelState.AddModelError("ErrorMessages", "Villa already exists!");
                    // Return a 400 Bad Request response with the error details
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {

                    return BadRequest(createDto);
                }
                Villa model = _mapper.Map<Villa>(createDto);

                await _DbVilla.CreateAsync(model);

                _response.Result = _mapper.Map<VillaDto>(model);
                _response.HttpStatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;



        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _DbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                await _DbVilla.RemoveAsync(villa);


                _response.HttpStatusCode = HttpStatusCode.NoContent;
                _response.Result = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;


        }




        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    return BadRequest();
                }

                Villa villa = _mapper.Map<Villa>(updateDto);


                await _DbVilla.UpdateAsync(villa);


                _response.HttpStatusCode = HttpStatusCode.NoContent;
                _response.Result = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }

            var villa = await _DbVilla.GetAsync(u => u.Id == id, tracked: false);
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);


            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDto, ModelState);

            Villa model = _mapper.Map<Villa>(villaDto);


            await _DbVilla.UpdateAsync(villa);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

    }
}
