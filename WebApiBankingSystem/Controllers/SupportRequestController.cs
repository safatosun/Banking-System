using Application.SupportRequestOperations.Commands.CreateSupportRequest;
using Application.SupportRequestOperations.Commands.UpdateSupportRequest;
using Application.SupportRequestOperations.Queries.GetSupportRequests;
using Application.SupportRequestOperations.Queries.GetSupportRequestsByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApiBankingSystem.Controllers
{
    [Route("api/supportRequests")]
    [ApiController]
    public class SupportRequestController : ControllerBase
    {
        private readonly CreateSupportRequestCommand _createSupportRequestCommand;
        private readonly UpdateSupportRequestCommand _updateSupportRequestCommand;
        private readonly GetSupportRequestsQuery _getSupportRequestsQuery;
        private readonly GetSupportRequestsByUserIdQuery _getSupportRequestsByUserIdQuery;

        public SupportRequestController(CreateSupportRequestCommand createSupportRequestCommand, UpdateSupportRequestCommand updateSupportRequestCommand,
            GetSupportRequestsQuery getSupportRequestsQuery, GetSupportRequestsByUserIdQuery getSupportRequestsByUserIdQuery)
        {
            _createSupportRequestCommand = createSupportRequestCommand;
            _updateSupportRequestCommand = updateSupportRequestCommand;
            _getSupportRequestsQuery = getSupportRequestsQuery;
            _getSupportRequestsByUserIdQuery = getSupportRequestsByUserIdQuery;
        }
        
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var supportRequest = await _getSupportRequestsQuery.Handle();
            return Ok(supportRequest);
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            _getSupportRequestsByUserIdQuery.UserId = userId;
            var supportRequest = await _getSupportRequestsByUserIdQuery.Handle();
            return Ok(supportRequest);
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupportRequestDto modelDto)
        {
            _createSupportRequestCommand.ModelDto = modelDto;
            await _createSupportRequestCommand.Hande();
            return StatusCode(201);
        }
        
        [Authorize(Roles = "Staff,Admin")]
        [HttpPut("{supportRequestId}")]
        public async Task<IActionResult> Update([FromRoute] int supportRequestId , [FromBody] UpdateSupportRequestDto modelDto)
        {
            _updateSupportRequestCommand.SupportRequestId = supportRequestId;
            _updateSupportRequestCommand.ModelDto = modelDto;
            await _updateSupportRequestCommand.Handle();

            return Ok();
        }


    }
}
