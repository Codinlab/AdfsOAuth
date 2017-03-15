using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace Codinlab.AspNetCore.Authentication.AdfsOAuth
{
    /// <summary>
    /// Extension methods to add ADFS authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AdfsOAuthAppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="AdfsOAuthMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables ADFS authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAdfsOAuthAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<AdfsOAuthMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="AdfsOAuthMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables ADFS authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="AdfsOAuthOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAdfsOAuthAuthentication(this IApplicationBuilder app, AdfsOAuthOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<AdfsOAuthMiddleware>(Options.Create(options));
        }
    }
}
