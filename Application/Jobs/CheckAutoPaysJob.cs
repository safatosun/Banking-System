using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Jobs
{
    public class CheckAutoPaysJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _loggerService;

        public CheckAutoPaysJob(IUnitOfWork unitOfWork, ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
        }

        public async Task CheckAutoPays()
        {
            await _unitOfWork.BeginTransactionAsyncWithPessimisticLocking();

            var autoPays = await _unitOfWork.Invoice.GetAllAutoPaysWithDetailsAsync();
            
            foreach(var invoice in autoPays)
            {
               if(!(invoice.Account.Balance > invoice.TotalAmount))
                {
                    _loggerService.LogInfo($"The {invoice.Name} invoice was not paid because there was not enough balance in the account with ID number {invoice.AccountId}.");
                    continue;
                }

               invoice.Account.Balance -= invoice.TotalAmount;               
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.TransactionCommitAsync();

            _loggerService.LogInfo($"The automatic payment transactions for accounts with sufficient balance have been completed.");

        }




    }
}
