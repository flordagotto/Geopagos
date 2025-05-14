using DTOs;

namespace Services
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDTO>> GetAll();

        Task Create(TournamentDTO tournament);
    }

    public class TournamentService : ITournamentService
    {
        public Task Create(TournamentDTO tournament)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TournamentDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
