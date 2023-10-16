namespace Simbir.GO.Domain.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string JWTToken { get; set; }

        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }

    }
}
