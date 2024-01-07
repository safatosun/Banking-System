using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.ValidateUser
{
    public class ValidateUserCommand
    {
        public  UserAuthenticationDto ModelDto { get; set; }  

        private readonly IBankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ValidateUserCommand(IBankingSystemDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<User> Handle()
        {
            var user = await _userManager.FindByEmailAsync(ModelDto.Email);

            var result = (user != null && await _userManager.CheckPasswordAsync(user, ModelDto.Password)); // Kullanıcı ve şifre kontrolü yapıldı

            if (!result)
            {
                //_logger.LogWarning($"{nameof(ValidateUser)} : Authentication failed. Wrong username or password.");
                throw new Exception("Authentication failed. Wrong username or password.");
            }
            return user;
        }


    }

    public record UserAuthenticationDto
    {
        public String Email { get; init; }
        public String Password { get; init; }
    }


}
