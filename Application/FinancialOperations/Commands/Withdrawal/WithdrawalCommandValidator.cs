using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.Withdrawal
{
    public class WithdrawalCommandValidator : AbstractValidator<WithdrawalDto>
    {
        public WithdrawalCommandValidator()
        {
            RuleFor(w=>w.AccountName).NotEmpty();
            RuleFor(w=>w.Amount).NotNull().GreaterThan(0);
        }

    }
}
