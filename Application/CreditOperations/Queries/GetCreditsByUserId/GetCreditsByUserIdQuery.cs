using Application.Common.Interfaces;
using Application.CreditOperations.Queries.CreditScoreChecking;
using Application.CreditOperations.Queries.GetCredits;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Queries.GetCreditsByUserId
{
    public class GetCreditsByUserIdQuery
    {
        public string UserId { get; set; } 

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCreditsByUserIdQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserCreditsViewModel>> Handle()
        {
            var credits = await _unitOfWork.Credit.GetAllByUserIdAsync(UserId);

            var viewModel = _mapper.Map<List<UserCreditsViewModel>>(credits);

            return viewModel;

        }

    }


    public class UserCreditsViewModel
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
    }


}
