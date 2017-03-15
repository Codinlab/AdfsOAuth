namespace Codinlab.AspNetCore.Authentication.AdfsOAuth
{
    public static class AdfsOAuthDefaults
    {
        public const string AuthenticationScheme = "Adfs";

        public static readonly string AuthorizationEndpointPath = "/adfs/oauth2/authorize";

        public static readonly string TokenEndpointPath = "/adfs/oauth2/token";
    }
}
