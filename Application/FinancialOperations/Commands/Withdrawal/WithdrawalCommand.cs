using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.Withdrawal
{
    public class WithdrawalCommand
    {
        public string UserId { get; set; }
        public WithdrawalDto ModelDto { get; set; }

        private readonly IBankingSystemDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _loggerService;

        public WithdrawalCommand(IBankingSystemDbContext dbContext, IUnitOfWork unitOfWork, ILoggerService loggerService)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
        }

        public async Task Handle()
        {
            await _unitOfWork.BeginTransactionAsyncWithPessimisticLocking();

            var usersAccount = await _dbContext.Accounts.Where(a => a.UserId == UserId && a.Name == ModelDto.AccountName).SingleOrDefaultAsync();

            if (usersAccount is null)
                throw new InvalidOperationException("The user's account could not find.");

            if (ModelDto.Amount > usersAccount.Balance)
                throw new InvalidOperationException("You cannot withdraw more money than your balance.");

            usersAccount.Balance -= ModelDto.Amount;

            await _dbContext.SaveChangesAsync();

            await _unitOfWork.TransactionCommitAsync();
            
            _loggerService.LogInfo($"The user with ID number {UserId} withdraw {ModelDto.Amount} USD from {usersAccount.Name} account on {DateTime.Now}.");
        }

    }

    public record WithdrawalDto
    {
        public string AccountName { get; init; }
        public decimal Amount { get; init; }
    }



}
