using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Jobs
{
    public  class UpdateTransferLimitJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _loggerService;

        public UpdateTransferLimitJob(IUnitOfWork unitOfWork, ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
        }

        public async Task AllAccount()
        {
            await _unitOfWork.BeginTransactionAsyncWithPessimisticLocking();
            
            var accounts = await _unitOfWork.Account.GetAllAsync(true);

             foreach (var account in accounts)
            {
                account.TransferLimit = 0;
            }

            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.TransactionCommitAsync();

            _loggerService.LogInfo($"All Accounts Transfer Limits are updated at {DateTime.Now}");
            
        }

    }
}
