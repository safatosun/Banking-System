using Application.Common.Interfaces;
using Application.CreditOperations.Queries.CreditScoreChecking;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Queries.GetCredits
{
    public class GetCreditsQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreditScoreCheckingQuery _creditScoreCheckingQuery;

        public GetCreditsQuery(IUnitOfWork unitOfWork, IMapper mapper, CreditScoreCheckingQuery creditScoreCheckingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _creditScoreCheckingQuery = creditScoreCheckingQuery;
        }

        public async Task<List<CreditViewModel>> Handle()
        {
            var credits = await _unitOfWork.Credit.GetAllAsync(true);

            var viewModel = _mapper.Map<List<CreditViewModel>>(credits);

            var tasks = viewModel.Select(async credit =>
            {
                _creditScoreCheckingQuery.UserId = credit.UserId;
                credit.isRecommended = await _creditScoreCheckingQuery.Handle();
            });

            return viewModel;

        }

    }

    public class CreditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int RequestedAmount { get; set; }  
        public int Maturity { get; set; } 
        public decimal InstallmentAmount { get; set; }  
        public int? InstallmentDueDay { get; set; }   
        public bool? IsProcessed { get; set; }
        public bool? Approved { get; set; }  
        public int? PaidInstallmentCount { get; set; } 
        public bool? Paid { get; set; }          
        public bool isRecommended { get; set; }  
    }

}
