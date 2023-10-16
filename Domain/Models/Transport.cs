using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Simbir.GO.Domain.Models
{
    public enum TransportType
    {
         Car, Bike, Scooter, All
    }

    public class Transport
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public bool CanBeRented { get; set; } = true;

        [Required]
        public TransportType TransportType { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Identifier { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [IgnoreDataMember]
        public virtual Account Account { get; set; }

        public string? Description { get; set; }
        public double? MinutePrice { get; set; } 
        public double? DayPrice { get; set; }
    }
}
