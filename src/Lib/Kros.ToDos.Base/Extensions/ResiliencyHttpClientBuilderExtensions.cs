using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Kros.ToDos.Base.Extensions
{
    /// <summary>
    /// Extensions methods for configuring <see cref="PolicyHttpMessageHandler"/> message handlers as part of
    /// and <see cref="HttpClient"/> message handler pipeline.
    /// </summary>
    public static class ResiliencyHttpClientBuilderExtensions
    {
        private const int DefaultRetryCount = 5;
        private const int DefaultFirstExponentialWaitTimeInMilliseconds = 500;

        private const int DefaultHandledEventsAllowedBeforeBreakingCount = 3;
        private const int DefaultDurationOfBreakInSeconds = 15;
        
        /// <summary>
        /// Adds default's <see cref="PolicyHttpMessageHandler"/> handlers for resiliency.
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
        /// <remarks>
        /// <para>
        /// See the remarks on <see cref="PolicyHttpMessageHandler"/> for guidance on configuring policies.
        /// </para>
        /// </remarks>
        public static IHttpClientBuilder AddResiliencyDefaultPolicyHandler(this IHttpClientBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            IAsyncPolicy<HttpResponseMessage> retryPolicy = GetRetryPolicy();
			IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = GetCircuitBreakerPolicy();

			builder.AddHttpMessageHandler(() => new PolicyHttpMessageHandler(retryPolicy.WrapAsync(circuitBreakerPolicy)));

            return builder;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            Random jitterer = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
				.WaitAndRetryAsync(DefaultRetryCount,
                    retryAttempt => {
						TimeSpan timeSpan = TimeSpan.FromMilliseconds(DefaultFirstExponentialWaitTimeInMilliseconds * Math.Pow(2, retryAttempt))
							+ TimeSpan.FromMilliseconds(jitterer.Next(0, 100));

						Console.WriteLine(retryAttempt);
						Console.WriteLine(timeSpan);

						return timeSpan;
					}
                );
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
				//.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
				.CircuitBreakerAsync(DefaultHandledEventsAllowedBeforeBreakingCount, TimeSpan.FromSeconds(DefaultDurationOfBreakInSeconds));
        }
    }
}
