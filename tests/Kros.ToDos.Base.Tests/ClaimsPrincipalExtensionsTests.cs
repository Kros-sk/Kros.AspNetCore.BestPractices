using FluentAssertions;
using Kros.ToDos.Base.Extensions;
using Kros.ToDos.Base.Infrastructure;
using System.Security.Claims;
using Xunit;

namespace Kros.ToDos.Base.Tests
{
    public class ClaimsPrincipalExtensionsTests
    {
        [Theory]
        [InlineData("10", 10)]
        [InlineData("-5", -5)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void ReturnCorrectOrganizationId(string claimValue, int expectedOrgId)
        {
            var identity = new ClaimsIdentity();
            if (claimValue != null)
            {
                identity.AddClaim(new Claim(PermissionsHelper.Claims.OrganizationId, claimValue));
            }
            var principal = new ClaimsPrincipal(identity);

            int orgId = principal.GetOrganizationId();

            orgId.Should().Be(expectedOrgId);
        }
    }
}
