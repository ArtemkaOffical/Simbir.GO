using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.DTO.Transport;
using Simbir.GO.App.DTO.TransportDto;
using Simbir.GO.App.Extensions;
using Simbir.GO.App.Services;

namespace Simbir.GO.Controllers.Admin
{
    [ApiController]
    [Route("api/Admin/Transport")]
    [Authorize(Roles = "Admin")]
    public class AdminTranstorController : Controller
    {
        private readonly TransportService _transportService;

        public AdminTranstorController(TransportService transportService)
        {
            _transportService = transportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransports(int start, int count, string transportType)
        {
            var accounts = await _transportService.GetTransports(start, count,transportType);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransportInfo(Guid id)
        {
            var transport = await _transportService.GetInfo(id);
            return Ok(transport);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdminTransportDto aAdminTransportDto)
        {
            await _transportService.RegisterTransport(aAdminTransportDto, aAdminTransportDto.OwnerId);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AdminTransportDto aAdminTransportDto)
        {
            await _transportService.UpdateTransport(aAdminTransportDto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _transportService.DeleteTransport(id, User.GetUserLogin());
            return Ok();
        }

    }
}
