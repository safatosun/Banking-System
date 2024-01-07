using Application.CreditOperations.Commands.CreateCredit;
using Application.CreditOperations.Commands.UpdateCreditSituation;
using Application.CreditOperations.Queries.CreditScoreChecking;
using Application.CreditOperations.Queries.GetCredits;
using Application.CreditOperations.Queries.GetCreditsByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBankingSystem.Controllers
{
    [ApiController]
    [Route("api/credits")]
    public class CreditController : ControllerBase
    {
        private readonly CreateCreditCommand _createCreditCommand;
        private readonly UpdateCreditSituationCommand _updateCreditSituationCommand;
        private readonly GetCreditsQuery _getCreditsQuery;
        private readonly GetCreditsByUserIdQuery _getCreditsByUserIdQuery;

        public CreditController(CreateCreditCommand createCreditCommand, UpdateCreditSituationCommand updateCreditSituationCommand,
            GetCreditsQuery getCreditsQuery, GetCreditsByUserIdQuery getCreditsByUserIdQuery)
        {
            _createCreditCommand = createCreditCommand;
            _updateCreditSituationCommand = updateCreditSituationCommand;
            _getCreditsQuery = getCreditsQuery;
            _getCreditsByUserIdQuery = getCreditsByUserIdQuery;
        }

        [Authorize(Roles ="Staff,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var credits = await _getCreditsQuery.Handle();
            return Ok(credits);
        }

        [Authorize(Roles = "User,Staff,Admin")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] string userId)
        {
            _getCreditsByUserIdQuery.UserId= userId;
            var credits = await _getCreditsByUserIdQuery.Handle();
            return Ok(credits);
        }

        [Authorize(Roles ="User,Staff,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCreditDto modelDto )
        {
            _createCreditCommand.ModelDto = modelDto;
            await _createCreditCommand.Handle();
            return StatusCode(201);            
        }
        
        [Authorize(Roles = "Staff,Admin")]
        [HttpPut("{creditId}")]
        public async Task<IActionResult> Update([FromRoute] int creditId , [FromBody] UpdateCreditSituationDto modelDto)
        {
           _updateCreditSituationCommand.CreditId = creditId;
           _updateCreditSituationCommand.ModelDto = modelDto;
           await _updateCreditSituationCommand.Handle();
           
            return Ok();
        }


    }
}
