using System.ComponentModel.DataAnnotations;

namespace Simbir.GO.Domain.Models
{
    public enum RentType
    {
       Minutes,Days
    }

    public class Rent
    {
        public Guid Id { get; set; }
        public Guid OwnerAccountId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? FinalPrice { get; set; }
        public double PriceOfUnit { get; set; } = 1;

        [Required]
        public RentType Type {  get; set; }
        [Required]
        public Guid TransportId { get; set; }
    }
}
