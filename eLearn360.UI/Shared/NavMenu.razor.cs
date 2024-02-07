using eLearn360.Data.VM.AccountVM;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IAuthService AuthService { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }
        public List<Organization> OrganizationList { get; set; } = new();
        public RefreshTokenVM RefreshTokenVM { get; set; } = new();
        public string OrganizationIdChoise { get; set; } = string.Empty;
        protected Guid OrgaId = new();
        protected Guid UserId = new();
        protected string StringUserId { get; set; } = string.Empty;
        private bool _isLoading = false;
        protected Guid UserChoice = new();
        private string filter = string.Empty;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authenticationState = (await AuthenticationStateTask).User;
                if (!authenticationState.Identity.IsAuthenticated)
                {
                    OrganizationList.Clear();
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    LoadList();
                }
            }
            else
            {
                var authenticationState = (await AuthenticationStateTask).User;
                if (!authenticationState.Identity.IsAuthenticated)
                {
                    OrganizationList.Clear();
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    if (OrganizationList.Count == 0)
                    {
                        LoadList();
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected async void LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var userId = user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            UserId = Guid.Parse(userId);
            var orgaIdToken = user.FindFirst(e => e.Type == "OrganizationId")?.Value;
            OrganizationList = await UserService.GetAllOrganizationByUserId(UserId);
            OrganizationIdChoise = orgaIdToken;
            OrgaId = Guid.Parse(OrganizationIdChoise);
            StringUserId = userId;
            StateHasChanged();
        }

        public async Task OrgnizationSelected(string e)
        {
            OrganizationIdChoise = e;
            RefreshTokenVM refreshTokenVM = new()
            {
                OrganizationId = Guid.Parse(OrganizationIdChoise),
                UserId = Guid.Parse(StringUserId),
            };
            await AuthService.RefreshTokenAuthorize(refreshTokenVM);
            NavigationManager.NavigateTo("/", forceLoad: true);
        }

        protected void HandleSearch()
        {
            if (filter != string.Empty && filter.Length > 2)
            {
                NavigationManager.NavigateTo($"search/{filter}");
                filter = string.Empty;
            }

        }
    }
}
