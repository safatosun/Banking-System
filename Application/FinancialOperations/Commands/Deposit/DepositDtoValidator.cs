using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.Deposit
{
    public class DepositDtoValidator : AbstractValidator<DepositDto>    
    {
         public DepositDtoValidator()
         {
        
            RuleFor(d=>d.AccountName).NotEmpty();
            RuleFor(d=>d.Amount).NotNull().GreaterThan(0);
         }
    }
}
