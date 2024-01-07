using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.UpdateUserRoles
{
    public class UpdateUserRolesCommand
    {
        public UpdateUserRolesDto RolesDto { get; set; }    

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UpdateUserRolesCommand(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task Handle()
        {
            var user = await _userManager.FindByEmailAsync(RolesDto.Email);

            if (user is null)
                throw new InvalidOperationException("The user could not find.");

            var userRoles = await _userManager.GetRolesAsync(user);
            
            if(userRoles is not null)
                await _userManager.RemoveFromRolesAsync(user,userRoles);

            await _userManager.AddToRolesAsync(user, RolesDto.Roles);

        }

    }

    public record UpdateUserRolesDto
    {
        public string Email { get; init; }
        public ICollection<String> Roles { get; set; }
    }

}
