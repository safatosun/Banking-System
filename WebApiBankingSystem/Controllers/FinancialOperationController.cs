using Application.FinancialOperations.Commands.CreateAutoPay;
using Application.FinancialOperations.Commands.DeleteAutoPay;
using Application.FinancialOperations.Commands.Deposit;
using Application.FinancialOperations.Commands.Transfer.Commands;
using Application.FinancialOperations.Commands.Withdrawal;
using Application.FinancialOperations.Queries.GetAutoPayById;
using Application.FinancialOperations.Queries.GetAutoPays;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApiBankingSystem.Controllers
{
    [ApiController]
    [Route("api/financialOperations")]
    public class FinancialOperationController : ControllerBase
    {
        private readonly DepositCommand _depositCommand;
        private readonly WithdrawalCommand _withdrawalCommand;
        private readonly TransferCommand  _transferCommand;

        private readonly CreateAutoPayCommand _createAutoPayCommand;
        private readonly GetAutoPaysQuery _getAutoPaysQuery;
        private readonly GetAutoPayByIdQuery _getAutoPayByIdQuery;
        private readonly DeleteAutoPayCommand _deleteAutoPayCommand;

        public FinancialOperationController(DepositCommand depositCommand, WithdrawalCommand withdrawalCommand, TransferCommand transferCommand, CreateAutoPayCommand createAutoPayCommand, GetAutoPayByIdQuery getAutoPayByIdQuery, DeleteAutoPayCommand deleteAutoPayCommand, GetAutoPaysQuery getAutoPaysQuery)
        {
            _depositCommand = depositCommand;
            _withdrawalCommand = withdrawalCommand;
            _transferCommand = transferCommand;
            _createAutoPayCommand = createAutoPayCommand;
            _getAutoPayByIdQuery = getAutoPayByIdQuery;
            _deleteAutoPayCommand = deleteAutoPayCommand;
            _getAutoPaysQuery = getAutoPaysQuery;
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpPut("{userId}/deposit")]
        public async Task<IActionResult> Deposit([FromRoute]string userId,[FromBody]DepositDto modelDto)
        {
            _depositCommand.UserId = userId;    
            _depositCommand.ModelDto = modelDto;
            await _depositCommand.Handle();
            return Ok();
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpPut("{userId}/withdrawal")]
        public async Task<IActionResult> Withdrawal([FromRoute] string userId, [FromBody] WithdrawalDto modelDto)
        {
            _withdrawalCommand.UserId = userId;
            _withdrawalCommand.ModelDto = modelDto;
            await _withdrawalCommand.Handle();
            return Ok();
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto modelDto)
        {
            _transferCommand.ModelDto = modelDto;
            await _transferCommand.Handle();
            return Ok();
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpPost("autopay")]
        public async Task<IActionResult> CreateAutoPay([FromBody] CreateAutoPayDto modelDto)
        {
            _createAutoPayCommand.ModelDto = modelDto;
            await _createAutoPayCommand.Handle();   
            return StatusCode(201);
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpGet("{accountId}/autopays")]
        public async Task<IActionResult> GetAllAutoPays([FromRoute]int accountId)
        {
            _getAutoPaysQuery.AccountId = accountId;
            var autopays = await _getAutoPaysQuery.Handle();
            return Ok(autopays);
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpGet("{invoiceId}/autopay")]
        public async Task<IActionResult> GetById([FromRoute] int invoiceId)
        {
            _getAutoPayByIdQuery.InvoiceId = invoiceId;
            var autopay = await _getAutoPayByIdQuery.Handle();
            return Ok(autopay);
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpDelete("{invoiceId}/autopay")]
        public async Task<IActionResult> Delete(int invoiceId)
        {
            _deleteAutoPayCommand.InvoiceId = invoiceId;
            await _deleteAutoPayCommand.Handle();
            return Ok();
        }

    }
}
