using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class GroupOverview
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public Guid GroupId { get; set; }
        protected Group Group { get; set; }

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
            Group = await GroupService.GetById(GroupId);
            StateHasChanged();
        }
    }
}
