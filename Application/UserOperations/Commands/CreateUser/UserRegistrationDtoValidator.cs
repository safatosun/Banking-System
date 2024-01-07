using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.CreateUser
{
    public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
    {
        public UserRegistrationDtoValidator() 
        {
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();
            RuleFor(u => u.UserName).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.PhoneNumber).NotEmpty();
        }
    }
}
