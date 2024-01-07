using Application.Common.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportRequestOperations.Commands.UpdateSupportRequest
{
    public class UpdateSupportRequestCommand
    {
        public int SupportRequestId { get; set; }   
        public UpdateSupportRequestDto ModelDto { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSupportRequestCommand(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle()
        {
           var supportRequest = await _unitOfWork.SupportRequest.GetOneByIdAsync(SupportRequestId,true);

           _mapper.Map(ModelDto, supportRequest);
           
          await _unitOfWork.SaveChangesAsync();

        }

    }

    public record UpdateSupportRequestDto
    {
        public int Priority { get; init; }
        public bool IsProcessed { get; init; }
    }

}
