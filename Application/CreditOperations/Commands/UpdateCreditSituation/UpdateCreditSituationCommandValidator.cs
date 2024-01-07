using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Commands.UpdateCreditSituation
{
    public class UpdateCreditSituationCommandValidator : AbstractValidator<UpdateCreditSituationCommand>
    {
        public UpdateCreditSituationCommandValidator()
        {
            RuleFor(c => c.CreditId).GreaterThan(0);        
        }
    }
}
