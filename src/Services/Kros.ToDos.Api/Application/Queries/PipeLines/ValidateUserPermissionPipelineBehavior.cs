﻿using Kros.AspNetCore.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Queries.PipeLines
{
    /// <summary>
    /// Pipeline behavior for validating if queried resource belong to user.
    /// </summary>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    /// <typeparam name="TResponse">Type of response.</typeparam>
    public class ValidateUserPermissionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IUserResourceQuery, IRequest<TResponse>
        where TResponse : IUserResourceQueryResult
    {

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var result = await next();

            if (result != null && (result.UserId != request.UserId || result.OrganizationId != request.OrganizationId))
            {
                throw new ResourceIsForbiddenException(string.Format(Properties.Resources.ForbiddenMessage,
                    request.UserId, typeof(TResponse), result.UserId));
            }

            return result;
        }
    }
}
