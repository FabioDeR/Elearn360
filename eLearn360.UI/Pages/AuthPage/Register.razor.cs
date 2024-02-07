using eLearn360.Data.VM.AccountVM;

namespace eLearn360.UI.Pages.AuthPage
{
    public partial class Register
    {
        [Inject] public IAuthService AuthService { get; set; }
        [Inject] public IGenderService GenderService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        public RegisterVM User { get; set; } = new();
        public List<Gender> GenderList { get; set; } = new();
        private bool _loading;
        private bool _genderFormError;
        private string _genderFormValidClass;

        protected async override Task OnInitializedAsync()
        {
            _loading = true;
            GenderList = (await GenderService.GetAll()).ToList();

            // Simulate Required field : Gender
            _genderFormError = false;
            _genderFormValidClass = string.Empty;

            _loading = false;
        }

        #region GenderSelect
        protected void GenderSelect(ChangeEventArgs e)
        {
            User.GenderId = Guid.Parse(e.Value.ToString());
            // Simulate Required field : Gender
            _genderFormError = false;
            _genderFormValidClass = "modified valid";
        }
        #endregion

        #region HandleValidRequest
        protected async Task HandleValidRequest()
        {
            if (User.GenderId != new Guid()) // Required field : Gender
            {
                RegisterResultVM result = await AuthService.Register(User);

                if (result.Success)
                {
                    NavigationManager.NavigateTo("/login");
                }

            }
            else
            {
                _genderFormError = true;
            }
        }
        #endregion
    }
}
