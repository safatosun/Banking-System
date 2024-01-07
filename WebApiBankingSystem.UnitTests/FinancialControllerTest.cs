using Application.AccountOperations.Commands.CreateAccount;
using Application.FinancialOperations.Commands.Deposit;
using Application.FinancialOperations.Commands.Transfer.Commands;
using Application.UserOperations.Commands.CreateUser;
using Application.UserOperations.Commands.ValidateUser;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiBankingSystem.UnitTests
{
    public class FinancialControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public FinancialControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Deposit_Returns_Successful()
        {
            var client = _factory.CreateClient();

            var userDto = new UserRegistrationDto
            {
                FirstName = "ismail",
                LastName = "caliskan",
                UserName = "ismail01",
                Email = "ismail@gmail.com",
                Password = "ismail12345",
                PhoneNumber = "1234567890",
                Roles = new List<string> { "Admin" }
            };

            var response = await client.PostAsJsonAsync("api/users", userDto);

            response.EnsureSuccessStatusCode();


            var userAuth = new UserAuthenticationDto
            {
                Email = "ismail@gmail.com",
                Password = "ismail12345"
            };

            var authResponse = await client.PostAsJsonAsync("api/users/login", userAuth);

            authResponse.EnsureSuccessStatusCode();

            var content = await authResponse.Content.ReadAsStringAsync();


            var authResult = JsonSerializer.Deserialize<AuthResponseClass>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            string accessToken = authResult?.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var accountDto = new CreateAccountDto
            {
                UserId = "123456789",
                Name = "Investment",
                Balance = 300
            };

            var createAccountResponse = await client.PostAsJsonAsync("api/accounts", accountDto);

            createAccountResponse.EnsureSuccessStatusCode();

            var depositDto = new DepositDto
            {               
                AccountName = "Investment",
                Amount = 10
            };

            var userId = "123456789";
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var depositResponse = await client.PutAsJsonAsync($"api/financialOperations/{userId}/deposit", depositDto);
            
            depositResponse.EnsureSuccessStatusCode();



            var balanceResponse = await client.GetAsync($"api/accounts/{userId}/balance?accountName={depositDto.AccountName}");

            balanceResponse.EnsureSuccessStatusCode();

            
            var balanceContent = await balanceResponse.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            var result = JsonSerializer.Deserialize<BalanceResponse>(balanceContent, options);

            Assert.Equal(310,result.Balance);

        }

        [Fact]
        public async Task Transfer_Returns_Successful()
        {
            var client = _factory.CreateClient();

            var userDto = new UserRegistrationDto
            {
                FirstName = "ismail",
                LastName = "caliskan",
                UserName = "ismail01",
                Email = "ismail@gmail.com",
                Password = "ismail12345",
                PhoneNumber = "1234567890",
                Roles = new List<string> { "Admin" }
            };

            var response = await client.PostAsJsonAsync("api/users", userDto);

            response.EnsureSuccessStatusCode();


            var userAuth = new UserAuthenticationDto
            {
                Email = "ismail@gmail.com",
                Password = "ismail12345"
            };

            var authResponse = await client.PostAsJsonAsync("api/users/login", userAuth);

            authResponse.EnsureSuccessStatusCode();

            var content = await authResponse.Content.ReadAsStringAsync();


            var authResult = JsonSerializer.Deserialize<AuthResponseClass>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            string accessToken = authResult?.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var accountDto = new CreateAccountDto
            {
                UserId = "123456789",
                Name = "Investment",
                Balance = 300
            };

            var createAccountResponse = await client.PostAsJsonAsync("api/accounts", accountDto);

            createAccountResponse.EnsureSuccessStatusCode();

            var transferDto = new TransferDto
            {
                SenderAccountId = 2, 
                ReceiverAccountId = 1, 
                Amount = 100
            };

            var userId = "123456789";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var depositResponse = await client.PostAsJsonAsync($"api/financialOperations/transfer", transferDto);

            depositResponse.EnsureSuccessStatusCode();


            var senderAccountResponse = await client.GetAsync($"api/accounts/{userId}/balance?accountName={accountDto.Name}");

            senderAccountResponse.EnsureSuccessStatusCode();


            var senderAccountBalanceContent = await senderAccountResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            var senderAccountBalance = JsonSerializer.Deserialize<BalanceResponse>(senderAccountBalanceContent, options);

            var receiverAccountResponse = await client.GetAsync($"api/accounts/{userId}/balance?accountName=savings");

            receiverAccountResponse.EnsureSuccessStatusCode();


            var receiverAccountResponseContent = await receiverAccountResponse.Content.ReadAsStringAsync();

            var receiver0ptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            var recevierAccountBalance = JsonSerializer.Deserialize<BalanceResponse>(receiverAccountResponseContent, receiver0ptions);

            Assert.Equal(200, senderAccountBalance.Balance);
            Assert.Equal(160, recevierAccountBalance.Balance);

        }

        [Fact]
        public async Task Transfer_Returns_InvalidOperationException()
        {
            var client = _factory.CreateClient();

            var userDto = new UserRegistrationDto
            {
                FirstName = "ismail",
                LastName = "caliskan",
                UserName = "ismail01",
                Email = "ismail@gmail.com",
                Password = "ismail12345",
                PhoneNumber = "1234567890",
                Roles = new List<string> { "Admin" }
            };

            var response = await client.PostAsJsonAsync("api/users", userDto);

            response.EnsureSuccessStatusCode();


            var userAuth = new UserAuthenticationDto
            {
                Email = "ismail@gmail.com",
                Password = "ismail12345"
            };

            var authResponse = await client.PostAsJsonAsync("api/users/login", userAuth);

            authResponse.EnsureSuccessStatusCode();

            var content = await authResponse.Content.ReadAsStringAsync();


            var authResult = JsonSerializer.Deserialize<AuthResponseClass>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            string accessToken = authResult?.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var accountDto = new CreateAccountDto
            {
                UserId = "123456789",
                Name = "Investment",
                Balance = 300
            };

            var createAccountResponse = await client.PostAsJsonAsync("api/accounts", accountDto);

            createAccountResponse.EnsureSuccessStatusCode();

            var transferDto = new TransferDto
            {
                SenderAccountId = 1,
                ReceiverAccountId = 2,
                Amount = 50
            };

            var userId = "123456789";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var transferResponse = await client.PostAsJsonAsync($"api/financialOperations/transfer", transferDto);

            var transferResponseContent = await transferResponse.Content.ReadAsStringAsync();

            Assert.Contains("Your daily transfer limit has been reached. Please try again after 00:01", transferResponseContent);
        }

    }

    public class BalanceResponse
    {
        public decimal Balance { get; set; }
    }


}
