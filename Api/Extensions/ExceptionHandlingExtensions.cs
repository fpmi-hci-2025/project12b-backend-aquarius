using Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static void AddExceptionHandling(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddProblemDetails(options =>
        {
            options.ExceptionDetailsPropertyName = "Exception details";
            options.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();

            options.Map<BadRequestException>(ex => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            });

            options.Map<NotFoundException>(ex => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message
            });

            options.Map<ConflictException>(ex => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message
            });

            options.Map<ForbiddenException>(ex => new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Detail = ex.Message
            });

            options.Map<Exception>(ex => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            });
        });
    }
}
