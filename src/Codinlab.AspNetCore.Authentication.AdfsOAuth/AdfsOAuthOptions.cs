using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace Codinlab.AspNetCore.Authentication.AdfsOAuth
{
    /// <summary>
    /// Configuration options for <see cref="AdfsOAuthMiddleware"/>.
    /// </summary>
    public class AdfsOAuthOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="AdfsOAuthOptions"/>.
        /// </summary>
        public AdfsOAuthOptions() : base()
        {
            AuthenticationScheme = AdfsOAuthDefaults.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            CallbackPath = new PathString("/signin-adfs");
            ClientSecret = "N/A";
        }

        /// <summary>
        /// Initializes a new <see cref="AdfsOAuthOptions"/> from ADFS server hostname.
        /// </summary>
        /// <param name="serverHostname">ADFS server host name.</param>
        public AdfsOAuthOptions(string serverHostname) : this()
        {
            if (!string.IsNullOrEmpty(serverHostname))
            {
                var serverUri = new UriBuilder("https", serverHostname).Uri;
                ClaimsIssuer = serverUri.AbsolutePath;
                AuthorizationEndpoint = new UriBuilder(serverUri) { Path = AdfsOAuthDefaults.AuthorizationEndpointPath }.ToString();
                TokenEndpoint = new UriBuilder(serverUri) { Path = AdfsOAuthDefaults.TokenEndpointPath }.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the client URI as defiend in ADFS
        /// </summary>
        public string ClientUri { get; set; }
    }
}
