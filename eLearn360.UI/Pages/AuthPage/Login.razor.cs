using eLearn360.Data.VM.AccountVM;

namespace eLearn360.UI.Pages.AuthPage
{
    public partial class Login
    {
        [Inject] public IAuthService AuthService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        public LoginVM User { get; set; }

        private bool _isLoading = false;
        protected string message = string.Empty;

        //Modal
        public bool AcceptDialogOpen { get; set; }

        protected override void OnInitialized()
        {
            User = new();
        }

        // MODAL
        private void OnAcceptDialogClose(bool accepted)
        {
            if (accepted)
            {
                //NavigationManager.NavigateTo($"usergrouplistoverview/{GroupId}");
            }

            AcceptDialogOpen = false;
            StateHasChanged();
        }

        private void OpenAcceptDialog()
        {
            AcceptDialogOpen = true;
            StateHasChanged();
        }

        protected async Task HandleValidRequest()
        {
            _isLoading = true;
            LoginResultVM result = await AuthService.Login(User);
            _isLoading = false;

            if (result.Success)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                message = "● Email et/ou mot de passe incorrect !";
            }
        }
    }
}
