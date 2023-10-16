using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.Extensions;
using Simbir.GO.App.Services;

namespace Simbir.GO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("Payment/Hesoyam/{accountId}")]
        [Authorize]
        public async Task<IActionResult> AddBalane(Guid accountId)
        {
            await _paymentService.AddBalance(accountId, User.GetUserLogin());
            return Ok();
        }
    }
}
