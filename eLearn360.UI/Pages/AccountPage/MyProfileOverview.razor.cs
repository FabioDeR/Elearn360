using eLearn360.Data.VM;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.AccountPage
{
    public partial class MyProfileOverview
    {
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IGenderService GenderService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        public AccountRegisterEditVM Item { get; set; } = new();
        public string GenderString { get; set; } = string.Empty;
        private bool _isLoading = false;
        protected Guid UserId = Guid.Empty;

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
            Item = await UserService.GetUserProfile(UserId);
            if (Item.ImageUrl == "image par défaut")
            {
                Item.ImageUrl = "/image/DefaultImageUser.png";
            }
            GenderString = (await GenderService.GetAll()).Where(x => x.DeleteDate == null && x.Id == Item.GenderId).Select(x => x.Name).FirstOrDefault();
        }
    }
}
