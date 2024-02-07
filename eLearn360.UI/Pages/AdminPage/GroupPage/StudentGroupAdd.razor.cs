using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class StudentGroupAdd
    {
        [Inject] public IOrganizationService OrganizationService { get; set; }
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid GroupId { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        protected Organization Organization { get; set; } = new();
        protected List<UserHasOccupation> UserHasOccupationsList { get; set; } = new();
        protected List<User> UserList { get; set; } = new();
        protected List<UserHasGroup> UserHasGroups { get; set; } = new();
        protected UserHasGroup UserHasGroup { get; set; } = new();
        protected Group Group { get; set; } = new();
        protected string groupName = string.Empty;
        private DateTime _today = DateTime.Now;

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
            Guid OrganizationId = Guid.Parse(OrgaId);
            Group = await GroupService.GetById(GroupId);
            groupName = Group.Name;
            //Organization = await OrganizationService.GetAllUserByOrganizationId(OrganizationId);
            UserList = await GroupService.GetallUsersNotInGroup(GroupId, OrganizationId);
        }

        protected async Task AddStudent(Guid userId)
        {
            UserHasGroup = new()
            {
                GroupId = GroupId,
                UserId = userId,
                StartDate = _today
            };

            UserHasGroups.Add(UserHasGroup);
            Group.UserHasGroups = new List<UserHasGroup>(UserHasGroups);

            await UpdateList();
        }


        private async Task UpdateList()
        {
            var res = await GroupService.Update(Group);
            if (res.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo($"usergrouplistoverview/{GroupId}");
            }
        }
    }
}
