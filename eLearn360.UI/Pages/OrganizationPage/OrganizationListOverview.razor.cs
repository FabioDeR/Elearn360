using eLearn360.Data.VM.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.OrganizationPage
{
    public partial class OrganizationListOverview
    {      

        [Inject] public IOrganizationService OrganizationService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<Organization> OrganizationList { get; set; } = new();

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
            OrganizationList = (await OrganizationService.GetAll()).OrderBy(x => x.Name).ToList();
        }
    }
}
