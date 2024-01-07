using Application.AccountOperations.Commands.CreateAccount;
using Application.AccountOperations.Commands.UpdateAccountBalance;
using Application.AccountOperations.Queries.GetBalanceByUserId;
using Application.Common.Interfaces;
using Application.Common.Mapping;
using Application.FinancialOperations.Commands.Deposit;
using Application.FinancialOperations.Commands.Transfer.Commands;
using Application.FinancialOperations.Commands.Withdrawal;
using Application.UserOperations.Commands.CreateToken;
using Application.UserOperations.Commands.CreateUser;
using Application.UserOperations.Commands.UpdateUserRoles;
using Application.UserOperations.Commands.ValidateUser;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Persistance.Repositories;
using System.Reflection;
using WebApi.Extensions;
using WebApiBankingSystem.Extensions;
using NLog;
using Application.Common.Services;
using Application.FinancialOperations.Queries.GetAutoPays;
using Application.FinancialOperations.Commands.CreateAutoPay;
using Application.FinancialOperations.Queries.GetAutoPayById;
using Application.FinancialOperations.Commands.DeleteAutoPay;
using Application.SupportRequestOperations.Commands.CreateSupportRequest;
using Application.SupportRequestOperations.Queries.GetSupportRequests;
using Application.SupportRequestOperations.Commands.UpdateSupportRequest;
using Application.SupportRequestOperations.Queries.GetSupportRequestsByUserId;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Application.Jobs;
using Application.CreditOperations.Commands.CreateCredit;
using Application.CreditOperations.Commands.UpdateCreditSituation;
using Application.CreditOperations.Queries.CreditScoreChecking;
using Application.CreditOperations.Queries.GetCredits;
using Application.CreditOperations.Queries.GetCreditsByUserId;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BankingSystemDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddScoped<IBankingSystemDbContext,BankingSystemDbContext>();


LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddSingleton<ILoggerService,LoggerManager>();


builder.Services.AddControllers().AddNewtonsoftJson().AddFluentValidation(s =>
                                                        {
                                                            s.RegisterValidatorsFromAssemblyContaining<CreateAccountDtoValidator>();
                                                        }
                                                       );



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IAccountRepository, AccountRepository>();    
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ISupportRequestRepository, SupportRequestRepository>();
builder.Services.AddScoped<ICreditRepository, CreditRepository>();

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddScoped<CreateUserCommand>();
builder.Services.AddScoped<ValidateUserCommand>();
builder.Services.AddScoped<CreateTokenCommand>();
builder.Services.AddScoped<UpdateUserRolesCommand>();

builder.Services.AddScoped<CreateAccountCommand>();
builder.Services.AddScoped<UpdateAccountBalanceByUserIdCommand>();
builder.Services.AddScoped<GetBalanceByUserIdQuery>();

builder.Services.AddScoped<DepositCommand>();
builder.Services.AddScoped<WithdrawalCommand>();
builder.Services.AddScoped<TransferCommand>();

builder.Services.AddScoped<CreateAutoPayCommand>();
builder.Services.AddScoped<GetAutoPaysQuery>();
builder.Services.AddScoped<GetAutoPayByIdQuery>();
builder.Services.AddScoped<DeleteAutoPayCommand>();

builder.Services.AddScoped<CreateSupportRequestCommand>();
builder.Services.AddScoped<UpdateSupportRequestCommand>();
builder.Services.AddScoped<GetSupportRequestsQuery>();
builder.Services.AddScoped<GetSupportRequestsByUserIdQuery>();

builder.Services.AddScoped<CreateCreditCommand>();
builder.Services.AddScoped<UpdateCreditSituationCommand>();
builder.Services.AddScoped<CreditScoreCheckingQuery>();
builder.Services.AddScoped<GetCreditsQuery>();
builder.Services.AddScoped<GetCreditsByUserIdQuery>();




builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);

var db = builder.Configuration.GetConnectionString("sqlConnection");

builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(db);
    RecurringJob.AddOrUpdate<UpdateTransferLimitJob>(j=>j.AllAccount(), "1 0 * * *");
    RecurringJob.AddOrUpdate<CheckAutoPaysJob>(j => j.CheckAutoPays(), "1 0 * * *");
    RecurringJob.AddOrUpdate<CheckCreditPaymentsJob>(j => j.CheckCreditPayments(), "1 0 * * *");
});

builder.Services.AddHangfireServer();



var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();

public partial class Program
{

}