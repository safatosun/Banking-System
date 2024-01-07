using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Commands.CreateCredit
{
    public class CreateCreditDtoValidator : AbstractValidator<CreateCreditDto>
    {
        public CreateCreditDtoValidator() 
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.RequestedAmount).GreaterThan(0);
            RuleFor(c => c.Maturity).GreaterThan(0);
        }
    }
}
