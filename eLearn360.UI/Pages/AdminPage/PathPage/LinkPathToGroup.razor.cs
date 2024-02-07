using eLearn360.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.AdminPage.PathPage
{
    public partial class LinkPathToGroup
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] public IPathWayService PathWayService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public Guid PathWayId { get; set; }
        public Group Group { get; set; } = new();
        public PathWay PathWay { get; set; } = new();
        public List<Group> GroupList { get; set; } = new();
        protected PathWayHasGroup PathWayHasGroup { get; set; } = new();
        public List<Guid> ListGroupId = new();

        private bool _isLoading = false;

        //Modal
        public bool AcceptDialogOpen { get; set; }

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

            GroupList = await GroupService.GetByOrganizationId(OrganizationId);
            PathWay = await PathWayService.GetById(PathWayId);

            await LoadGuid();
            StateHasChanged();
        }

        private async Task LoadGuid()
        {
            ListGroupId = await PathWayService.GetGroupGuid(PathWayId);
            StateHasChanged();
        }

        #region Add Group
        private async Task AddGroup(Guid GroupId)
        {
            Group = await GroupService.GetById(GroupId);

            Group.PathWayHasGroups = new();
            Group.PathWayHasGroups.Add(new PathWayHasGroup()
            {
                PathWayId = PathWayId,
                GroupId = GroupId
            });

            await GroupService.Update(Group);
            await LoadGuid();

            StateHasChanged();
        }
        #endregion

        #region Delete Group
        private async Task DeleteGroup(Guid GroupId)
        {
            PathWayHasGroup = new()
            {
                PathWayId = PathWayId,
                GroupId = GroupId
            };

            await PathWayService.RemovePathWayHasGroup(PathWayHasGroup);
            await LoadGuid();

            StateHasChanged();
        }
        #endregion


        protected void OnAcceptDialogClose(bool accepted)
        {
            if (accepted)
            {
                NavigationManager.NavigateTo("pathlistoverview");
            }
            AcceptDialogOpen = false;
            StateHasChanged();
        }

        #region ShowDeleteModal
        private void OpenAcceptDialog()
        {
            AcceptDialogOpen = true;
            StateHasChanged();
        }
        #endregion
    }
}
