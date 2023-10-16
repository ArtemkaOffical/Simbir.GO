using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.DTO.Rent;
using Simbir.GO.App.Extensions;
using Simbir.GO.App.Services;
using Simbir.GO.Domain.Models;

namespace Simbir.GO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentController : Controller
    {
        private readonly RentService _rentService;

        public RentController(RentService rentService)
        {
            _rentService = rentService;
        }

        [HttpGet("/Transport")]
        public async Task<IActionResult> GetTransports(RentTransportDto rentTransportDto)
        {
           List<Transport> transports = await _rentService.GetTransports(rentTransportDto);

            if(transports.Count == 0) 
                return NotFound("в текущем радиусе доступного транспорта нет");

            return Ok(transports);
        }

        [HttpGet("/{rentId}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid rentId)
        {
            Rent rent = await _rentService.GetRent(rentId, User.GetUserLogin());

            return Ok(rent);
        }

        [HttpGet("/MyHistory")]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            List<Rent> rents = await _rentService.GetHistory(User.GetUserLogin());

            return Ok(rents);
        }

        [HttpGet("/TransportHistory/{transportId}")]
        [Authorize]
        public async Task<IActionResult> GetTransportHistory(Guid transportId)
        {
            List<Rent> rents = await _rentService.GetTransportHistory(transportId, User.GetUserLogin());

            return Ok(rents);
        }

        [HttpPost("/New/{transportId}")]
        [Authorize]
        public async Task<IActionResult> CreateRent(Guid transportId, string rentType)
        {
            await _rentService.CreateRent(transportId, User.GetUserLogin(), rentType);
            return Ok();
        }

        [HttpPost("/End/{rentId}")]
        [Authorize]
        public async Task<IActionResult> CreateRent(Guid rentId, RentDto rentDto)
        {
            await _rentService.EndRent(rentId, User.GetUserLogin(), rentDto);
            return Ok();
        }

    }
}
