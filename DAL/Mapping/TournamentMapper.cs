using AutoMapper;

namespace DAL.Mapping
{
    public class TournamentMapper : Profile
    {
        public TournamentMapper()
        {
            CreateMap<Domain.Entities.Tournament, Entities.Tournament>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Entities.Tournament, Domain.Entities.Tournament>();
        }
    }
}
