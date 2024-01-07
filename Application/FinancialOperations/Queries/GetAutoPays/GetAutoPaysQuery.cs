using Application.Common.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancialOperations.Queries.GetAutoPays
{
    public class GetAutoPaysQuery
    {
        public int AccountId { get; set; }  

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAutoPaysQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AutoPaysViewModel>> Handle()
        {
            var autopays = await _unitOfWork.Invoice.GetAllByAccountIdAsync(AccountId);

            var viemModel = _mapper.Map<List<AutoPaysViewModel>>(autopays);

            return viemModel;

        }

    }

    public class AutoPaysViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string AccountName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsAutomated { get; set; }
    }

}
