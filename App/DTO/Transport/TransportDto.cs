using Simbir.GO.Domain.Models;

namespace Simbir.GO.App.DTO.TransportDto
{
    public class TransportDto : IModelDto<Domain.Models.Transport>
    {
        public bool CanBeRented { get; set; } = true;
        public string TransportType { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Identifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Description { get; set; }
        public double? MinutePrice { get; set; }
        public double? DayPrice { get; set; }

        public virtual Domain.Models.Transport GetModel()
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

            };
        }
    }
}
