using Simbir.GO.Domain.Models;

namespace Simbir.GO.App.DTO.AccountDto
{
    public class AccountDto : IModelDto<Domain.Models.Account>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual Account GetModel()
        {
            return new Account
            {
                UserName = UserName,
                Password = Password,
            };
        }
    }
}
