using Application.AccountOperations.Commands.CreateAccount;
using Application.UserOperations.Commands.CreateUser;
using Application.UserOperations.Commands.ValidateUser;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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
    public class AccountControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AccountControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Create_Returns_Created()
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

        }

        [Fact]
        public async Task Create_Returns_Exception()
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
                Balance = 30
            };

            var createAccountResponse = await client.PostAsJsonAsync("api/accounts", accountDto);


            var createAccountContent = await createAccountResponse.Content.ReadAsStringAsync();

            
            Assert.Contains("For newly opened accounts, the minimum balance is 50$.", createAccountContent);
        }


    }

    public class AuthResponseClass
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

}
