using Application.UserOperations.Commands.CreateToken;
using Application.UserOperations.Commands.CreateUser;
using Application.UserOperations.Commands.UpdateUserRoles;
using Application.UserOperations.Commands.ValidateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBankingSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly CreateUserCommand _createUserCommand;
        private readonly ValidateUserCommand _validateUserCommand;
        private readonly CreateTokenCommand _createTokenCommand;
        private readonly UpdateUserRolesCommand _updateUserRolesCommand;

        public UserController(CreateUserCommand createUserCommand, ValidateUserCommand validateUserCommand, CreateTokenCommand createTokenCommand, UpdateUserRolesCommand updateUserRolesCommand)
        {
            _createUserCommand = createUserCommand;
            _validateUserCommand = validateUserCommand;
            _createTokenCommand = createTokenCommand;
            _updateUserRolesCommand = updateUserRolesCommand;
        }

        [HttpPost]
        public async  Task<IActionResult> Create([FromBody] UserRegistrationDto modelDto)
        {
            _createUserCommand.ModelDto = modelDto;
            
            var result = await _createUserCommand.Handle();


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) 
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);

        }

        [HttpPost("login")]        
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto user)
        {
            _validateUserCommand.ModelDto = user;
            
            var validatedUser = await _validateUserCommand.Handle();

            _createTokenCommand.User = validatedUser;
            _createTokenCommand.populateExp = true;

            var tokenDto = await _createTokenCommand.Handle();

            return Ok(tokenDto);
        }
        
        [Authorize(Roles ="Admin,Staff")]
        [HttpPut("roles")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesDto rolesDto)
        {
            _updateUserRolesCommand.RolesDto = rolesDto;
            await _updateUserRolesCommand.Handle();
            return Ok();
        }

    }
}
