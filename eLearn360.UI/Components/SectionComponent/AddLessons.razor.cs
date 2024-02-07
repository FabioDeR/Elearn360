using eLearn360.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Components.SectionComponent
{
    public partial class AddLessons
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [CascadingParameter] protected Guid SectionId { get; set; }
        [Inject] public ILessonService LessonService { get; set; }
        [Inject] public ISectionService SectionService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }

        public List<Guid> ListLessonId = new();
        public List<Lesson> PublicLessons { get; set; } = new();
        public List<Lesson> PrivateLessons { get; set; } = new();
        public Section Section { get; set; } = new();
        public SectionHasLesson SectionHasLesson { get; set; } = new();

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
            PublicLessons = await LessonService.GetPublicLesson(Guid.Parse(userId));
            PrivateLessons = await LessonService.GetPrivateLesson(Guid.Parse(userId));
            Section = await SectionService.GetById(SectionId);
            await LoadGuidAndPosition();
            StateHasChanged();
        }

        private async Task LoadGuidAndPosition()
        {
            ListLessonId = await SectionService.GetLessonGuid(SectionId);
            position = ListLessonId.Count;
        }

        #region AddLesson
        private async Task AddLesson(Guid Id)
        {
            try
            {
                position++;
                Section.SectionHasLessons = new();
                Section.SectionHasLessons.Add(new SectionHasLesson()
                {
                    SectionId = SectionId,
                    LessonId = Id,
                    Position = position
                });
                await SectionService.Update(Section);
                await LoadGuidAndPosition();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        #endregion

        #region Delete Lesson
        private async Task DeleteLesson(Guid lessonId)
        {
            try
            {
                SectionHasLesson = new()
                {
                    SectionId = SectionId,
                    LessonId = lessonId
                };
                await SectionService.RemoveSectionHasLesson(SectionHasLesson);
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
