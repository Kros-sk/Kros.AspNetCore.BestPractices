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
        /// Collection of templates to move data from URL parts to http headers of authorization request.
        /// </summary>
        public UrlToHeaderTemplate[] UrlPartsToHeaders { get; set; }
    }
}