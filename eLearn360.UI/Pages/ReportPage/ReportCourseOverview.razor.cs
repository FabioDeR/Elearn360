using eLearn360.Data.VM;
using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.ReportPage
{
    public partial class ReportCourseOverview
    {
        [Inject] public IReportService ReportService { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Parameter] public Guid UserId { get; set; }
        [Parameter] public Guid GroupId { get; set; }
        [Parameter] public string Role { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public LessonReportVM LessonReportVM { get; set; } = new();

        public string Username { get; set; } = string.Empty;
        protected string roleName = string.Empty;
        protected string redirectUri = string.Empty;
        private bool _isLoading = false;


        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            var authenticationState = (await AuthenticationStateTask).User;
            Username = authenticationState.Identity.Name;
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
            LessonReportVM = await ReportService.StudentCourseReport(UserId, GroupId);
        }

        protected void CheckRole()
        {
            switch (Role)
            {
                case "Student":
                    NavigationManager.NavigateTo("/");
                    break;                
                case "Teacher":
                    NavigationManager.NavigateTo($"studentlistoverview/{GroupId}");
                    break;                
                case "Admin":
                    NavigationManager.NavigateTo($"usergrouplistoverview/{GroupId}");
                    break;
            }
        }
    }
}
