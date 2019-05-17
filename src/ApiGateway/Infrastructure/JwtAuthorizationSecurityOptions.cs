namespace ApiGateway.Infrastructure
{
    public class JwtAuthorizationSecurityOptions
    {
        public string IdentityServerUserInfoEndpoint { get; set; }
        public string UserClaimsEndpoint { get; set; }
        public string JwtSecret { get; set; }
    }
}
