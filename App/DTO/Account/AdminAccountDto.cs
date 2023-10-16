using Simbir.GO.Domain.Models;

namespace Simbir.GO.App.DTO.AccountDto
{
    public class AdminAccountDto : AccountDto
    {
        public bool isAdmin { get; set; }
        public double Balance { get; set; }

        public override Account GetModel()
        {
            return new Account
            {
                UserName = UserName,
                Password = Password,
                isAdmin = isAdmin,
                Balance = Balance
            };
        }
    }
}
