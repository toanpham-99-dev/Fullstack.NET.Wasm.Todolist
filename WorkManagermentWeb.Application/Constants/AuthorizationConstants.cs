namespace WorkManagermentWeb.Application.Constants
{
    /// <summary>
    /// AuthorizationConstants
    /// </summary>
    public static class AuthorizationConstants
    {
        /// <summary>
        /// Jwt
        /// </summary>
        public const string Jwt = "JWT";

        /// <summary>
        /// BearerName
        /// </summary>
        public const string BearerName = "JWT Authentication";

        /// <summary>
        /// AuthorizeDescription
        /// </summary>
        public const string AuthorizeDescription =
            "Swagger authorization using the Bearer scheme. Example: 'Bearer your_token'";

        /// <summary>
        /// AuthorizationSectionName
        /// </summary>
        public const string AuthorizationSectionName = "Authorization";

        /// <summary>
        /// Token
        /// </summary>
        public const string Token = "token";

        /// <summary>
        /// GraphToken
        /// </summary>
        public const string GraphToken = "graph-token";

        /// <summary>
        /// Bearer
        /// </summary>
        public const string Bearer = "Bearer";

        /// <summary>
        /// TwoAuthPolicy
        /// </summary>
        public const string TwoAuthPolicy = "JwtOrAzureAD";
    }
}
