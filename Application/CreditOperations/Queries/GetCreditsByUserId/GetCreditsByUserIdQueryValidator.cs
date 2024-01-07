using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Queries.GetCreditsByUserId
{
    public class GetCreditsByUserIdQueryValidator : AbstractValidator<GetCreditsByUserIdQuery>
    {
        public GetCreditsByUserIdQueryValidator() 
        {
            RuleFor(c=>c.UserId).NotEmpty();
        }
    }
}
