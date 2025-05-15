using AutoMapper;

namespace DAL.Mapping
{
    public class MatchMapper : Profile
    {
        public MatchMapper()
        {
            CreateMap<Domain.Entities.Match, Entities.Match>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Entities.Match, Domain.Entities.Match>();
        }
    }
}
