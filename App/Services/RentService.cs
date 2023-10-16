using Microsoft.EntityFrameworkCore;
using Simbir.GO.App.DTO.Rent;
using Simbir.GO.Domain.Models;
using Simbir.GO.Infrastructure.Context;

namespace Simbir.GO.App.Services
{
    public class RentService
    {
        private readonly DataContext _context;
        private readonly AccountService _accountService;

        public RentService(DataContext dataContext, AccountService accountService)
        {
            _context = dataContext;
            _accountService = accountService;
        }

        public async Task<List<Transport>> GetTransports(RentTransportDto rentTransportDto)
        {
            var transports = await _context.Transports.ToListAsync();

            List<Transport> avaliableTransports = new List<Transport>();

            foreach (var transport in transports)
            {
                double x = Math.Pow(rentTransportDto.Lat - transport.Latitude, 2);
                double y = Math.Pow(rentTransportDto.Long - transport.Longitude, 2);
                double r = Math.Pow(rentTransportDto.Radius, 2);

                if (x + y <= r)
                    avaliableTransports.Add(transport);
            }

            return avaliableTransports
                .Where(x => x.TransportType == Enum.Parse<TransportType>(rentTransportDto.Type))
                .ToList();
        }

        public async Task<Rent> GetRent(Guid id, string userName)
        {
            return await GetRent(id,await _accountService.GetUserByName(userName));
        }

        public async Task<Rent> GetRent(Guid id, Account account)
        {
            if (account == null)
                throw new Exception("пользователь не найден");

            var rent = await _context.Rents.FirstOrDefaultAsync(x => x.Id == id);
            if (rent == null)
                throw new Exception("аренда не найден");

            if (!account.isAdmin &&( rent.OwnerAccountId != account.Id || rent.UserId != account.Id))
                throw new Exception("запросить может только арендатор и владелец транспорта");

            return rent;
        }

        public async Task<List<Rent>> GetHistory(Guid id)
        {
            var account = await _accountService.GetUserById(id);
            return await GetHistory(account);
        }

        public async Task<List<Rent>> GetHistory(Account account)
        {
            if (account == null)
                throw new Exception("пользователь не найден");

            var rents = await _context.Rents.Where(x => x.UserId == account.Id)
                 .OrderBy(x => x.StartTime)
                 .ToListAsync();

            return rents;
        }

        public async Task<List<Rent>> GetHistory(string userName)
        {
            var account = await _accountService.GetUserByName(userName);

            return await GetHistory(account);
        }

        public async Task<List<Rent>> GetTransportHistory(Guid transportId, string userName)
        {
            var account = await _accountService.GetUserByName(userName);
            if (account == null)
                throw new Exception("пользователь не найден");

            var rents = new List<Rent>();

            if (account.isAdmin)
                rents = _context.Rents.Where(x => x.Id == transportId).OrderBy(x => x.StartTime).ToList();
            else
                rents = _context.Rents.Where(x => x.OwnerAccountId == account.Id && x.TransportId == transportId)
                 .OrderBy(x => x.StartTime)
                 .ToList();

            return rents;
        }

        public async Task<Rent> AdminCreateRent(AdminRentDto adminRent)
        {
            var account = await _accountService.GetUserById(adminRent.UserId);
            if (account == null)
                throw new Exception("пользователь не найден");

            var transport = await _context.Transports.FirstOrDefaultAsync(x => x.Id == adminRent.TransportId);
            if (transport == null)
                throw new Exception("транспорт не найден");

            if (!transport.CanBeRented)
                throw new Exception("транспорт уже в аренде");

            var rent = new Rent()
            {
                TransportId = adminRent.TransportId,
                StartTime = adminRent.TimeStart,
                EndTime = adminRent.TimeEnd,
                UserId = adminRent.UserId,
                OwnerAccountId = transport.AccountId,
                Type = Enum.Parse<RentType>(adminRent.PriceType),
                PriceOfUnit = adminRent.PriceOfUnit,
                FinalPrice = adminRent.FinalPrice,
            };

            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            return rent;

        }

        public async Task<Rent> CreateRent(Guid transportId, string userName, string inputRentType)
        {
            var account = await _accountService.GetUserByName(userName);
            if (account == null)
                throw new Exception("пользователь не найден");

            var transport = await _context.Transports.FirstOrDefaultAsync(x => x.Id == transportId);
            if(transport == null)
                throw new Exception("транспорт не найден");

            if(transport.AccountId == account.Id)
                throw new Exception("нельзя брать в аренду собственный транспорт");

            if (!transport.CanBeRented)
                throw new Exception("транспорт уже в аренде");

            var rent = new Rent()
            {
                TransportId = transportId,
                StartTime = DateTime.UtcNow,
                UserId = account.Id,
                OwnerAccountId = transport.AccountId,
                Type = Enum.Parse<RentType>(inputRentType)
            };

            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            return rent;

        }

        public async Task EndRent(Guid rentId, string userName, RentDto rentDto)
        {
            var account = await _accountService.GetUserByName(userName);
            if (account == null)
                throw new Exception("пользователь не найден");

            var rent = await _context.Rents.FirstOrDefaultAsync(x => x.Id == rentId);
            if (rent == null)
                throw new Exception("аренда не найдена");

            var transport = await _context.Transports.FirstOrDefaultAsync(x => x.Id == rent.TransportId);
            if (transport == null)
                throw new Exception("транспорт не найден");

            if (rent.OwnerAccountId == account.Id)
                throw new Exception("закрыть аренду может только тот, кто её начал");

            rent.EndTime = DateTime.UtcNow;
            transport.CanBeRented = true;
            transport.Latitude = rentDto.Lat;
            transport.Longitude = rentDto.Long;

            await _context.SaveChangesAsync();

        }

        public async Task UpdateRent(Guid id, AdminRentDto adminRentDto)
        {
            var rent =await _context.Rents.FirstOrDefaultAsync(x=>x.Id == id);
            if (rent == null)
                throw new Exception("аренда не найдена");

            rent.StartTime = adminRentDto.TimeStart;
            rent.FinalPrice = adminRentDto.FinalPrice;
            rent.TransportId = adminRentDto.TransportId;
            rent.UserId = adminRentDto.UserId;
            rent.EndTime = adminRentDto.TimeEnd;
            rent.PriceOfUnit = adminRentDto.PriceOfUnit;
            rent.Type = Enum.Parse<RentType>(adminRentDto.PriceType);

            await _context.SaveChangesAsync();

        }

        public async Task DeleteRent(Guid id)
        {
            var rent = await _context.Rents.FirstOrDefaultAsync(x => x.Id == id);
            if (rent == null)
                throw new Exception("аренда не найдена");

            _context.Rents.Remove(rent);

            await _context.SaveChangesAsync();
        }
    }
}
