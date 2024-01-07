using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.ValidateUser
{
    public class UserAuthenticationDtoValidator : AbstractValidator<UserAuthenticationDto>
    {
        public UserAuthenticationDtoValidator() 
        {
            RuleFor(a => a.Email).NotEmpty();
            RuleFor(a => a.Password).NotEmpty();
        }
    }
}
