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
        Task Create(NewPlayerDTO player);

        Task<IEnumerable<PlayerDTO>> GetAll();
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

        public async Task Create(NewPlayerDTO newPlayerDTO)
        {
            try
            {
                if (!Enum.IsDefined(newPlayerDTO.Gender))
                    throw new ArgumentException("The gender must be 0 (male) or 1 (female)");

                Player player = newPlayerDTO.Gender switch
                {
                    Gender.Female when HasCorrectData(newPlayerDTO, true) =>
                        FemalePlayer.Create(newPlayerDTO.Name, newPlayerDTO.Skill, newPlayerDTO.ReactionTime!.Value),

                    Gender.Male when HasCorrectData(newPlayerDTO, false) =>
                        MalePlayer.Create(newPlayerDTO.Name, newPlayerDTO.Skill, newPlayerDTO.Strength!.Value, newPlayerDTO.Speed!.Value),

                    _ => throw new ArgumentException("Invalid player data.")
                };

                await _playerRepository.Add(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error creating a player.");
                throw;
            }
        }

        private bool HasCorrectData(NewPlayerDTO player, bool femaleDataIsExpected)
        {
            if (femaleDataIsExpected)
            {
                return player.ReactionTime.HasValue
                    && !player.Strength.HasValue
                    && !player.Speed.HasValue;
            }
            else
            {
                return !player.ReactionTime.HasValue
                    && player.Strength.HasValue
                    && player.Speed.HasValue;
            }
        }

        public async Task<IEnumerable<PlayerDTO>> GetAll()
        {
            var result = new List<PlayerDTO>();

            try
            {
                var players = await _playerRepository.GetAll();

                if (players != null && players.Count != 0)
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
