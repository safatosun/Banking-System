using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Jobs
{
    public class CheckCreditPaymentsJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _loggerService;

        public CheckCreditPaymentsJob(IUnitOfWork unitOfWork, ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
        }

        public async Task CheckCreditPayments()
        {
            var credits = await _unitOfWork.Credit.GetAllWithAllDetailsAsync();

            foreach (var credit in credits)
            {
                if (credit.Approved ?? false || credit.InstallmentDueDay >= DateTime.Now.Day)
                {
                    continue;
                }

                Account highBalanceAccount = credit.User.Accounts.FirstOrDefault(account => account.Balance > 3000);

                if (highBalanceAccount is null)
                    continue;

                highBalanceAccount.Balance -= credit.InstallmentAmount;
                credit.PaidInstallmentCount += 1;
                
                if (credit.Maturity == credit.PaidInstallmentCount)
                { 
                    credit.Paid = true;
                    _loggerService.LogWarning($"The user with ID number {credit.UserId} has fully paid off the credit with id number {credit.Id}");
                    continue;
                }
            }

            await _unitOfWork.SaveChangesAsync();

            _loggerService.LogWarning($"Credit payments are checked.");
        }

    }


}
