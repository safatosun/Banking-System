using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportRequestOperations.Queries.GetSupportRequests
{
    public class GetSupportRequestsQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSupportRequestsQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SupportRequestsViewModel>> Handle()
        {
           var supportRequests = await _unitOfWork.SupportRequest.GetAllAsync(true);

           var vmModel = _mapper.Map<List<SupportRequestsViewModel>>(supportRequests);

           return vmModel;

        }
    }

    public class SupportRequestsViewModel
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Priority { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
