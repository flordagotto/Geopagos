using Domain.Entities;

namespace DAL.Repositories
{
    public interface IPlayerRepository
    {
        Task Add(Player player);
        Task<List<Player>> GetAll();
    }

    public class PlayerRepository : IPlayerRepository
    {
        public Task Add(Player player)
        {
            throw new NotImplementedException();
        }

        public Task<List<Player>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
