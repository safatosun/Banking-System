using Application.AccountOperations.Commands.UpdateAccountBalance;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountOperations.Commands.UpdateAccountBalanceByUserId
{
    public class UpdateAccountBalanceDtoValidator : AbstractValidator<UpdateAccountBalanceDto>
    {

        public UpdateAccountBalanceDtoValidator() 
        {

        RuleFor(a=>a.Balance).GreaterThanOrEqualTo(0);
   
        }
    }
}
