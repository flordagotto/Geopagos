using AutoMapper;
using Domain.Entities;
using DTOs;

namespace Services.Mapping
{
    public class PlayerMapper : Profile
    {
        public PlayerMapper()
        {
            CreateMap<MalePlayerDTO, MalePlayer>()
                .ConstructUsing(src => MalePlayer.Create(src.Name, src.Skill, src.Strength, src.Speed));

            CreateMap<FemalePlayerDTO, FemalePlayer>()
                .ConstructUsing(src => FemalePlayer.Create(src.Name, src.Skill, src.ReactionTime));

            CreateMap<MalePlayer, MalePlayerDTO>();
            CreateMap<FemalePlayer, FemalePlayerDTO>();
        }
    }
}
