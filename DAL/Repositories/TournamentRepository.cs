using AutoMapper;
using Common.Enums;
using DAL.Context;
using DAL.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface ITournamentRepository
    {
        Task Add(Domain.Entities.Tournament tournament);

        Task<List<Domain.Entities.Tournament>> GetFiltered(Gender? type, DateTime? from, DateTime? to, bool? isFinished);

        Task<Domain.Entities.Tournament>? GetById(Guid id);

        Task SetWinner(Guid tournamentId, Guid playerId);
    }

    public class TournamentRepository : ITournamentRepository
    {
        private readonly GameDbContext _context;
        private readonly IMapper _mapper;

        public TournamentRepository(GameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Domain.Entities.Tournament tournament)
        {
            Entities.Tournament entityTournament = _mapper.Map<Entities.Tournament>(tournament);
            entityTournament.WinnerId = null;

            _context.Tournaments.Add(entityTournament);

            foreach (var player in tournament.Players)
            {
                var playerTournament = new PlayersByTournament
                {
                    TournamentId = entityTournament.Id,
                    PlayerId = player.Id
                };

                _context.PlayersByTournament.Add(playerTournament);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Domain.Entities.Tournament>> GetFiltered(Gender? type, DateTime? fromDate, DateTime? toDate, bool? isFinished)
        {
            IQueryable<Entities.Tournament> query = _context.Tournaments;

            if (type.HasValue)
                query = query.Where(t => t.Type == type.Value);

            if (fromDate.HasValue)
                query = query.Where(t => t.Created >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.Created <= toDate.Value);

            if (isFinished.HasValue)
                query = query.Where(t => t.IsFinished == isFinished.Value);

            var dalTournaments = await query.ToListAsync();

            var domainTournaments = new List<Domain.Entities.Tournament>();

            foreach (var dalTournament in dalTournaments)
            {
                var domainTournament = _mapper.Map<Domain.Entities.Tournament>(dalTournament);

                domainTournaments.Add(domainTournament);
            }

            return domainTournaments;
        }

        public async Task<Domain.Entities.Tournament>? GetById(Guid id)
        {
            var result = await _context.Tournaments
                .Include(t => t.Players)
                .ThenInclude(pt => pt.Player)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return null;

            var domainPlayersInTournament = new List<Domain.Entities.Player>();

            foreach (var player in result.Players)
            {
                var dalPlayer = await _context.Players.FirstOrDefaultAsync(x => x.Id == player.PlayerId);
                domainPlayersInTournament.Add(_mapper.Map<Domain.Entities.Player>(dalPlayer));
            }

            var tournament = TournamentFactory.FromPersistence(
                result.Id, 
                result.Type, 
                result.Created, 
                result.IsFinished, 
                result.WinnerId,
                domainPlayersInTournament
                );

            return tournament;
        }

        public async Task SetWinner(Guid tournamentId, Guid playerId)
        {
            var tournament = await _context.Tournaments
                .FirstAsync(x => x.Id == tournamentId);

            tournament.WinnerId = playerId;

            await _context.SaveChangesAsync();
        }
    }
}
