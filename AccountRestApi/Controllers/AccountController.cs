using System;
using System.Collections.Generic;
using System.Linq;
using AuthenticationProject;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AccountRestApi.Controllers
{
    [ApiController]
    [Route("apisecure/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountsStore _accountsStore;

        public AccountController(IAccountsStore accountsStore)
        {
            _accountsStore = accountsStore;
        }

        [HttpGet("/accounts")]
        public IActionResult GetAllUserAccounts()
        {
            var userId = Extensions.ParseAuthToken(HttpContext.Request.Headers["Authorization"]);
            return Ok(_accountsStore.GetAllUserAccounts(userId));
        }
        
        [HttpPost("/accounts/add")]
        public IActionResult RegisterAcc(AccountRequest accountRequest)
        {
            var validator = new AccValidator();
            var result = validator.Validate(accountRequest);

            if (result.IsValid == false)
            {
                return Ok(new StatusModel {Errors = result.Errors.Select(failure => failure.ErrorMessage).ToList()});
            }
            else
            {
                var account = new Account
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = Extensions.ParseAuthToken(HttpContext.Request.Headers["Authorization"]),
                    Name = accountRequest.Name,
                    Currency = accountRequest.Currency,
                    Balance = 0

                };
                _accountsStore.RegisterAcc(account);
                return Ok(new StatusModel
                {
                    Status = "success"
                });
            }
        }

        [HttpGet("/accounts/getById/{idOfAccount}/")]
        public IActionResult GetAccById(string idOfAccount)
        {
            return Ok (_accountsStore.GetAccById(idOfAccount));
        }
        
        [HttpGet("/accounts/getByName/{nameOfAccount}/")]
        public IActionResult GetAccByName(string nameOfAccount)
        {
            return Ok (_accountsStore.GetAccByName(nameOfAccount));
        }
        
    }
}