using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.DTO.Rent;
using Simbir.GO.App.Extensions;
using Simbir.GO.App.Services;

namespace Simbir.GO.Controllers.Admin
{
    [ApiController]
    [Route("api/Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminRentController : Controller
    {
        private readonly RentService _rentService;

        public AdminRentController(RentService rentService)
        {
            _rentService = rentService;
        }

        [HttpGet("Rent/{rentId}")]
        public async Task<IActionResult> GetRentInfo(Guid rentId)
        {
            var rent = await _rentService.GetRent(rentId, User.GetUserLogin());
            return Ok(rent);
        }

        [HttpGet("UserHistory/{userId}")]
        public async Task<IActionResult> GetUserHistory(Guid userId)
        {
            var rent = await _rentService.GetHistory(userId);
            return Ok(rent);
        }

        [HttpGet("TransportHistory/{transportId}")]
        public async Task<IActionResult> GetTransportHistory(Guid transportId)
        {
            var rent = await _rentService.GetTransportHistory(transportId, User.GetUserLogin());
            return Ok(rent);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdminRentDto adminRentDto)
        {
           var rent = await _rentService.AdminCreateRent(adminRentDto);   
            return Ok(rent);
        }

        [HttpPut("Rent/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AdminRentDto adminRentDto)
        {
             await _rentService.UpdateRent(id, adminRentDto);
            return Ok();
        }

        [HttpDelete("Rent/{rentId}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _rentService.DeleteRent(id);
            return Ok();
        }

    }
}
