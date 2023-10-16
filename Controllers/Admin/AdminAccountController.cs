using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.GO.App.DTO.AccountDto;
using Simbir.GO.App.Services;

namespace Simbir.GO.Controllers.Admin
{
    [ApiController]
    [Route("api/Admin/Account")]
    [Authorize(Roles = "Admin")]
    public class AdminAccountController : Controller
    {
        private readonly AccountService _accountService;

        public AdminAccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(int start, int count)
        {
            var accounts = await _accountService.GetAccounts(start, count);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserInfo(Guid id)
        {
            var account = await _accountService.GetUserById(id);
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> AccountPost([FromBody] AdminAccountDto adminAccountDto)
        {
           
            await _accountService.RegisterAccount(adminAccountDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AccountUpdate(Guid id, [FromBody] AdminAccountDto adminAccountDto)
        {
            await _accountService.UpdateAccount(adminAccountDto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> AccountDelete(Guid id)
        {
            await _accountService.DeleteAccount(id);
            return Ok();
        }

    }
}
