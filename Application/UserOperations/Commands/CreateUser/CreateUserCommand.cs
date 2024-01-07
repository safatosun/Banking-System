using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.CreateUser
{
    public class CreateUserCommand
    {

        public UserRegistrationDto ModelDto { get;set;}  

        private readonly IBankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CreateUserCommand(IBankingSystemDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle()
        {
            var user = _mapper.Map<User>(ModelDto);
                                                                
            var result = await _userManager.CreateAsync(user, ModelDto.Password); 

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, ModelDto.Roles);
            }
            return result;
        }


    }


    public record UserRegistrationDto
    {
        public String FirstName { get; init; }

        public String LastName { get; init; }

        public String UserName { get; init; }

        public String Password { get; init; }

        public String Email { get; init; }

        public String PhoneNumber { get; init; }
        
        public ICollection<String> Roles { get; init; }

    }

}
