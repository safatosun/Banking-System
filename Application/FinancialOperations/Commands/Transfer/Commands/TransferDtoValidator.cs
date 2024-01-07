using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.Transfer.Commands
{
    public class TransferDtoValidator : AbstractValidator<TransferDto>
    {
        public TransferDtoValidator() 
        {
            RuleFor(t=>t.SenderAccountId).GreaterThan(0);
            RuleFor(t => t.ReceiverAccountId).GreaterThan(0);
            RuleFor(t => t.Amount).GreaterThan(0);
        }

    }
}
