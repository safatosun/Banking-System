using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountOperations.Queries.GetBalanceByUserId
{
    public class GetBalanceByUserIdQueryValidator : AbstractValidator<GetBalanceByUserIdQuery>
    {
        public GetBalanceByUserIdQueryValidator()
        {
            RuleFor(a => a.UserId).NotEmpty();
            RuleFor(a => a.AccountName).NotEmpty();
        }
    }
}
