using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.ProfessorPage.LessonPage
{
    public partial class LessonEditPage
    {
        [Inject] public ILessonService LessonService { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public Guid Id { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        public Lesson LessonData { get; set; } = new();
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
            if (Id != new Guid())
            {
                var itemId = Id;
                LessonData = await LessonService.GetById(itemId);
            }
            var txt = await Task.Run(() => 1);
        }

        #region HandleValidRequest
        protected async Task HandleValidRequest()
        {
            if (Id == new Guid()) // We need to add the item
            {                   
                var res = await LessonService.Add(LessonData);

                if (res != null)
                {                    
                    LessonData = new Lesson();
                    NavigationManager.NavigateTo("/lessonlistoverview");
                }
            }
            else // We are updating the item
            {
                await LessonService.Update(LessonData);
                NavigationManager.NavigateTo("/lessonlistoverview");
            }
        }
        #endregion
    }
}
