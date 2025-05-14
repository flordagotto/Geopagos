using AutoMapper;

namespace DAL.Mapping
{
    public class PlayerMapper : Profile
    {
        public PlayerMapper()
        {
            CreateMap<Domain.Entities.FemalePlayer, Entities.FemalePlayer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Domain.Entities.MalePlayer, Entities.MalePlayer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Domain.Entities.Player, Entities.Player>()
                .Include<Domain.Entities.FemalePlayer, Entities.FemalePlayer>()
                .Include<Domain.Entities.MalePlayer, Entities.MalePlayer>();
        }
    }
}
