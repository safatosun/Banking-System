using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Queries.CreditScoreChecking
{
    public class CreditScoreCheckingQuery
    {
        public string UserId { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CreditScoreCheckingQuery(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<bool> Handle()
        {
            var user = await _userManager.Users.Include(u => u.Credits).Include(u=>u.Accounts).SingleOrDefaultAsync(u=>u.Id == UserId);

            if (user is null)
                throw new InvalidOperationException("The user whose credit score would be calculated could not be found.");

            int creditScore = 0;

            decimal accountTotalBalance = user.Accounts.Sum(a => a.Balance);
            creditScore += (int)(accountTotalBalance / 100);

            int numberOfApprovedCredits = user.Credits.Count(c => c.Approved == true);
            creditScore += numberOfApprovedCredits * 20;

            int totalMaturity = user.Credits.Where(c => c.Approved == true).Sum(c => c.Maturity);
            int numberOfPaidInstallments = user.Credits.Sum(c => c.PaidInstallmentCount ?? 0);
            double paidInstallmentsPercentage = (double)numberOfPaidInstallments / totalMaturity;
            int paidInstallmentsCreditScore = (int)(paidInstallmentsPercentage * 50);
            creditScore += paidInstallmentsCreditScore;

            const int acceptableCreditScoreThreshold = 220;

            bool isCreditScoreAcceptable = creditScore >= acceptableCreditScoreThreshold;

            return isCreditScoreAcceptable;

        }


    }
}
