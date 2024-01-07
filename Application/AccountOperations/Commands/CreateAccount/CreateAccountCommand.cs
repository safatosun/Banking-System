using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountOperations.Commands.CreateAccount
{
    public class CreateAccountCommand
    {
        public CreateAccountDto ModelDto { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper { get; set; }
        private UserManager<User> _userManager { get; set; }

        public CreateAccountCommand(IMapper mapper, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle()
        {
            var user = await _userManager.FindByIdAsync(ModelDto.UserId);

            if (user is null)
                throw  new InvalidOperationException("The user could not find.");

            var account = _mapper.Map<Account>(ModelDto);

            _unitOfWork.Account.CreateOne(account);

             await _unitOfWork.SaveChangesAsync();

        }

    }

    public record CreateAccountDto
    {
        public string UserId { get; init; }
        public string Name { get; set; }
        public decimal Balance { get; init; }
    }

}
