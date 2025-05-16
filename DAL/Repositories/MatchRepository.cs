using AutoMapper;
using DAL.Context;
using Domain.Entities;

namespace DAL.Repositories
{
    public interface IMatchRepository
    {
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

        public async Task Add(Match newMatch)
        {
            var entityMatch = _mapper.Map<Entities.Match>(newMatch);

            _context.Matches.Add(entityMatch);
        }
    }
}
