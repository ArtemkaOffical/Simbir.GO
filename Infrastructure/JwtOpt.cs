namespace Simbir.GO.Infrastructure
{
    public class JwtOpt
    {
        public string Issuer { get; set; }

        public string Secret { get; set; }

        public int ExpiresHours { get; set; }
    }
}