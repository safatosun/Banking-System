using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.UpdateUserRoles
{
    public class UpdateUserRolesDtoValidator : AbstractValidator<UpdateUserRolesDto>
    {
        public UpdateUserRolesDtoValidator() 
        {
            RuleFor(u=>u.Email).NotEmpty();
        }
    }
}
