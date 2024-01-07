using Application.Common.Interfaces;
using Application.FinancialOperations.Queries.GetAutoPayById;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Commands.DeleteAutoPay
{
    public class DeleteAutoPayCommand
    {
        public int InvoiceId { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteAutoPayCommand(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle()
        {
            var autopay = await _unitOfWork.Invoice.GetOneByIdAsync(InvoiceId,true);

            if (autopay is null)
                throw new InvalidOperationException("The invoice which has autopay could not find.");

            _unitOfWork.Invoice.DeleteOne(autopay);
            
            await _unitOfWork.SaveChangesAsync();

        }
    }

}
