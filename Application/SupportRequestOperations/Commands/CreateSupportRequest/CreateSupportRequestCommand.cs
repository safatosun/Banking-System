using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportRequestOperations.Commands.CreateSupportRequest
{
    public class CreateSupportRequestCommand
    {
        public  CreateSupportRequestDto ModelDto { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CreateSupportRequestCommand(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task Hande()
        {
            var user = await _userManager.FindByIdAsync(ModelDto.UserId);
            if (user is null)
                throw new InvalidOperationException("The user who want to create the support request could not find.");

            var supportRequest = _mapper.Map<SupportRequest>(ModelDto);

            _unitOfWork.SupportRequest.CreateOne(supportRequest);

            await _unitOfWork.SaveChangesAsync(); 
        }

    }

    public record CreateSupportRequestDto
    {
        public string UserId { get; init; }
        public string Text { get; init; }
    }

}
