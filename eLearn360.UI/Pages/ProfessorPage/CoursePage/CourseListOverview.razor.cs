using eLearn360.Service.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.ProfessorPage.CoursePage
{
    public partial class CourseListOverview
    {

        [Inject] public ICourseService CourseService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<Course> PrivateCourses { get; set; } = new();
        public List<Course> PublicCourses { get; set; } = new();
        public Guid UserId { get; set; }

        protected bool _isLoading = false;

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
            PublicCourses = await CourseService.GetPublicCourse(Guid.Parse(userId));
            PrivateCourses = await CourseService.GetPrivateCourse(Guid.Parse(userId));
            UserId = Guid.Parse(userId);
            StateHasChanged();
        }
        #region Duplicate
        protected async Task Duplicate(Guid courseid)
        {
            await CourseService.DuplicateCourse(courseid, UserId);
            await LoadList();
        }
        #endregion
    }
}
