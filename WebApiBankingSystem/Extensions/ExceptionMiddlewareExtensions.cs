
using Application.Common.Interfaces;
using Domain.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;


namespace WebApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {

        
        public static WebApplication ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {

                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            _ => StatusCodes.Status500InternalServerError
                        };
                        logger.LogError($"Something went wrong : {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails() { 
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    
                    }
                });
            });
            return app;
        }
    }
}
