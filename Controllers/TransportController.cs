using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.DTO.Transport;
using Simbir.GO.App.DTO.TransportDto;
using Simbir.GO.App.Extensions;
using Simbir.GO.App.Services;
using Simbir.GO.Domain.Models;

namespace Simbir.GO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportController : Controller
    {
        private readonly TransportService _transportService;

        public TransportController(TransportService transportService)
        {
            _transportService = transportService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransport(Guid id)
        {
            Transport transport = await _transportService.GetInfo(id);
            return Ok(transport);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] TransportDto transportDto)
        {
            var transport = await _transportService.RegisterTransport(transportDto,
                User.GetUserId());
            return Ok(transport);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTransport([FromBody] TransportUpdateDto transportUpdate, 
            Guid id)
        {
            await _transportService.UpdateTransport(transportUpdate, id, User.GetUserLogin());
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTransport(Guid id)
        {
            await _transportService.DeleteTransport(id, User.GetUserLogin());
            return Ok();
        }

    }
}
