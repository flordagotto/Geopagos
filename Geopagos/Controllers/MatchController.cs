using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace GeoPagos.Controllers
{
    [Route("Match")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        public readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _matchService.GetAll();

            return Ok(results);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] MatchDTO match)
        {
            await _matchService.Create(match);

            return Ok();
        }
    }
}
