using Application.AccountOperations.Commands.CreateAccount;
using Application.FinancialOperations.Commands.CreateAutoPay;
using Application.FinancialOperations.Queries.GetAutoPays;
using Application.FinancialOperations.Queries.GetAutoPayById;
using Application.UserOperations.Commands.CreateUser;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SupportRequestOperations.Commands.CreateSupportRequest;
using Application.SupportRequestOperations.Queries.GetSupportRequests;
using Application.SupportRequestOperations.Commands.UpdateSupportRequest;
using Application.SupportRequestOperations.Queries.GetSupportRequestsByUserId;
using Domain.Enums;
using Application.CreditOperations.Commands.CreateCredit;
using Application.CreditOperations.Queries.GetCredits;
using Application.CreditOperations.Queries.GetCreditsByUserId;
using Application.CreditOperations.Commands.UpdateCreditSituation;

namespace Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();
            
            CreateMap<CreateAccountDto,Account>();  
           
            CreateMap<CreateAutoPayDto,Invoice>();
            
            CreateMap<Invoice,AutoPaysViewModel>().ForMember(dest=>dest.AccountName,opt=>opt.MapFrom(src=>src.Account.Name));
            CreateMap<Invoice, AutoPayViewModel>().ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name));
            
            CreateMap<CreateSupportRequestDto, SupportRequest>().ForMember(dest => dest.Priority, opt => opt.MapFrom(src => 1))
                                                                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                                                                .ForMember(dest => dest.IsProcessed, opt => opt.MapFrom(src => false));
            CreateMap<SupportRequest, SupportRequestsViewModel>();
            CreateMap<UpdateSupportRequestDto, SupportRequest>();
            CreateMap<SupportRequest, UserSupportRequestsViewModel>().ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (PriorityLevel)src.Priority));

            CreateMap<CreateCreditDto, Credit>()
                                       .ForMember(dest => dest.IsProcessed, opt => opt.MapFrom(src => false))
                                       .ForMember(dest => dest.InstallmentAmount,opt=> opt.MapFrom(src => Math.Round((double)src.RequestedAmount / src.Maturity, 2)));

            CreateMap<Credit, CreditViewModel>();
            CreateMap<Credit,UserCreditsViewModel>();
            CreateMap<UpdateCreditSituationDto,Credit>();

        }                                                       
    }
}
