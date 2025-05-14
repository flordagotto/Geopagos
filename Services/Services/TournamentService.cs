using DTOs;

namespace Services.Services
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDTO>> GetAll();

        Task<IEnumerable<TournamentDTO>>? GetByFilter(TournamentFilterDto tournament);

        Task Create(NewTournamentDTO tournament);
    }

    public class TournamentService : ITournamentService
    {
        public Task<IEnumerable<TournamentDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TournamentDTO>>? GetByFilter(TournamentFilterDto tournament)
        {
            throw new NotImplementedException();
        }
        public Task Create(NewTournamentDTO tournament)
        {
            throw new NotImplementedException();
        }

    }
}
