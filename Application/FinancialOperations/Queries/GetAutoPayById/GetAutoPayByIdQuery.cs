using Application.Common.Interfaces;
using Application.FinancialOperations.Queries.GetAutoPays;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Queries.GetAutoPayById
{
    public class GetAutoPayByIdQuery
    {
        public int InvoiceId { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAutoPayByIdQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AutoPayViewModel> Handle()
        {
            var autopay = await _unitOfWork.Invoice.GetByIdWithDetailsAsync(InvoiceId);
            
            if(autopay is null)
                throw new InvalidOperationException("The invoice which has autopay could not find.");


            var viemModel = _mapper.Map<AutoPayViewModel>(autopay);

            return viemModel;

        }

    }

    public class AutoPayViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string AccountName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsAutomated { get; set; }
    }


}
