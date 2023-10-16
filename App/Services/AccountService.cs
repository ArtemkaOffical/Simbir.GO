using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simbir.GO.App.DTO.AccountDto;
using Simbir.GO.Domain.Models;
using Simbir.GO.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simbir.GO.App.Services
{
    public class AccountService
    {
        private readonly DataContext _context;
        private readonly JWTService _jwt;

        public AccountService(DataContext dataContext, JWTService jWTService) 
        { 
            _context = dataContext;
            _jwt = jWTService;
        }

        public async Task<List<Account>> GetAccounts(int start, int count)
        {
            return await _context.Accounts.Skip(start).Take(count).ToListAsync();
        }

        public async Task SignOut(string token)
        {
            await _jwt.ExpireToken(token);
        }

        public async Task<string> SignIn(AccountDto accountDto)
        {
            Account user = await GetUserByName(accountDto.UserName);
            if (user == null)
                throw new Exception("пользователь не найден");

            if (accountDto.Password != DecryptPassword(user))
                throw new Exception("неверные данные для входа");
            
            return await _jwt.CreateToken(user);

        }

        public async Task<Account> GetUserByName(string name)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<Account> GetUserById(Guid id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> isAdmin(string name)
        {
            var user = await GetUserByName(name);

            if (user != null && user.isAdmin)
                return true;

            return false;
        }

        public async Task RegisterAccount(AccountDto accountDto)
        {
            Account user = await GetUserByName(accountDto.UserName);
            if (user != null)
                throw new Exception("Такое имя уже занято");

            var account = CreateModelByDto(accountDto);
            EncryptPassword(account);
            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccount(AccountDto accountDto, Guid id)
        {
            Account user = await GetUserById(id);
            if (user == null)
                throw new Exception("пользователь не найден");

            var user2 = await GetUserByName(accountDto.UserName);
            if (user2 != null)
                throw new Exception("Такое имя уже занято");

            user = CreateModelByDto(accountDto);
            user.Password = EncryptPassword(user);

            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetInfo(string userName)
        {
            var user = await GetUserByName(userName);
            if (user == null)
                throw new Exception("пользователь не найден");

            return user;
        }

        public async Task DeleteAccount(Guid id)
        {
            _context.Accounts.Remove(await GetUserById(id));
            await _context.SaveChangesAsync();
        }

        

        private string EncryptPassword(Account model)
        {
            byte[] storedPassword = ASCIIEncoding.ASCII.GetBytes(model.Password);
            model.Password = Convert.ToBase64String(storedPassword);
            return model.Password;
        }

        private Account CreateModelByDto<T>(T dto) where T : AccountDto
        {
            return dto.GetModel();
        }

        private string DecryptPassword(Account model)
        {
            byte[] storedPassword = Convert.FromBase64String(model.Password);
            return ASCIIEncoding.ASCII.GetString(storedPassword);
        }
    }
}
