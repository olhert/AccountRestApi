using System;
using AccountRestApi.Controllers;
using AccountRestApi.DB;
using FluentValidation;

namespace AuthenticationProject
{
    public class UserValidator : AbstractValidator<RegisterRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class AccValidator : AbstractValidator<AccountRequest>
    {
        public AccValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty();
        }
    }
}