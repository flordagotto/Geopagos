using AutoMapper;
using Common.Enums;
using DAL.Repositories;
using Domain.Entities;
using DTOs;
using Microsoft.Extensions.Logging;

namespace Services.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDTO>> GetAll();
    }

    public class MatchService : IMatchService
    {
        public readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MatchService> _logger;

        public MatchService(IMatchRepository matchRepository, IMapper mapper, ILogger<MatchService> logger) {
            _matchRepository = matchRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MatchDTO>> GetAll()
        {
            var result = new List<MatchDTO>();

            try
            {
                var matches = await _matchRepository.GetAll();

                if (matches != null && matches.Count != 0)
                {
                    result = matches.Select(m => _mapper.Map<MatchDTO>(m)).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error retrieving matches.");
                throw;
            }
        }
    }
}
