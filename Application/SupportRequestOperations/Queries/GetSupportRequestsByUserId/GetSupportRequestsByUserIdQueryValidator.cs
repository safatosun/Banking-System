using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportRequestOperations.Queries.GetSupportRequestsByUserId
{
    public class GetSupportRequestsByUserIdQueryValidator : AbstractValidator<GetSupportRequestsByUserIdQuery>
    {
        public GetSupportRequestsByUserIdQueryValidator()
        {
            RuleFor(s=>s.UserId).NotEmpty();
        }
    }
}
