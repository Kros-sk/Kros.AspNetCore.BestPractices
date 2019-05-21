namespace Kros.Authorization.Api.Application.Options
{
    public class JwtAuthorizationSecurityOptions
    {
        public string IdentityServerUserInfoEndpoint { get; set; }
        public string JwtSecret { get; set; }
    }
}
