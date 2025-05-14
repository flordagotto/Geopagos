using Common.Enums;
using DAL.Repositories;
using Domain.Entities;
using DTOs;

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

        public PlayerService(IPlayerRepository playerRepository) {
            _playerRepository = playerRepository;
        }

        public async Task Create(PlayerDTO playerDTO)
        {
            try
            {
                Player player;

                if (playerDTO.Gender == Gender.Female)
                {
                    var femalePlayerDTO = (FemalePlayerDTO)playerDTO;
                    player = FemalePlayer.Create(femalePlayerDTO.Name, femalePlayerDTO.Skill, femalePlayerDTO.ReactionTime);
                }
                else
                {
                    var malePlayerDTO = (MalePlayerDTO)playerDTO;
                    player = MalePlayer.Create(malePlayerDTO.Name, malePlayerDTO.Skill, malePlayerDTO.Strength, malePlayerDTO.Speed);
                }

                await _playerRepository.Add(player);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<IEnumerable<PlayerDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
