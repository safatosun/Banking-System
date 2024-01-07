using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Queries.GetAutoPayById
{
    public class GetAutoPayByIdQueryValidator : AbstractValidator<GetAutoPayByIdQuery>
    {
        public GetAutoPayByIdQueryValidator()
        {
            RuleFor(a=>a.InvoiceId).GreaterThan(0);
        }

    }
}
