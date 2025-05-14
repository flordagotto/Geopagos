using DTOs;

namespace Services
{
    public interface IMatchService
    {
        Task<IEnumerable<PlayerDTO>> GetAll();

        Task Create(MatchDTO match);
    }

    public class MatchService : IMatchService
    {
        public MatchService() { }

        public Task Create(MatchDTO match)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<PlayerDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
