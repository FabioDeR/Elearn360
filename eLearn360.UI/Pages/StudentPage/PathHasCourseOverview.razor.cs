using Microsoft.AspNetCore.Components.Authorization;
using Sotsera.Blazor.Toaster;
using System.Security.Claims;

namespace eLearn360.UI.Pages.StudentPage
{
    public partial class PathHasCourseOverview
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public IPathWayService PathWayService { get; set; }
        [Inject] public ILessonService LessonService { get; set; }
        [Parameter] public Guid PathId { get; set; }

        public List<Course> Courses { get; set; } = new();
        public PathWay PathWay { get; set; } = new();
        protected Guid UserId { get; set; } = new();

        private bool _isLoading = false;

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
            PathWay = await PathWayService.GetById(PathId);
            Courses = await CourseService.GetCourseByPathAndUserIdWithHistoricCourse((PathId), Guid.Parse(userId));
        }

        protected void GoToQuizz()
        {
            NavigationManager.NavigateTo($"/student/quizzedit/{PathId}");
        }

        protected async Task PostStartHistoric(Guid pathId, Guid courseId)
        {
            try
            {
                var res = await LessonService.PostStartHistoric(UserId, pathId);
                NavigationManager.NavigateTo($"viewcourse/{pathId}/{courseId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
