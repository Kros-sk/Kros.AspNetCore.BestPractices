using Kros.AspNetCore.Exceptions;
using Kros.AspNetCore.Extensions;
using Kros.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Kros.AspNetCore.Middlewares
{
    /// <summary>
    /// Middleware handles erros and sends exception to client.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">Delegate for next middleware.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = Check.NotNull(next, nameof(next));
        }

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context">Current http context.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ResourceIsForbiddenException ex)
            {
                await SetResponseFromException(context, ex, HttpStatusCode.Forbidden);
            }            
            //catch (NotFoundException ex)
            //{
            //    await SetResponseFromException(context, ex, HttpStatusCode.NotFound);
            //}
            catch (TimeoutException ex)
            {
                await SetResponseFromException(context, ex, HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static async Task SetResponseFromException(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            context.Response.ClearExceptCorsHeaders();
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(ex.Message);
        }
    }

    /// <summary>
    /// Extension for adding error handling middleware to application.
    /// </summary>
    public static class ErrorHandlingMiddlewareExtension
    {
        /// <summary>
        /// Extension for adding error handling middleware to application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public static void UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
