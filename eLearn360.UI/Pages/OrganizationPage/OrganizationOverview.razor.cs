using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.OrganizationPage
{
    public partial class OrganizationOverview
    {
        [Inject] public IOrganizationService OrganizationService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        protected Organization Organization { get; set; }

        private bool _isLoading = false;

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
                LoadList();
                _isLoading = false;
            }
            _isLoading = false;
        }

        protected async void LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var orgaIdToken = user.FindFirst(e => e.Type == "OrganizationId").Value;
            Organization = await OrganizationService.GetById(Guid.Parse(orgaIdToken));
            StateHasChanged();
        }
    }
}
