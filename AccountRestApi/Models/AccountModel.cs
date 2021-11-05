using System.Reflection;

namespace AccountRestApi.Controllers
{
    public interface IAccount
        {
            string Id { get; set; }
            string Name { get; set; }
            double Balance { get; set; }
            string Currency { get; set; }
            string UserId { get; set; }
        }
        
        public class Account : IAccount
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public double Balance { get; set; }
            public string Currency { get; set; }
            public string UserId { get; set; }

            public static Account Create(IAccount src, IUser user)
            {
                return new Account
                {
                    Id = src.Id,
                    Name = src.Name,
                    Balance = src.Balance,
                    Currency = src.Currency,
                    UserId = user.Id
                };
            }
        }

        public class AccountRequest
        {
            public string Name { get; set; }
            public string Currency { get; set; }
        }
        
    
}