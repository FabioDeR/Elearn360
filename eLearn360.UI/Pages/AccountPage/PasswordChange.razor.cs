using elearn.Data.ViewModel.AccountVM;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.AccountPage
{
    public partial class PasswordChange
    {
        [Inject] public IAuthService AuthService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public AccountChangePasswordVM Password { get; set; } = new();
        private bool _isLoading = false;

        protected async override Task OnInitializedAsync()
        {
            _isLoading = true;
            var authenticationState = (await AuthenticationStateTask).User;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                var userId = authenticationState.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
                Password.UserId = Guid.Parse(userId);
            }
            _isLoading = false;

        }

        #region HandleValidRequest

        protected async Task HandleValidRequest()
        {
            var pass = await AuthService.ChangePassword(Password);

            if (pass.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("profileoverview");
            }
        }
        #endregion
    }
}
