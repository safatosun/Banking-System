using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.Deposit
{
    public class DepositCommand
    {
        public string UserId { get; set; }
        public DepositDto ModelDto { get; set; }

        private readonly IBankingSystemDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _loggerService;

        public DepositCommand(IBankingSystemDbContext dbContext, IUnitOfWork unitOfWork, ILoggerService loggerService)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
        }

        public async Task Handle()
        {
            await _unitOfWork.BeginTransactionAsyncWithPessimisticLocking();

            var usersAccount = await _unitOfWork.Account.GetByUserIdAndAccountNameWithUserDetailsAsync(UserId, ModelDto.AccountName, true);

            if (usersAccount is null)
                throw new InvalidOperationException("The user's account could not find.");
            
            usersAccount.Balance += ModelDto.Amount;

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.TransactionCommitAsync();

            _loggerService.LogInfo($"The user with ID number {UserId} deposit {ModelDto.Amount} USD {usersAccount.Name} account on {DateTime.Now}.");
        }

    }

    public record DepositDto
    {
        public string  AccountName { get; init; }
        public decimal Amount { get; init; }
    }

}
