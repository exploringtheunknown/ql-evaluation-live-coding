using AutoMapper;
using Standard.API.PSQL.Domain.DTOs;
using Standard.API.PSQL.Domain.Entities;


namespace Standard.API.PSQL.Infra.Data.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SampleDto, Sample>().ReverseMap();
        }
    }
}