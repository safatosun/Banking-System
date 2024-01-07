using Application.AccountOperations.Commands.CreateAccount;
using Application.AccountOperations.Commands.UpdateAccountBalance;
using Application.AccountOperations.Queries.GetBalanceByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBankingSystem.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {

        private readonly CreateAccountCommand _createAccountCommand;
        private readonly UpdateAccountBalanceByUserIdCommand _updateAccountBalanceByUserIdCommand;
        private readonly GetBalanceByUserIdQuery _getBalanceByUserIdQuery;


        public AccountController(CreateAccountCommand createAccountCommand, UpdateAccountBalanceByUserIdCommand updateAccountBalanceByUserIdCommand, GetBalanceByUserIdQuery getBalanceByUserIdQuery)
        {
            _createAccountCommand = createAccountCommand;
            _updateAccountBalanceByUserIdCommand = updateAccountBalanceByUserIdCommand;
            _getBalanceByUserIdQuery = getBalanceByUserIdQuery;
        }

        [Authorize(Roles= "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto modelDto)
        {

            _createAccountCommand.ModelDto = modelDto;
            await _createAccountCommand.Handle();

            return StatusCode(201);

        }
        
        [Authorize(Roles = "User,Staff,Admin")]
        [HttpGet("{userId}/balance")]
        public async Task<IActionResult> GetBalanceByUserId([FromRoute] string userId,[FromQuery]string accountName)
        {
            _getBalanceByUserIdQuery.UserId = userId;
            _getBalanceByUserIdQuery.AccountName = accountName;
            var balanceViewModel = await _getBalanceByUserIdQuery.Handle();

            return Ok(balanceViewModel);
        }


        [Authorize(Roles ="Staff,Admin")]
        [HttpPut("{userId}/balance")]
        public async Task<IActionResult> UpdateBalanceByUserId([FromRoute]string userId,[FromBody] UpdateAccountBalanceDto modelDto)
        {

            _updateAccountBalanceByUserIdCommand.UserId = userId;
            _updateAccountBalanceByUserIdCommand.ModelDto = modelDto;
            await _updateAccountBalanceByUserIdCommand.Handle();

            return Ok();

        }

    }
}
