using AutoMapper;
using MagicVilla_web.Models.DTO;


namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
          
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();


            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();

        }
    }
}
