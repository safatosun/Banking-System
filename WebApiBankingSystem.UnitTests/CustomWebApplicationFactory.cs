using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.AccountOperations.Commands.CreateAccount;

namespace WebApiBankingSystem.UnitTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<BankingSystemDbContext>));
                services.Remove(serviceDescriptor);

                services.AddDbContext<BankingSystemDbContext>(options =>
                {
                    options.UseInMemoryDatabase("banking");
                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<BankingSystemDbContext>();
                
                db.Database.EnsureCreated();

                var user = new User
                {
                    Id="123456789",
                    UserName="deneme01",
                    FirstName = "deneme",
                    LastName = "denemelast",
                    Email = "deneme@gmail.com",
                    PhoneNumber = "987654321",
                };

                var account = new Account
                {
                    Id = 1,
                    UserId = "123456789",
                    Name = "savings",
                    Balance = 60,
                    TransferLimit = 11,
                };


                db.Users.Add(user);
                db.Accounts.Add(account);
                db.SaveChanges();

                builder.UseEnvironment("Development");
            });
        }
    }
}
