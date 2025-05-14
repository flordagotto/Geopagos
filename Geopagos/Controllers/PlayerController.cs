using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace GeoPagos.Controllers
{
    [ApiController]
    [Route("Player")]
    public class PlayerController : ControllerBase
    {
        public readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _playerService.GetAll();

            return Ok(results);
        }

        [HttpPost("/Female")]
        public async Task<IActionResult> CreateFemalePlayer([FromBody] FemalePlayerDTO player)
        {
            await _playerService.Create(player);

            return Ok();
        }

        [HttpPost("/Male")]
        public async Task<IActionResult> Create([FromBody] MalePlayerDTO player)
        {
            await _playerService.Create(player);

            return Ok();
        }
    }
}
