using AutoMapper;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;

namespace GrossAPI.Utils
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Services, ServicesDTO>();
            CreateMap<ServicesDTO, Services>();
        }
    }
}
