using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;

namespace Users.Infrastructure
{
    public class AgeClaimsProvider : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && !principal.HasClaim(c => c.Type == CustomClaimTypes.IsOver21))
            {
                if (principal.Identity is ClaimsIdentity identity && identity.IsAuthenticated && identity.Name != null)
                {
                    if (identity.Name.ToLower() == "bob")
                    {
                        identity.AddClaims(new[]
                        {
                            new Claim(CustomClaimTypes.IsOver21, "false", ClaimValueTypes.Boolean, "AgeVerificationClaims")
                        });
                    }
                    else
                    {
                        identity.AddClaims(new[]
                        {
                            new Claim(CustomClaimTypes.IsOver21, "true", ClaimValueTypes.Boolean, "AgeVerificationClaims")
                        });
                    }
                }
            }

            return Task.FromResult(principal);
        }
    }
}