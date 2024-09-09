using Microsoft.AspNetCore.Components;
using WorkManagermentWeb.Client.Interfaces;

namespace WorkManagermentWeb.Client.Components
{
    /// <summary>
    /// CustomNotAuthorized
    /// </summary>
    public partial class CustomNotAuthorized : IDisposable
    {
        [Inject]
        private IAuthService AuthService { get; set; } = default!;

        /// <summary>
        /// ExternalAuthMode
        /// </summary>
        [Parameter] public bool ExternalAuthMode { get; set; }

        /// <summary>
        /// ChildContent
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        /// <summary>
        /// OnInitialized
        /// </summary>
        protected override void OnInitialized()
        {
            AuthService.OnAuthStateChanged += AuthStateChanged;
        }

        /// <summary>
        /// AuthStateChanged
        /// </summary>
        private void AuthStateChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            AuthService.OnAuthStateChanged -= AuthStateChanged;
        }
    }
}
