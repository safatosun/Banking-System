using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Queries.GetAutoPays
{
    public class GetAutoPaysQueryValidator : AbstractValidator<GetAutoPaysQuery>
    {
        public GetAutoPaysQueryValidator()
        {
            RuleFor(a=>a.AccountId).GreaterThan(0);
        }
    }
}
