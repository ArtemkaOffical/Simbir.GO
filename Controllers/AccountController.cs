using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.DTO.AccountDto;
using Simbir.GO.App.Extensions;
using Simbir.GO.App.Services;

namespace Simbir.GO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> AccountGetInfo()
        {
            return Ok(await _accountService.GetInfo(User.GetUserLogin()));
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> AccountSignIn([FromBody] AccountDto accountDto)
        {
            string token = await _accountService.SignIn(accountDto);
            HttpContext.Session.SetString("token",token);

            return Ok(token);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> AccountSignUp([FromBody]AccountDto accountDto)
        {
            await _accountService.RegisterAccount(accountDto);

            return Ok();
        }

        [HttpPost("SignOut")]
        [Authorize]
        public async Task<IActionResult> AccountSignOut()
        {
            await _accountService.SignOut(HttpContext.Session.GetString("token"));
            HttpContext.Session.Clear();
            return StatusCode(401);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> AccountUpdate([FromBody]AccountDto accountDto)
        {
            await _accountService.UpdateAccount(accountDto, User.GetUserId());

            return Ok();
        }

    }
}
