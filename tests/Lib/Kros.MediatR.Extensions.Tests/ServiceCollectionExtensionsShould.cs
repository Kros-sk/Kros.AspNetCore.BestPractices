using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Kros.MediatR.Extensions.Tests
{
    public class ServiceCollectionExtensionsShould
    {
        #region Nested class

        public interface IFooRequest { }

        public interface IFooResponse { }

        public class FooRequest: IRequest<FooRequest.FooResponse>, IFooRequest
        {
            public class FooResponse: IFooResponse
            {
            }
        }

        public class FooPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IFooRequest
            where TResponse : IFooResponse
        {
            public async Task<TResponse> Handle(
                TRequest request,
                CancellationToken cancellationToken,
                RequestHandlerDelegate<TResponse> next)
            {
                var result = await next();               
                return result;
            }
        }

        #endregion

        [Fact]
        public void RegisterPipelineBehaviorsForRequestType()
        {
            var services = new ServiceCollection();

            services.AddPipelineBehaviorsForRequest<IFooRequest, IFooResponse>();

            var provider = services.BuildServiceProvider();

            var behavior = provider.GetRequiredService<IPipelineBehavior<FooRequest, FooRequest.FooResponse>>();

            behavior.Should().BeAssignableTo<FooPipelineBehavior<FooRequest, FooRequest.FooResponse>>();
        }
    }
}
