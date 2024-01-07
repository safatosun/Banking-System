using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.CreateAutoPay
{
    public class CreateAutoPayCommand
    {
        public CreateAutoPayDto ModelDto { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public CreateAutoPayCommand(IUnitOfWork unitOfWork, IMapper mapper, ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        public async Task Handle()
        {
            var account = await _unitOfWork.Account.GetOneByIdAsync(ModelDto.AccountId,true);

            if (account is null)
                throw new InvalidOperationException("The Account could not find.");

            var autoPayInvoice = _mapper.Map<Invoice>(ModelDto);

             _unitOfWork.Invoice.CreateOne(autoPayInvoice);

            await _unitOfWork.SaveChangesAsync();

            _loggerService.LogInfo($"The user with ID number {account.UserId} has set up automatic payment instructions for {ModelDto.Name} services.");
        }


    }

    public record CreateAutoPayDto
    {
        public string Name { get; init; }
        public string CompanyName { get; init; }
        public int AccountId { get; init; }
        public decimal TotalAmount { get; init; }
        public bool IsAutomated { get; init; }

    }

}
