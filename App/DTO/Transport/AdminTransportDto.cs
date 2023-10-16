using Simbir.GO.Domain.Models;

namespace Simbir.GO.App.DTO.TransportDto
{
    public class AdminTransportDto : TransportDto
    {
        public Guid OwnerId { get; set; }

        public override Domain.Models.Transport GetModel()
        {
            return new Domain.Models.Transport
            {
                Identifier = Identifier,
                DayPrice = DayPrice,
                Description = Description,
                CanBeRented = CanBeRented,
                Color = Color,
                Latitude = Latitude,
                Longitude = Longitude,
                MinutePrice = MinutePrice,
                Model = Model,
                TransportType = Enum.Parse<TransportType>(TransportType),
                AccountId = OwnerId

            };
        }
    }
}
