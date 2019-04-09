using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kros.MediatR.Extensions
{
    /// <summary>
    /// Extensions to scan for MediatR pipeline behaviors and registers them.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Scan for MediatR pipeline behaviors 
        /// when request implement <typeparamref name="TRequest"/> and response implement <typeparamref name="TResponse"/>.
        /// </summary>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <typeparam name="TResponse">Response type.</typeparam>
        /// <param name="services">Service container.</param>
        public static IServiceCollection AddPipelineBehaviorsForRequest<TRequest, TResponse>(this IServiceCollection services)
        {
            var requestType = typeof(TRequest);
            var responseType = typeof(TResponse);
            var pipeLineType = typeof(IPipelineBehavior<,>);
            (var requests, var responses) = GetTypes(requestType, responseType);

            Type MakeGenericType(Type type, int i)
                => type.MakeGenericType(requests[i], responses[i]);

            var pipelineBehaviours = Assembly.GetAssembly(requestType).GetTypes()
                .Where(t
                => t.GetInterface(pipeLineType.Name) != null
                && t.GetGenericArguments()[0].GetInterface(requestType.Name) != null);

            foreach (var behavior in pipelineBehaviours)
            {
                for (int i = 0; i < requests.Count; i++)
                {
                    services.AddTransient(MakeGenericType(pipeLineType, i), MakeGenericType(behavior, i));
                }
            }

            return services;
        }

        private static (IList<Type>, IList<Type>) GetTypes(Type requestType, Type responseType)
        {
            IList<Type> GetTypes(Type type)
                => Assembly.GetAssembly(requestType)
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract & type.IsAssignableFrom(t))
                .ToList();

            var requests = GetTypes(requestType);
            var responses = GetTypes(responseType);

            if (requests.Count != responses.Count)
            {
                throw new InvalidOperationException(
                    string.Format(Properties.Resources.IncorrectNumberOfImplementation, requestType.Name, responseType.Name));
            }

            return (requests, responses);
        }
    }
}
