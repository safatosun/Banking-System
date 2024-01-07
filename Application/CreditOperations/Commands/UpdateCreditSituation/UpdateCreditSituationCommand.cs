using Application.Common.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditOperations.Commands.UpdateCreditSituation
{
    public class UpdateCreditSituationCommand
    {
        public int CreditId { get; set; }           
        public UpdateCreditSituationDto ModelDto { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCreditSituationCommand(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle()
        {
            var credit = await _unitOfWork.Credit.GetOneByIdAsync(CreditId,true);

            if (credit is null)
                throw new InvalidOperationException("The credit could not find.");

            _mapper.Map(ModelDto,credit);

            if(ModelDto.Approved == true)
            {
                credit.IsProcessed = true;
                credit.InstallmentDueDay = DateTime.Now.Day;
                _unitOfWork.SaveChangesAsync();
                return;
            }

            _unitOfWork.SaveChangesAsync();
        }

    }


    public record UpdateCreditSituationDto
    {
        public bool IsProcessed { get; init; }
        public bool Approved { get; init; } 

    }
}
