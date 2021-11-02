using AccountRestApi.DB;

namespace AccountRestApi.Controllers
{
    public interface IUserStore
    {
        void Registration(IUser user);
        DbUser GetUserByEmail(string email);
    }
}