using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class GroupListOverview
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<Group> GroupList { get; set; } = new();
        protected Guid OrganizationId { get; set; } = Guid.NewGuid();
        private bool _isLoading = false;
        private string _message = string.Empty;

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
            OrganizationId = Guid.Parse(orgaIdToken);
            GroupList = (await GroupService.GetByOrganizationId(OrganizationId)).ToList();
            StateHasChanged();
        }
    }
}
