using AutoMapper;
using DAL.Repositories;
using DTOs;
using Microsoft.Extensions.Logging;

namespace Services.Services
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDTO>> GetByFilter(TournamentFilterDto filter);

        Task Create(NewTournamentDTO tournament);
    }

    public class TournamentService : ITournamentService
    {
        public readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MatchService> _logger;

        public TournamentService(ITournamentRepository tournamentRepository, IMapper mapper, ILogger<MatchService> logger)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TournamentDTO>> GetByFilter(TournamentFilterDto filter)
        {
            var result = new List<TournamentDTO>();

            try
            {
                var tournaments = await _tournamentRepository.GetFiltered(filter.Type, filter.FromDate, filter.ToDate, filter.IsFinished);

                if (tournaments != null && tournaments.Count != 0)
                {
                    result = tournaments.Select(t => _mapper.Map<TournamentDTO>(t)).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error retrieving tournaments.");
                throw;
            }
        }

        public Task Create(NewTournamentDTO tournament)
        {
            throw new NotImplementedException();
        }

    }
}
