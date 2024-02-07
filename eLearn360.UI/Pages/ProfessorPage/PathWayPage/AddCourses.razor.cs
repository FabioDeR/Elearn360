using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace eLearn360.UI.Pages.ProfessorPage.PathWayPage
{
    public partial class AddCourses
    {
        [Parameter] public Guid PathWayId { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public IPathWayService PathWayService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }

        public List<Guid> ListCourseId = new();
        public List<Course> PublicCourses { get; set; } = new();
        public List<Course> PrivateCourses { get; set; } = new();
        public PathWay PathWay { get; set; } = new();
        protected PathWayHasCourse PathWayHasCourse { get; set; } = new();
        private int position = 0;
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            await LoadList();
            _isLoading = false;
        }

        private async Task LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var userId = user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            PublicCourses = await CourseService.GetPublicCourse(Guid.Parse(userId));
            PrivateCourses = await CourseService.GetPrivateCourse(Guid.Parse(userId));
            PathWay = await PathWayService.GetById(PathWayId);
            await LoadGuidAndPosition();
            StateHasChanged();
        }

        private async Task LoadGuidAndPosition()
        {
            ListCourseId = await PathWayService.GetCourseGuid(PathWayId);
            position = ListCourseId.Count;
        }

        #region Add Course
        private async Task AddCourse(Guid Id)
        {
            try
            {
                position++;
                PathWay.PathWayHasCourses = new();
                PathWay.PathWayHasCourses.Add(new PathWayHasCourse()
                {
                    PathWayId = PathWayId,
                    CourseId = Id,
                    Position = position
                });
                await PathWayService.Update(PathWay);
                await LoadGuidAndPosition();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        #endregion

        #region Delete Course
        private async Task DeleteCourse(Guid courseId)
        {
            try
            {
                PathWayHasCourse = new()
                {
                    PathWayId = PathWayId,
                    CourseId = courseId
                };
                await PathWayService.RemovePathWayHasCourse(PathWayHasCourse);
                await LoadList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
