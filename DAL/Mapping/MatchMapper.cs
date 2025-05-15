using AutoMapper;

namespace DAL.Mapping
{
    public class MatchMapper : Profile
    {
        public MatchMapper()
        {
            CreateMap<Domain.Entities.Match, Entities.Match>()
                .ForMember(dest => dest.Player1, opt => opt.Ignore())
                .ForMember(dest => dest.Player2, opt => opt.Ignore())
                .ForMember(dest => dest.Winner, opt => opt.Ignore())
                .ForMember(dest => dest.Tournament, opt => opt.Ignore());

            CreateMap<Entities.Match, Domain.Entities.Match>()
                .ForMember(dest => dest.Player1, opt => opt.Ignore())
                .ForMember(dest => dest.Player2, opt => opt.Ignore())
                .ForMember(dest => dest.Winner, opt => opt.Ignore())
                .ForMember(dest => dest.Tournament, opt => opt.Ignore());
        }
    }
}
