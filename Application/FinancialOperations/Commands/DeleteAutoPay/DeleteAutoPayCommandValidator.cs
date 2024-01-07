using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.DeleteAutoPay
{
    public class DeleteAutoPayCommandValidator : AbstractValidator<DeleteAutoPayCommand>
    {
        public DeleteAutoPayCommandValidator()
        {
            RuleFor(a=>a.InvoiceId).GreaterThan(0);
        }

    }
}
