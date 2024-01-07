using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserOperations.Commands.CreateToken
{
    public class CreateTokenCommand
    {
        public User User { get; set; }
        public bool populateExp {get; set; }

        private readonly IBankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public CreateTokenCommand(IBankingSystemDbContext dbContext, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<TokenDto> Handle()
        {
            var signinCredentials = GetSiginCredentials(); 
            var claims = await GetAndCreateClaims(); 
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            User.RefreshToken = refreshToken;

            if (populateExp)
            {
                User.RefreshTokenExpireTime = DateTime.Now.AddMinutes(15);
            }

            await _userManager.UpdateAsync(User);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

        }

        private SigningCredentials GetSiginCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        }

        private async Task<List<Claim>> GetAndCreateClaims()
        {
            var claims = new List<Claim>() {                           
                new Claim(ClaimTypes.Name,User.UserName),
                new Claim(ClaimTypes.Email,User.Email)
            };

            var roles = await _userManager.GetRolesAsync(User);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signinCredentials); 

            return tokenOptions;

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

        }

    }

    public record TokenDto
    {
        public String AccessToken { get; init; }
        public String RefreshToken { get; init; }
    }


}
