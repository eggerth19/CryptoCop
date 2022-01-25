using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.ExceptionHandlerExtension
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            // TODO: Implement
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionHandlerFeature.Error;
                    var statusCode = (int) HttpStatusCode.InternalServerError;

                    if (exception is ResourceNotFoundException)
                    {
                        statusCode = (int) HttpStatusCode.NotFound;
                    }
                    else if (exception is ArgumentOutOfRangeException)
                    {
                        statusCode = (int) HttpStatusCode.BadRequest;
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsync(new ExceptionModel
                    {
                        StatusCode = statusCode,
                        ExceptionMessage = exception.Message
                    }.ToString());
                });
            });

        }
    }
}