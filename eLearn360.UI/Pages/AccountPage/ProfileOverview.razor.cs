using eLearn360.Data.VM;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data;
using System.Security.Claims;

namespace eLearn360.UI.Pages.AccountPage
{
    public partial class ProfileOverview
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public string Role { get; set; }
        [Parameter] public Guid GroupId { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IAuthService AuthService { get; set; }
        [Inject] public IGenderService GenderService { get; set; }
        
        [Parameter] public Guid UserId { get; set; }

        public AccountRegisterEditVM Item { get; set; } = new();
        public string GenderString { get; set; } = string.Empty;
        public bool DeleteDialogOpen { get; set; }

        protected Guid OrganizationId = Guid.Empty;
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            var authenticationState = (await AuthenticationStateTask).User;
            var orgaId = authenticationState.FindFirst(e => e.Type == "OrganizationId").Value;
            OrganizationId = Guid.Parse(orgaId);
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
            Item = await UserService.GetUserProfile(UserId);
            GenderString = (await GenderService.GetAll()).Where(x => x.DeleteDate == null && x.Id == Item.GenderId).Select(x => x.Name).FirstOrDefault();
            if (Item.ImageUrl == null)
            {
                Item.ImageUrl = "/image/DefaultImageUser.png";
            }
            StateHasChanged();
        }

        #region ResetPassword
        public async Task ResetPassword()
        {
            var res = await AuthService.ResetPasswordByGuid(Item.Id);
        }
        #endregion

        #region Modal
        protected async Task OnDeleteDialogClose(bool accepted)
        {
            if (accepted)
            {
                await ResetPassword();
            }

            DeleteDialogOpen = false;
            StateHasChanged();
        }

        protected void OpenDeleteDialog()
        {
            DeleteDialogOpen = true;
            StateHasChanged();
        }
        #endregion

        #region Redirect
        protected void CheckRole()
        {
            switch (Role)
            {
                case "Teacher":
                    NavigationManager.NavigateTo($"studentlistoverview/{GroupId}");
                    break;
                case "AdminGroup":
                    NavigationManager.NavigateTo($"usergrouplistoverview/{GroupId}");
                    break;
                case "AdminOrga":
                    NavigationManager.NavigateTo($"userorganizationlistoverview/{OrganizationId}");
                    break;
            }
        }
        #endregion

    }
}
