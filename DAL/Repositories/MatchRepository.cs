using AutoMapper;
using DAL.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetAll();

        Task Add(Match newMatch);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly GameDbContext _context;
        private readonly IMapper _mapper;

        public MatchRepository(GameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Match>> GetAll()
        {
            var dalMatches = await _context.Matches
                .Include(x => x.Player1)
                .Include(x => x.Player2)
                .Include(x => x.Winner)
                .ToListAsync();

            var domainMatches = new List<Match>();

            foreach (var dalMatch in dalMatches)
            {
                var player1 = _mapper.Map<Player>(dalMatch.Player1);
                var player2 = _mapper.Map<Player>(dalMatch.Player2);
                var winner = _mapper.Map<Player>(dalMatch.Winner);

                var domainMatch = MatchFactory.LoadFromPersistance(
                    dalMatch.Id,
                    dalMatch.Round,
                    player1,
                    player2,
                    winner
                    );

                domainMatches.Add(domainMatch);
            }

            return domainMatches;
        }

        public async Task Add(Match newMatch)
        {
            var entityMatch = _mapper.Map<Entities.Match>(newMatch);

            _context.Matches.Add(entityMatch);
        }
    }
}
