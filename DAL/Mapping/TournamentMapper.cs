using AutoMapper;

namespace DAL.Mapping
{
    public class TournamentMapper : Profile
    {
        public TournamentMapper()
        {
            CreateMap<Domain.Entities.Tournament, Entities.Tournament>()
                .ForMember(dest => dest.Players, opt => opt.Ignore());

            CreateMap<Entities.Tournament, Domain.Entities.Tournament>()
                .ForMember(dest => dest.Players, opt => opt.Ignore());
        }
    }
}
