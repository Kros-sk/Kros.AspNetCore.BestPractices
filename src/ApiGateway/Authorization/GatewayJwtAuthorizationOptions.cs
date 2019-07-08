namespace ApiGateway.Authorization
{
    /// <summary>
    /// JWT authorization options for api gateway.
    /// </summary>
    public class GatewayJwtAuthorizationOptions
    {
        public GatewayJwtAuthorizationOptions() { }

        /// <summary>
        /// Authorization service url.
        /// </summary>
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Set of headers to preserve from original request.
        /// </summary>
        public string[] HeadersToPreserve { get; set; }
    }
}