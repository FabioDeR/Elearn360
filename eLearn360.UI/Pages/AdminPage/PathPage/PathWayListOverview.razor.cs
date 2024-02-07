using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.AdminPage.PathPage
{
    public partial class PathWayListOverview
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] public IPathWayService PathWayService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<PathWay> PathWayList { get; set; } = new();
        protected Guid OrganizationId { get; set; } = new();
        protected Guid UserId { get; set; } = new();

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
                await LoadList();
                _isLoading = false;
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

            PathWayList = (await PathWayService.GetPathByOrganizationId(OrganizationId, UserId)).ToList();

            if (PathWayList.Count == 0)
            {
                _message = "Vous n'avez pas encore de parcours enregistré";
            }
            StateHasChanged();
        }

        #region SoftDelete
        protected async void Delete(Guid pathwayid)
        {
            var res = await PathWayService.Delete(pathwayid);

            if (res.IsSuccessStatusCode)
            {
                await LoadList();
            }
        }
        #endregion
    }
}
