using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System;

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

        public interface IBarRequest { }

        public interface IBarResponse { }

        public class BarRequest : IRequest<BarRequest.BarResponse>, IBarRequest
        {
            public class BarResponse : IBarResponse
            {
            }
        }

        public class Bar1Request : IRequest<Bar1Request.BarResponse>, IBarRequest
        {
            public class BarResponse : IBarResponse
            {
            }
        }

        public class BarPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IBarRequest
            where TResponse : IBarResponse
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

        public interface ITestRequest { }

        public interface ITestResponse { }

        public class TestRequest : IRequest<TestRequest.TestResponse>, ITestRequest
        {
            public class TestResponse
            {
            }
        }

        public class TestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : ITestRequest
            where TResponse : ITestResponse
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

        [Fact]
        public void RegisterPipelineBehaviorsForRequestTypeWhenMoreRequestsImplementInterface()
        {
            var services = new ServiceCollection();

            services.AddPipelineBehaviorsForRequest<IBarRequest, IBarResponse>();

            var provider = services.BuildServiceProvider();

            var behavior = provider.GetRequiredService<IPipelineBehavior<BarRequest, BarRequest.BarResponse>>();
            behavior.Should().BeAssignableTo<BarPipelineBehavior<BarRequest, BarRequest.BarResponse>>();

            var behaviorBar1 = provider.GetRequiredService<IPipelineBehavior<Bar1Request, Bar1Request.BarResponse>>();
            behaviorBar1.Should().BeAssignableTo<BarPipelineBehavior<Bar1Request, Bar1Request.BarResponse>>();
        }

        [Fact]
        public void ThrowExceptionWhenNumberOfImplementationsAreDifferent()
        {
            var services = new ServiceCollection();

            Action action = () =>  services.AddPipelineBehaviorsForRequest<ITestRequest, ITestResponse>();

            action.Should().Throw<InvalidOperationException>();
        }
    }
}
