using System;
using Newtonsoft.Json;

namespace AccountRestApi.Controllers
{
    public interface IUser
    { 
        string Id { get; set; }
        string Email { get; set; } 
        string Password { get; set; }
    }

    public class User: IUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class AuthRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthToken
    {
        public string UserId { get; set; }
        public DateTime TokenExpiredDate { get; set; }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static AuthToken FromJson(string jsonObject)
        {
            return JsonConvert.DeserializeObject<AuthToken>(jsonObject);
        }
    }

}