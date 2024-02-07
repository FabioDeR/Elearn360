using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.ProfessorPage.SectionPage
{
    public partial class SectionEditPage
    {
        [Inject] public ISectionService SectionService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter] public Guid Id { get; set; }
        [Parameter] public string EditorValue { get; set; }

        public Section SectionData { get; set; } = new();
        public Guid SectionId { get; set; }

        private bool _isLoading = false;

        protected async override Task OnInitializedAsync()
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
            if (Id != new Guid())
            {
                Guid itemId = Id;
                SectionData = await SectionService.GetById(itemId);
                SectionId = SectionData.Id;
            }
        }

        #region Tabs

        protected bool IsShowCreateSection = true;
        protected bool IsInactive = true;
        protected bool IsShowAddLessons = false;
        protected bool IsShowOrganizeLessons = false;
        protected string InactiveClassAdd = "inactive";
        protected string InactiveClassOrganize = "inactive";


        public void ShowCreateSection()
        {
            IsShowCreateSection = true;
            IsShowAddLessons = false;
            IsShowOrganizeLessons = false;
        }

        public void ShowAddLessons()
        {
            IsShowCreateSection = false;
            IsShowAddLessons = true;
            IsShowOrganizeLessons = false;
            IsInactive = false;
            InactiveClassAdd = "";
        }

        public void ShowOrganizeLessons()
        {
            IsShowCreateSection = false;
            IsShowAddLessons = false;
            IsShowOrganizeLessons = true;
            InactiveClassOrganize = "";
        }
        #endregion

        #region HandleValidRequest
        protected async Task HandleValidRequest()
        {
            if (Id == new Guid()) // We need to add the item
            {                    
                    Guid id = await SectionService.Add(SectionData);
                    SectionId = id;                   
                    ShowAddLessons();           
               
            }
            else // We are updating the item
            {
                SectionData.Content = EditorValue;
                await SectionService.Update(SectionData);                
                ShowAddLessons();
            }
        }
        #endregion
    }
}
