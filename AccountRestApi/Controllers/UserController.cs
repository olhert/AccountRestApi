using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AccountRestApi.Controllers;
using AccountRestApi.DB;
using AuthenticationProject;
using Dapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AccountRestApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserStore _userStore;
        
        public UserController(IUserStore userStore)
        {
            _userStore = userStore;
        }
        
        [HttpPost("/registration")]
        public IActionResult Registration(RegisterRequest registerRequest)
        {
            var validator = new UserValidator();
            ValidationResult result = validator.Validate(registerRequest);
            if (result.IsValid == false)
            {
                return Ok(new StatusModel {Errors = result.Errors.Select(failure => failure.ErrorMessage).ToList()});
            }
            else
            {
                var user = new User
                {
                    Password = sha256_hash(registerRequest.Password),
                    Email = registerRequest.Email,
                    Id = Guid.NewGuid().ToString()
                };
                _userStore.Registration(user);
                return Ok(new StatusModel
                {
                    Status = "success"
                });
            }
            
        }
        
        [HttpPost("/authentication")]
        public IActionResult  Authentication(AuthRequest authRequest)
        {
            var user = _userStore.GetUserByEmail(authRequest.Email);
            if (user == null)
                return Ok(new StatusModel {Status = "user is not found"});
            
            var pass = sha256_hash(authRequest.Password);
            
            if (pass == user.Password)
            {
                var authUser = new AuthToken {UserId = user.Id, TokenExpiredDate = DateTime.UtcNow.AddHours(1)};
                var key = "ARAPn1FJlgqe2DIM0lOFxUBj";
                var userAsJson = authUser.GetJson();
                
                var encodedBytes = EncodeDecode.AesEncodeDecode.Encode(Encoding.ASCII.GetBytes(userAsJson), Encoding.ASCII.GetBytes(key));
                var token = Convert.ToBase64String(encodedBytes);

                return Ok(new StatusModel {Status = $"user is authenticated. user's token: {token}  "});
            }
            else
            {
                return Ok(new StatusModel {Status = "password is wrong"});
            }
            
        }
        
        
        
        private static String sha256_hash(String value) {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create()) {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}