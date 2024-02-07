using eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM;
using eLearn360.Data.VM.Policies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.OrganizationPage
{
    public partial class UserOrganizationAdd
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] IOrganizationService OrganizationService { get; set; }
        [Inject] IGenderService GenderService { get; set; }
        [Inject] IOccupationService OccupationService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }
        [Parameter] public Guid OrganizationId { get; set; }

        [Inject] IAuthenticationService AuthenticationService { get; set; }

        public UserHasOccupationVM UserHasOccupationVM { get; set; }
        public Organization Organization { get; set; }
        public List<Gender> GenderList { get; set; }
        public List<Occupation> OccupationList { get; set; }
        public List<Guid> UserOccupationList { get; set; }
        [Inject]
        private IAuthorizationService AuthorizationService { get; set; }
        private bool _isLoading;
        private bool _genderFormError;
        private string _genderFormValidClass;
        private bool _occupationFormError;
        private string organizationName;

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

                #region Simulate Required field
                // Simulate Required field : Gender
                _genderFormError = false;
                _genderFormValidClass = string.Empty;
                // Simulate Required field : Occupation
                _occupationFormError = false;
                #endregion

                organizationName = Organization.Name;
            }
            _isLoading = false;
        }

        protected async Task LoadList()
        {
            UserHasOccupationVM = new();
            GenderList = new();
            OccupationList = new();
            UserOccupationList = new();
            Organization = new();

            try
            {
                var user = (await AuthenticationStateTask).User;
                GenderList = (await GenderService.GetAll()).ToList();
                OccupationList = (await OccupationService.GetAll()).ToList();
                if ((await AuthorizationService.AuthorizeAsync(user, Policies.IsAdmin)).Succeeded)
                {
                    OccupationList.RemoveAll(o => o.NormalizedName == "SuperAdmin");
                    StateHasChanged();
                }
                Organization = await OrganizationService.GetById(OrganizationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region GenderSelect
        protected void GenderSelect(ChangeEventArgs e)
        {
            UserHasOccupationVM.GenderId = Guid.Parse(e.Value.ToString());
            // Simulate Required field : Gender
            _genderFormError = false;
            _genderFormValidClass = "modified valid";
        }
        #endregion

        protected async Task HandleRequest()
        {
            if (UserOccupationList.Count == 0)
            {
                _occupationFormError = true;
            }
            else
            {
                _isLoading = true;
                UserHasOccupationVM.OccupationId = new List<Guid>(UserOccupationList);

                UserHasOccupationVM.OrganizationId = OrganizationId;

                var res = await OrganizationService.AddNewUserAndOccupationByOrganizationId(UserHasOccupationVM);

                if (res.IsSuccessStatusCode)
                {
                    Toaster.Success("Utilisateur ajouté !");
                    UserHasOccupationVM = new();
                    UserOccupationList = new();
                }
                else
                {
                    Toaster.Warning("Erreur pendant la modification");
                }
                _isLoading = false;
            }
        }

    }
}
