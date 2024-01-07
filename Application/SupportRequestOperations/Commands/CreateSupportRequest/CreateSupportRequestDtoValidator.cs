using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportRequestOperations.Commands.CreateSupportRequest
{
    public class CreateSupportRequestDtoValidator : AbstractValidator<CreateSupportRequestDto>
    {
        public CreateSupportRequestDtoValidator() 
        {
            RuleFor(s=>s.UserId).NotEmpty();
            RuleFor(s=>s.Text).NotEmpty();
        }
    }
}
