using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace GeoPagos.Controllers
{
    [Route("Tournament")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        public readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _tournamentService.GetAll();

            return Ok(results);
        }

        [HttpGet("/filter")]
        public async Task<IActionResult> GetAll([FromBody] TournamentFilterDto filter)
        {
            var results = await _tournamentService.GetByFilter(filter);

            return Ok(results);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] TournamentDTO tournament)
        {
            await _tournamentService.Create(tournament);

            return Ok();
        }
    }
}
