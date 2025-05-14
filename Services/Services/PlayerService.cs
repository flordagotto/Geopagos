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
        public PlayerService() { }

        public Task Create(PlayerDTO player)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<PlayerDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
