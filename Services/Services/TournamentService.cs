using AutoMapper;
using DAL.Repositories;
using Domain.Entities;
using DTOs;
using Microsoft.Extensions.Logging;

namespace Services.Services
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDTO>> GetByFilter(TournamentFilterDto filter);

        Task Create(NewTournamentDTO tournament);

        Task<Player> StartTournament(Guid tournamentId);
    }

    public class TournamentService : ITournamentService
    {
        public readonly ITournamentRepository _tournamentRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TournamentService> _logger;

        public TournamentService(ITournamentRepository tournamentRepository, IPlayerRepository playerRepository, IMatchRepository matchRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<TournamentService> logger)
        {
            _tournamentRepository = tournamentRepository;
            _playerRepository = playerRepository;
            _matchRepository = matchRepository;
            _unitOfWork = unitOfWork;
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

        public async Task Create(NewTournamentDTO newTournament)
        {
            try
            {
                if(!Enum.IsDefined(newTournament.Type))
                    throw new ArgumentException("The gender must be 0 (male) or 1 (female)");

                var tournamentPlayers = newTournament.Players.GroupBy(x => x).Where(x => x.Count() > 1);

                if (tournamentPlayers.Any())
                    throw new ArgumentException("The players can not be repeated.");

                var players = await _playerRepository.GetByIds(newTournament.Players);
                var missing = newTournament.Players.Except(players.Select(p => p.Id));

                if (missing.Any())
                    throw new ArgumentException($"Some players not found: {string.Join(", ", missing)}");

                if (players == null || !players.Any())
                    throw new ArgumentException("No valid players found for the tournament.");

                Tournament tournament = Tournament.Create(newTournament.Type, players);

                await _tournamentRepository.Add(tournament);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error creating a new tournament.");
                throw;
            }
        }

        public async Task<Player> StartTournament(Guid tournamentId)
        {
            try
            {
                var tournament = await _tournamentRepository.GetById(tournamentId) ?? throw new Exception("Tournament not found");

                if (tournament.IsFinished)
                    throw new Exception("The tournament is already finished.");
                
                var winner = tournament.Start();

                foreach (var match in tournament.Matches)
                    await _matchRepository.Add(match);

                await _tournamentRepository.SetWinner(tournamentId, winner.Id);

                await _unitOfWork.SaveChangesAsync();

                return winner;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error starting the tournament.");
                throw;
            }
        }
    }
}
