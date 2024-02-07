using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.ProfessorPage.GroupPage
{
    public partial class MyGroupListOverview
    {
        [Inject] IGroupService GroupService { get; set; }
        [Inject] IOrganizationService OrganizationService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<Group> Groups { get; set; } = new();
        public Guid OrganizationId { get; set; } = Guid.Empty;
        protected Guid GroupId { get; set; } = new();
        protected Guid UserId { get; set; } = new();
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
            var user = (await AuthenticationStateTask).User;
            var userId = user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            UserId = Guid.Parse(userId);
            var orgaIdToken = user.FindFirst(e => e.Type == "OrganizationId").Value;
            OrganizationId = Guid.Parse(orgaIdToken);
            Groups = await GroupService.GetMyGroups(UserId, OrganizationId);
        }
    }
}
