using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.CreateAutoPay
{
    public class CreateAutoPayDtoValidator : AbstractValidator<CreateAutoPayDto>    
    {

        public CreateAutoPayDtoValidator() 
        {
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.CompanyName).NotEmpty();
            RuleFor(a => a.AccountId).GreaterThan(0);
            RuleFor(a => a.TotalAmount).GreaterThan(0);
        }
    }
}
