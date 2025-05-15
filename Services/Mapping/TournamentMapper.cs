using AutoMapper;
using Domain.Entities;
using DTOs;

namespace Services.Mapping
{
    public class TournamentMapper : Profile
    {
        public TournamentMapper()
        {
            CreateMap<TournamentDTO, Tournament>();

            CreateMap<Tournament, TournamentDTO>();
        }
    }
}
