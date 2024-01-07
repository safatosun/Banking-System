using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountOperations.Commands.CreateAccount
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>    
    {
        public CreateAccountDtoValidator() 
        {
            RuleFor(a => a.UserId).NotEmpty()
                                  .NotNull();
            RuleFor(b => b.Name).NotEmpty();
            
            RuleFor(a => a.Balance).NotEmpty()
                                   .NotNull()
                                   .GreaterThan(50).WithMessage("For newly opened accounts, the minimum balance is 50$.");
        }
    }
}
