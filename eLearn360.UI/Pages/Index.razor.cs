using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages
{
    public partial class Index
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private bool _isLoading { get; set; } = false;

        protected string RoleName = string.Empty;
        protected string OrgaName = string.Empty;
        protected string OrgaImg = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            var authenticationState = (await AuthenticationStateTask).User;

            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                RoleName = authenticationState.FindFirst(e => e.Type == ClaimTypes.Role).Value;
                OrgaName = authenticationState.FindFirst(e => e.Type == "OrganizationName").Value;
                OrgaImg = authenticationState.FindFirst(e => e.Type == "OrganizationImg")?.Value;
            }
            _isLoading = false;
        }

    }
}
