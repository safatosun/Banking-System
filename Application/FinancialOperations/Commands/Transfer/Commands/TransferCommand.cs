using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.Transfer.Commands
{
    public class TransferCommand
    {
        public TransferDto ModelDto { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _loggerService;

        public TransferCommand(IUnitOfWork unitOfWork, ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
        }

        public async Task Handle()
        {
            await _unitOfWork.BeginTransactionAsyncWithPessimisticLocking();

            var senderAccount = await _unitOfWork.Account.GetOneByIdAsync(ModelDto.SenderAccountId,true);

            if (senderAccount is null)
                throw new InvalidOperationException("The sender account could not find.");

            if (senderAccount.Balance < ModelDto.Amount)
                throw new InvalidOperationException("Your current balance is less than the amount you want to send.");

            if(senderAccount.TransferLimit >=10)
                throw new InvalidOperationException("Your daily transfer limit has been reached. Please try again after 00:01");

            senderAccount.Balance -= ModelDto.Amount;
            senderAccount.TransferLimit += 1;

            var recevierAccount = await _unitOfWork.Account.GetOneByIdAsync(ModelDto.ReceiverAccountId, true);

            if (recevierAccount is null)
                throw new InvalidOperationException("The receiver account could not find.");

            recevierAccount.Balance += ModelDto.Amount;

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.TransactionCommitAsync();
           
            _loggerService.LogInfo($"{ModelDto.Amount} dollars has been transferred from account number {ModelDto.SenderAccountId} to {ModelDto.ReceiverAccountId}.");

        }
    }

    public record TransferDto
    {
        public int SenderAccountId { get; init; }

        public int ReceiverAccountId { get; init; }

        public decimal Amount { get; init; }
    }

}
