using AutoMapper;

namespace DAL.Mapping
{
    public class MatchMapper : Profile
    {
        public MatchMapper()
        {
            CreateMap<Domain.Entities.Match, Entities.Match>();

            CreateMap<Entities.Match, Domain.Entities.Match>();
        }
    }
}
