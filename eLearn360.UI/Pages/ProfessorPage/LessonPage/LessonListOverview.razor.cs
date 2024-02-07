using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.ProfessorPage.LessonPage
{
    public partial class LessonListOverview
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILessonService LessonService { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public List<Lesson> PrivateLessons { get; set; } = new();
        public List<Lesson> PublicLessons { get; set; } = new();
        protected Guid UserId { get; set; }

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


        #region LoadList
        private async Task LoadList()
        {
            var user = (await AuthenticationStateTask).User;
            var userId = user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            PublicLessons = await LessonService.GetPublicLesson(Guid.Parse(userId));
            PrivateLessons = await LessonService.GetPrivateLesson(Guid.Parse(userId));
            UserId = Guid.Parse(userId);
        }
        #endregion

        #region Duplicate
        protected async Task Duplicate(string lessonId)
        {
            var rep = await LessonService.DuplicateLesson(Guid.Parse(lessonId), UserId);
            if (rep.IsSuccessStatusCode)
            {
                await LoadList();
            }
        }
        #endregion
    }
}
