using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistance.Context;
using System.Text;

namespace WebApiBankingSystem.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            
            var builder = services.AddIdentity<User, IdentityRole>(opts =>
            {
                
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = false; 
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 5; 
                
                opts.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<BankingSystemDbContext>()  
              .AddDefaultTokenProviders();

            return services;
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtSettings = configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["secretKey"];

            services.AddAuthentication(opt =>
            {    
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                    
            }).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {   
                ValidateIssuer = true, 
                ValidateAudience = true, 
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],  
                
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            });

        }


    }
}
