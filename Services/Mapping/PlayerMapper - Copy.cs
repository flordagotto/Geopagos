using AutoMapper;
using Domain.Entities;
using DTOs;

namespace Services.Mapping
{
    public class MatchMapper : Profile
    {
        public MatchMapper()
        {
            CreateMap<MatchDTO, Match>();

            CreateMap<Match, MatchDTO>();
        }
    }
}
