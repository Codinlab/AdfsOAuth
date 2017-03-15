using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Text.Encodings.Web;

namespace Codinlab.AspNetCore.Authentication.AdfsOAuth
{
    public class AdfsOAuthMiddleware : OAuthMiddleware<AdfsOAuthOptions>
    {
        /// <summary>
        /// Initializes a new <see cref="AdfsOAuthMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="sharedOptions"></param>
        /// <param name="options">Configuration options for the middleware.</param>
        public AdfsOAuthMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            IOptions<AdfsOAuthOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
            if (string.IsNullOrEmpty(Options.ClientUri))
            {
                throw new ArgumentException($"The '{nameof(Options.ClientUri)}' option must be provided.");
            }
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler{T}"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler{T}"/> configured with the <see cref="AdfsOAuthOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<AdfsOAuthOptions> CreateHandler()
        {
            return new AdfsOAuthHandler(Backchannel);
        }
    }
}
