namespace Simbir.GO.App.DTO.Rent
{
    public class AdminRentDto
    {
        public Guid TransportId { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public double PriceOfUnit { get; set; }
        public string PriceType { get; set; }
        public double? FinalPrice { get; set; }
    }
}
