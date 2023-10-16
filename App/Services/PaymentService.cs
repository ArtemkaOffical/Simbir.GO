using Microsoft.EntityFrameworkCore;
using Simbir.GO.Infrastructure.Context;

namespace Simbir.GO.App.Services
{
    public class PaymentService
    {
        private readonly DataContext _context;
        private readonly AccountService _accountService;

        public PaymentService(DataContext dataContext, AccountService accountService)
        {
            _context = dataContext;
            _accountService = accountService;
        }

        public async Task AddBalance(Guid id, string userName)
        {
            var account = await _accountService.GetUserById(id);
            if (account == null)
                throw new Exception("пользователь не найден");

            var InvokedAccount = await _accountService.GetUserByName(userName);
            if (InvokedAccount == null)
                throw new Exception("пользователь не найден");

            if (!InvokedAccount.isAdmin && InvokedAccount != account)
                throw new Exception("пользователь без прав администратора, может пополнить только свой баланс");

            account.Balance += 250000;
            await _context.SaveChangesAsync();
        }

    }
}
