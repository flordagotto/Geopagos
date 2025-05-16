using AutoMapper;

namespace DAL.Mapping
{
    public class PlayerMapper : Profile
    {
        public PlayerMapper()
        {
            CreateMap<Domain.Entities.FemalePlayer, Entities.FemalePlayer>();

            CreateMap<Domain.Entities.MalePlayer, Entities.MalePlayer>();

            CreateMap<Domain.Entities.Player, Entities.Player>()
                .Include<Domain.Entities.FemalePlayer, Entities.FemalePlayer>()
                .Include<Domain.Entities.MalePlayer, Entities.MalePlayer>();

            CreateMap<Entities.FemalePlayer, Domain.Entities.FemalePlayer>()
            .ConstructUsing(src => Domain.Entities.FemalePlayer.Create(src.Name, src.Skill, src.ReactionTime));

            CreateMap<Entities.MalePlayer, Domain.Entities.MalePlayer>()
                .ConstructUsing(src => Domain.Entities.MalePlayer.Create(src.Name, src.Skill, src.Strength, src.Speed));

            CreateMap<Entities.Player, Domain.Entities.Player>()
                .Include<Entities.FemalePlayer, Domain.Entities.FemalePlayer>()
                .Include<Entities.MalePlayer, Domain.Entities.MalePlayer>();
        }
    }
}
