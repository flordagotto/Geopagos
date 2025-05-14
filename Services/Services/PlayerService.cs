using AutoMapper;
using Common.Enums;
using DAL.Repositories;
using Domain.Entities;
using DTOs;
using Microsoft.Extensions.Logging;

namespace Services.Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDTO>> GetAll();

        Task Create(PlayerDTO player);
    }

    public class PlayerService : IPlayerService
    {
        public readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository playerRepository, IMapper mapper, ILogger<PlayerService> logger) 
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Create(PlayerDTO playerDTO)
        {
            try
            {
                Player player;

                if (playerDTO.Gender == Gender.Female && playerDTO is FemalePlayerDTO femaleDto)
                {
                    player = FemalePlayer.Create(femaleDto.Name, femaleDto.Skill, femaleDto.ReactionTime);
                }
                else if (playerDTO.Gender == Gender.Male && playerDTO is MalePlayerDTO maleDto)
                {
                    player = MalePlayer.Create(maleDto.Name, maleDto.Skill, maleDto.Strength, maleDto.Speed);
                }
                else
                {
                    throw new ArgumentException("Unknown gender for player.");
                }

                await _playerRepository.Add(player);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Error creating player.");
                throw;
            }
        }

        public async Task<IEnumerable<PlayerDTO>> GetAll()
        {
            var result = new List<PlayerDTO>();

            try
            {
                var players = await _playerRepository.GetAll();

                if (players.Count != 0)
                {
                    result = players.Select<Player, PlayerDTO>(p => p.Gender switch
                    {
                        Gender.Female => _mapper.Map<FemalePlayerDTO>(p),
                        Gender.Male => _mapper.Map<MalePlayerDTO>(p),
                        _ => throw new NotImplementedException()
                    }).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error retrieving players.");
                throw;
            }
        }
    }
}
