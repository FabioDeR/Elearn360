using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.AdminPage.StaffOccupationPage
{
    public partial class StaffOccupationEO
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public IOrganizationService OrganizationService { get; set; }
        [Inject] public IStaffHasOccupationService StaffHasOccupationService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid StaffOccupationId { get; set; }

        protected List<StaffOccupation> StaffOccupationList { get; set; } = new();
        protected StaffOccupation StaffOccupation { get; set; } = new();
        protected Organization Organization { get; set; } = new();
        protected Guid OrganizationId { get; set; } = new();
        protected Guid OccupationId { get; set; } = new();

        private bool _isLoading = false;

        //Modal
        public bool AcceptDialogOpen { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = (await AuthenticationStateTask).User;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                _isLoading = true;
                await LoadList();
                _isLoading = false;
            }
        }

        private async Task LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var OrgaId = user.FindFirst(e => e.Type == "OrganizationId").Value;
            OrganizationId = Guid.Parse(OrgaId);
            Organization = await OrganizationService.GetById(OrganizationId);
            StaffOccupationList = await StaffHasOccupationService.GetByOrganizationId(OrganizationId);
        }

        protected async Task HandleValidRequest()
        {
            if (StaffOccupationId == new Guid()) // Add
            {
                StaffOccupation.OrganizationId = OrganizationId;
                var res = await StaffHasOccupationService.Add(StaffOccupation);

                if (res.IsSuccessStatusCode)
                {                    
                    StaffOccupation = new ();
                    StaffOccupationId = new();
                    await LoadList();
                }
            }
            else // update
            {
                var res = await StaffHasOccupationService.Update(StaffOccupation);
                StaffOccupationId = new();
                if (res.IsSuccessStatusCode)
                {                    
                    StaffOccupation = new();
                    await LoadList();
                    StateHasChanged();
                }
            }
        }

        #region Update
        protected async void Update(Guid staffOccupationId)
        {
            NavigationManager.NavigateTo($"staffoccupationeo/{staffOccupationId}");
            StaffOccupation = await StaffHasOccupationService.GetById(staffOccupationId);
            StateHasChanged();
        }
        #endregion

        #region Delete
        protected async void OnAcceptDialogClose(bool accepted)
        {
            if (accepted)
            {
                var res = await StaffHasOccupationService.Delete(OccupationId);
                if (res.IsSuccessStatusCode)
                {                    
                    await LoadList();
                }
            }
            AcceptDialogOpen = false;
            StateHasChanged();
        }
        #endregion

        #region ShowDeleteModal
        private void OpenAcceptDialog(Guid occupationId)
        {
            OccupationId = occupationId;
            AcceptDialogOpen = true;
            StateHasChanged();
        }
        #endregion
    }
}
