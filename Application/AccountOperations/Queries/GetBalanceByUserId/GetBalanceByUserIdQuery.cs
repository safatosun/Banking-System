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

namespace Application.AccountOperations.Queries.GetBalanceByUserId
{
    public  class GetBalanceByUserIdQuery
    {
        public  string UserId { get; set; }
        public  string AccountName { get; set; }    

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBalanceByUserIdQuery(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountBalanceViewModel> Handle()
        {
            var account = await _unitOfWork.Account.GetByUserIdAndAccountNameWithUserDetailsAsync(UserId,AccountName,true);

            if (account is null)
                throw new InvalidOperationException("The Users account could not find.");

            var viewModel = new AccountBalanceViewModel
            {
                Balance = account.Balance,
            };

            return viewModel;
        }

    }


    public class AccountBalanceViewModel
    {
        public decimal Balance { get; set; }
    }

}
