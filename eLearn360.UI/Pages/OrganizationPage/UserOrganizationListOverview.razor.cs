using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.OrganizationPage
{
    public partial class UserOrganizationListOverview
    {
        [Inject] public IOrganizationService OrganizationService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = new();

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
                await LoadList();
            }
            _isLoading = false;
        }

        protected async Task LoadList()
        {
            Organization = await OrganizationService.GetAllUserByOrganizationId(OrganizationId);
        }
    }
}
