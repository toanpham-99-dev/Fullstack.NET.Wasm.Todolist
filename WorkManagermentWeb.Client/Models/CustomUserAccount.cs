using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace WorkManagermentWeb.Client.Models
{
    /// <summary>
    /// CustomUserAccount
    /// </summary>
    public class CustomUserAccount : RemoteUserAccount
    {
        /// <summary>
        /// Roles
        /// </summary>
        [JsonPropertyName("roles")]
        public List<string>? Roles { get; set; }
    }
}
