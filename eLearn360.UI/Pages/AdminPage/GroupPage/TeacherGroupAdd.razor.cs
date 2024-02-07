using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class TeacherGroupAdd
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid GroupId { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected Group Group { get; set; } = new();
        protected Guid OrganizationId { get; set; } = new();
        protected Guid UserId { get; set; } = new();
        protected List<User> UserList { get; set; } = new();

        protected string groupName = string.Empty;
        protected Guid groupId = new();

        //Modal
        public bool AcceptDialogOpen { get; set; }

        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = (await AuthenticationStateTask).User;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                _isLoading = true;
                await LoadList();
                _isLoading = false;
            }
        }

        private async Task LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var OrgaId = user.FindFirst(e => e.Type == "OrganizationId").Value;
            OrganizationId = Guid.Parse(OrgaId);
            Group = await GroupService.GetById(GroupId);
            groupName = Group.Name;
            groupId = Group.Id;
            UserList = await GroupService.GetallUsersNotInGroup(groupId, OrganizationId);
        }

        // MODAL
        private void OnAcceptDialogClose(bool accepted)
        {
            if (accepted)
            {
                NavigationManager.NavigateTo($"usergrouplistoverview/{GroupId}");
            }

            AcceptDialogOpen = false;
            StateHasChanged();
        }

        private void OpenAcceptDialog(Guid userId)
        {
            UserId = userId;
            AcceptDialogOpen = true;
            StateHasChanged();
        }
    }
}
