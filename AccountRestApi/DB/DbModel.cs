using AccountRestApi.Controllers;

namespace AccountRestApi.DB
{
    public class DbAccount : IAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }
        public string UserId { get; set; }

        public static DbAccount Create(IAccount src, IUser user)
        {
            return new DbAccount
            {
                Id = src.Id,
                Name = src.Name,
                Balance = src.Balance,
                Currency = src.Currency,
                UserId = user.Id
            };
        }
    }

    public class DbUser : IUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}