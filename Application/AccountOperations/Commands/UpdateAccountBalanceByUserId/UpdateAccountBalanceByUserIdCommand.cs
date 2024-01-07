using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AccountOperations.Commands.UpdateAccountBalance
{
    public class UpdateAccountBalanceByUserIdCommand
    {
        public string UserId { get; set; }
        public UpdateAccountBalanceDto ModelDto { get; set; }   

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAccountBalanceByUserIdCommand(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle()
        {
            var account = await _unitOfWork.Account.GetByUserIdAndAccountNameWithUserDetailsAsync(UserId, ModelDto.AccountName, true);

            if (account is null)
                throw new InvalidOperationException("The Users account could not find.");

           
            account.Balance = ModelDto.Balance;           

            await _unitOfWork.SaveChangesAsync();

        }
    }


    public record UpdateAccountBalanceDto
    {  
        public string AccountName { get; init; }
        public decimal Balance { get; init; }
    }

}
