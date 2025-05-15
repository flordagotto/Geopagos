using AutoMapper;
using Common.Enums;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface ITournamentRepository
    {
        Task<List<Domain.Entities.Tournament>> GetFiltered(Gender? type, DateTime? from, DateTime? to, bool? isFinished);
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

        public async Task<List<Domain.Entities.Tournament>> GetFiltered(Gender? type, DateTime? fromDate, DateTime? toDate, bool? isFinished)
        {
            IQueryable<Tournament> query = _context.Tournaments;

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
    }
}
