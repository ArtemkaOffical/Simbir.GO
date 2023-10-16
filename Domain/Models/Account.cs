using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Simbir.GO.Domain.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [IgnoreDataMember]
        public string Password { get; set; }

        public bool isAdmin { get; set; } = false;
        public double Balance { get; set; } = 0;
        public virtual IEnumerable<Transport> Transports { get; set; }

        public Account()
        {
            Transports = new List<Transport>();
        }

    }
}
