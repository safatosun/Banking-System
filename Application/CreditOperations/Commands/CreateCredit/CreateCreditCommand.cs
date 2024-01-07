using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Commands.CreateCredit
{
    public class CreateCreditCommand
    {
        public CreateCreditDto ModelDto { get; set; }
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CreateCreditCommand(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task Handle()
        {
            var user = await _userManager.FindByIdAsync(ModelDto.UserId);
            
            if (user is null)
                throw new InvalidOperationException("The users credit could not find.");

            var credit = _mapper.Map<Credit>(ModelDto);

            _unitOfWork.Credit.CreateOne(credit);

            await _unitOfWork.SaveChangesAsync();

        }

    }

    public record CreateCreditDto
    {
        public string Name { get; init; }
        public string UserId { get; init; }
        public int RequestedAmount { get; init; } 
        public int Maturity { get; init; } 
    }

}
