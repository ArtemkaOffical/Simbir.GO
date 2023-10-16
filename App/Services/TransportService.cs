using Microsoft.EntityFrameworkCore;
using Simbir.GO.App.DTO.Transport;
using Simbir.GO.App.DTO.TransportDto;
using Simbir.GO.Domain.Models;
using Simbir.GO.Infrastructure.Context;

namespace Simbir.GO.App.Services
{
    public class TransportService
    {
        private readonly DataContext _context;
        private readonly AccountService _accountService;

        public TransportService(DataContext dataContext, AccountService accountService)
        {
            _context = dataContext;
            _accountService = accountService;
        }

        public async Task<Transport> RegisterTransport(TransportDto transportDto, Guid id)
        {
            var transport = await GetTransportByIdentifier(transportDto.Identifier);
            if (transport != null)
                throw new Exception("Такой транспорт уже существует(идентификатор)");

            var account = await _accountService.GetUserById(id);
            if (account == null)
                throw new Exception("пользователь не найден");

            var newTransport = CreateModelByDto(transportDto);
            newTransport.Account = account;
            newTransport.AccountId = account.Id;
            _context.Transports.Add(newTransport);

            await _context.SaveChangesAsync();

            return newTransport;
        }

        public async Task UpdateTransport(TransportUpdateDto transportDto, Guid id, string userName)
        {
            Transport transport = await GetTransport(id);
            if (transport == null)
                throw new Exception("Такого транспорта не существует");

            var account = await _accountService.GetUserByName(userName);
            if (account == null)
                throw new Exception("пользователь не найден");

            if (transport.AccountId != account.Id)
                throw new Exception("Вы не являетесь владельцем");

            transport.Longitude = transportDto.Longitude;
            transport.Latitude = transportDto.Latitude;
            transport.Description = transportDto.Description;
            transport.Identifier = transportDto.Identifier;
            transport.CanBeRented = transportDto.CanBeRented;
            transport.Color = transportDto.Color;
            transport.Model = transportDto.Model;
            transport.MinutePrice = transportDto.MinutePrice;
            transport.DayPrice = transportDto.DayPrice;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransport(AdminTransportDto transportDto, Guid id)
        {
            Transport transport = await GetTransport(id);
            if (transport == null)
                throw new Exception("Такого транспорта не существует");

            var account = await _accountService.GetUserById(transportDto.OwnerId);
            if (account == null)
                throw new Exception("пользователь не найден");


            transport.Longitude = transportDto.Longitude;
            transport.Latitude = transportDto.Latitude;
            transport.Description = transportDto.Description;
            transport.Identifier = transportDto.Identifier;
            transport.CanBeRented = transportDto.CanBeRented;
            transport.Color = transportDto.Color;
            transport.Model = transportDto.Model;
            transport.MinutePrice = transportDto.MinutePrice;
            transport.DayPrice = transportDto.DayPrice;
            transport.AccountId = account.Id;
            transport.Account = account;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransport(Guid id, string userName)
        {
            Transport transport = await GetTransport(id);
            if (transport == null)
                throw new Exception("Такого транспорта не существует");

            var account = await _accountService.GetUserByName(userName);
            if (account == null)
                throw new Exception("пользователь не найден");

            if (!account.isAdmin && transport.AccountId != account.Id)
                throw new Exception("Вы не являетесь владельцем");

            _context.Transports.Remove(transport);
            await _context.SaveChangesAsync();
        }

        public async Task<Transport> GetInfo(Guid id)
        {
            Transport transport = await GetTransport(id);
            if (transport == null)
                throw new Exception("транспорт не найден");

            return transport;
        }

        public async Task<List<Transport>> GetTransports(int start, int count, string transportType)
        {
            var list = await _context.Transports.ToListAsync();

            if (transportType == "All")
                return list;

            return list.Skip(start).Take(count)
                .Where(x => x.TransportType == Enum.Parse<TransportType>(transportType))
                .ToList();

        }

        private async Task<Transport> GetTransport(Guid id)
        {
            return await _context.Transports.FirstOrDefaultAsync(x => x.Id == id);
        }

        private Transport CreateModelByDto<T>(T dto) where T : TransportDto
        {
            return dto.GetModel();
        }

        private async Task<Transport> GetTransportByIdentifier(string identifier)
        {
            return await _context.Transports.FirstOrDefaultAsync(x => x.Identifier == identifier);
        }
    }
}
