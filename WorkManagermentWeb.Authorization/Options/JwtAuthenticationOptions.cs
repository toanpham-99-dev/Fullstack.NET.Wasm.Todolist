#nullable disable

namespace WorkManagermentWeb.Authorization.Options
{
    /// <summary>
    /// JwtAuthenticationOptions
    /// </summary>
    public class JwtAuthenticationOptions
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }
    }
}
