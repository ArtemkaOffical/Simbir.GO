using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simbir.GO.Domain.Models;
using Simbir.GO.Infrastructure;
using Simbir.GO.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simbir.GO.App.Services
{
    public class JWTService
    {
        private readonly DataContext _context;
        private readonly JwtOpt _jwt;

        public JWTService(DataContext dataContext, IOptions<JwtOpt> jwt)
        {
            _context = dataContext;
            _jwt = jwt.Value;
        }

        public async Task<string> CreateToken(Account user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwt.Secret));

            var claims = new ClaimsIdentity(new List<Claim>
            {
                new Claim("UserName", user.UserName),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.isAdmin?"Admin":"None")
            });

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwt.Issuer,
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(_jwt.ExpiresHours),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            var generatedToken = tokenHandler.WriteToken(token);

            var existedToken = await _context.Tokens.FirstOrDefaultAsync(x => x.AccountId == user.Id);
            if (existedToken == null)
                _context.Tokens.Add(new Token
                {
                    Account = user,
                    AccountId = user.Id,
                    JWTToken = generatedToken,

                });
            else
                existedToken.JWTToken = generatedToken;

            await _context.SaveChangesAsync();

            return generatedToken;
        }

        public async Task<bool> TokenIsValid(string token)
        {
            var existedToken = await _context.Tokens.FirstOrDefaultAsync(x => x.JWTToken == token);

            if (existedToken != null)
                return true;

            return false;
        }

        public async Task ExpireToken(string token)
        {
            var existedToken = await _context.Tokens.FirstOrDefaultAsync(x => x.JWTToken == token);

            if (existedToken == null)
                throw new Exception("token not found");

            existedToken.JWTToken = "";

            await _context.SaveChangesAsync();
        }

    }
}
