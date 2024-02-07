using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.ProfessorPage.SectionPage
{
    public partial class SectionListOverview
    {
        [Inject] public ISectionService SectionService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<Section> PrivateSection { get; set; } = new();
        public List<Section> PublicSection { get; set; } = new();
        public Guid UserId { get; set; }

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

        private async Task LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var userId = user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            PublicSection = await SectionService.GetPublicSection(Guid.Parse(userId));
            PrivateSection = await SectionService.GetPrivateSection(Guid.Parse(userId));
            UserId = Guid.Parse(userId);
            StateHasChanged();
        }

        #region Duplicate
        protected async Task Duplicate(Guid sectionId)
        {
            var rep = await SectionService.DuplicateSection(sectionId, UserId);
            if (rep.IsSuccessStatusCode)
            {                
                await LoadList();
            }
        }
        #endregion
    }
}
