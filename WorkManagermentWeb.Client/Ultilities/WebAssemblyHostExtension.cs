using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

namespace WorkManagermentWeb.Client.Ultilities
{
    /// <summary>
    /// WebAssemblyHostExtension
    /// </summary>
    public static class WebAssemblyHostExtension
    {
        /// <summary>
        /// SetDefaultCulture
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public async static Task SetDefaultCulture(this WebAssemblyHost host)
        {
            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
            string? result = await localStorage.GetItemAsStringAsync("culture");

            CultureInfo culture;

            if (!string.IsNullOrEmpty(result))
                culture = new CultureInfo(result);
            else
            {
                culture = new CultureInfo("en-US");
                await localStorage.SetItemAsStringAsync("culture", culture.Name);
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
