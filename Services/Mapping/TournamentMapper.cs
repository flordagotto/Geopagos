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

            CreateMap<Tournament, TournamentDTO>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players))
                .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => src.Matches));

            CreateMap<Player, PlayerDTO>()
                .Include<FemalePlayer, FemalePlayerDTO>()
                .Include<MalePlayer, MalePlayerDTO>();
        }
    }
}
