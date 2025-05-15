using AutoMapper;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface IPlayerRepository
    {
        Task Add(Domain.Entities.Player player);

        Task<List<Domain.Entities.Player>> GetAll();

        Task<List<Domain.Entities.Player>> GetByIds(IEnumerable<Guid> ids);
    }

    public class PlayerRepository : IPlayerRepository
    {
        private readonly GameDbContext _context;
        private readonly IMapper _mapper;

        public PlayerRepository(GameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Domain.Entities.Player player)
        {
            Player entityPlayer = player switch
            {
                Domain.Entities.FemalePlayer female => _mapper.Map<FemalePlayer>(female),
                Domain.Entities.MalePlayer male => _mapper.Map<MalePlayer>(male),
                _ => throw new ArgumentException("Unknown player type")
            };

            _context.Players.Add(entityPlayer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Domain.Entities.Player>> GetAll()
        {
            var dalPlayers = await _context.Players.ToListAsync();

            return MapPlayers(dalPlayers);
        }

        public async Task<List<Domain.Entities.Player>> GetByIds(IEnumerable<Guid> ids)
        {
            var dalPlayers = await _context.Players.Where(x => ids.Contains(x.Id)).ToListAsync();

            var domainPlayers = MapPlayers(dalPlayers);

            return domainPlayers;
        }

        private List<Domain.Entities.Player> MapPlayers(List<Player> dalPlayers)
        {
            var domainPlayers = new List<Domain.Entities.Player>();

            foreach (var dalPlayer in dalPlayers)
            {
                Domain.Entities.Player domainPlayer = dalPlayer switch
                {
                    FemalePlayer female => _mapper.Map<Domain.Entities.FemalePlayer>(female),
                    MalePlayer male => _mapper.Map<Domain.Entities.MalePlayer>(male),
                    _ => throw new ArgumentException("Unknown DAL player type")
                };

                domainPlayers.Add(domainPlayer);
            }

            return domainPlayers;
        }
    }
}
