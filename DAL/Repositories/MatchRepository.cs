using AutoMapper;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface IMatchRepository
    {
        Task<List<Domain.Entities.Match>> GetAll();
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

        public async Task<List<Domain.Entities.Match>> GetAll()
        {
            var dalMatches = await _context.Matches.ToListAsync();

            var domainMatches = new List<Domain.Entities.Match>();

            foreach (var dalMatch in dalMatches)
            {
                var domainMatch = _mapper.Map<Domain.Entities.Match>(dalMatch);

                domainMatches.Add(domainMatch);
            }

            return domainMatches;
        }
    }
}
