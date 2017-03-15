using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Codinlab.AspNetCore.Authentication.AdfsOAuth
{
    internal class AdfsOAuthHandler : OAuthHandler<AdfsOAuthOptions>
    {
        public AdfsOAuthHandler(HttpClient backchannel)
            : base(backchannel)
        {

        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var scope = FormatScope();

            var state = Options.StateDataFormat.Protect(properties);
            var parameters = new Dictionary<string, string>
            {
                { "client_id", Options.ClientId },
                { "scope", scope },
                { "response_type", "code" },
                { "redirect_uri", redirectUri },
                { "state", state },
                { "resource", Options.ClientUri }
            };
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.ReadJwtToken(tokens.AccessToken);

            var decodedPayoad = Base64UrlEncoder.Decode((securityToken as JwtSecurityToken).RawPayload);
            var payload = JObject.Parse(decodedPayoad);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), properties, Options.AuthenticationScheme);
            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);

            // Map claims
            if (TryGetClaimValue(securityToken.Claims, JwtRegisteredClaimNames.NameId, out var nameId) ||
                TryGetClaimValue(securityToken.Claims, JwtRegisteredClaimNames.Sub, out nameId))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, nameId, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            if (TryGetClaimValue(securityToken.Claims, JwtRegisteredClaimNames.UniqueName, out var uniqueName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, uniqueName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            if (TryGetClaimValue(securityToken.Claims, JwtRegisteredClaimNames.FamilyName, out var familyName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Surname, familyName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            if (TryGetClaimValue(securityToken.Claims, JwtRegisteredClaimNames.GivenName, out var givenName))
            {
                identity.AddClaim(new Claim(ClaimTypes.GivenName, givenName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            if (TryGetClaimValue(securityToken.Claims, JwtRegisteredClaimNames.Email, out var email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.Email, Options.ClaimsIssuer));
            }

            await Options.Events.CreatingTicket(context);
            return context.Ticket;
        }

        private bool TryGetClaimValue(IEnumerable<Claim> claims, string claimType, out string claimValue)
        {
            var claim = claims.FirstOrDefault(c => c.Type == claimType);
            if(claim != null)
            {
                claimValue = claim.Value;
                return true;
            }
            else
            {
                claimValue = null;
                return false;
            }
        }
    }
}
