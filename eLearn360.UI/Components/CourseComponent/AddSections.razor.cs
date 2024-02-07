using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Components.CourseComponent
{
    public partial class AddSections
    {
        [CascadingParameter] protected Guid CourseId { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public ISectionService SectionService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }

        public List<Guid> ListSectionId = new();
        public List<Section> PublicSections { get; set; } = new();
        public List<Section> PrivateSections { get; set; } = new();
        public Course Course { get; set; } = new();
        public CourseHasSection CourseHasSection { get; set; }

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
            PublicSections = await SectionService.GetPublicSection(Guid.Parse(userId));
            PrivateSections = await SectionService.GetPrivateSection(Guid.Parse(userId));
            Course = await CourseService.GetById(CourseId);
            await LoadGuidAndPosition();
            StateHasChanged();
        }

        private async Task LoadGuidAndPosition()
        {
            ListSectionId = await CourseService.GetSectionGuid(CourseId);
            position = ListSectionId.Count;
        }

        #region AddLesson
        private async Task AddSection(Guid Id)
        {
            try
            {
                position++;
                Course.CourseHasSections = new();
                Course.CourseHasSections.Add(new CourseHasSection()
                {
                    CourseId = CourseId,
                    SectionId = Id,
                    Position = position
                });
                await CourseService.Update(Course);
                await LoadGuidAndPosition();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        #endregion

        #region Delete Section
        private async Task DeleteSection(Guid sectionId)
        {
            try
            {
                CourseHasSection = new()
                {
                    CourseId = CourseId,
                    SectionId = sectionId
                };
                await CourseService.RemoveCourseHasSection(CourseHasSection);
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
