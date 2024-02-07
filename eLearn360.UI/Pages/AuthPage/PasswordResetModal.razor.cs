using System.Net.Mail;

namespace eLearn360.UI.Pages.AuthPage
{
    public partial class PasswordResetModal
    {
        [Inject] public IAuthService AuthService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        private bool _isLoading { get; set; } = false;


        protected override void OnInitialized()
        {

        }

        private Task ModalCancel()
        {
            return OnClose.InvokeAsync(false);
        }

        private async Task<Task> ModalOk()
        {
            try
            {
                _isLoading = true;
                var res = await AuthService.ResetPasswordByMail(Email);

                Toaster.Success($"Un mail a été envoyé à {Email} avec un nouveau mot de passe");
                return OnClose.InvokeAsync(true);

            }
            catch (Exception e)
            {
                _isLoading = false;
                Console.WriteLine(e);
                throw;
            }
        }

        public void IsValidEmail(string emailaddress)
        {
            try
            {
                Message = "";
                if (!String.IsNullOrWhiteSpace(emailaddress))
                {
                    MailAddress m = new MailAddress(emailaddress);
                    ModalOk();
                }
                else
                {
                    Message = "Le format d'adresse mail est incorrect";
                }
            }
            catch (FormatException)
            {
                Message = "Le format d'adresse mail est incorrect";
            }
        }
    }
}
