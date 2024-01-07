using Application.Common.Interfaces;
using AutoMapper;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportRequestOperations.Queries.GetSupportRequestsByUserId
{
    public class GetSupportRequestsByUserIdQuery
    {
        public string UserId { get; set;}   

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSupportRequestsByUserIdQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserSupportRequestsViewModel>> Handle()
        {
            var userSupportRequest = await _unitOfWork.SupportRequest.GetAllByUserIdAsync(UserId);

            var viewModel = _mapper.Map<List<UserSupportRequestsViewModel>>(userSupportRequest);

            return viewModel;
        }

    }

    public class UserSupportRequestsViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public PriorityLevel Priority { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
