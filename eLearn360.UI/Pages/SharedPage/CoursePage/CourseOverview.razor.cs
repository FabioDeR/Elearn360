using eLearn360.Data.VM.CourseVM;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace eLearn360.UI.Pages.SharedPage.CoursePage
{
    public partial class CourseOverview
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public string HistoNav { get; set; }
        [Parameter] public Guid PathWayId { get; set; }
        [Parameter] public Guid CourseId { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public ILessonService LessonService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }


        protected Course CourseMenu { get; set; } = new();
        protected ViewContentVM ViewContentVM { get; set; } = new();
        protected List<Guid> GuidList { get; set; } = new();

        private bool _isLoading = false;

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
                ItemId = CourseId;
                _isLoading = false;
            }
        }

        #region LoadList
        protected async Task LoadList()
        {
            CourseMenu = await CourseService.GetCourseTolesson(CourseId);
            GuidList = await CourseService.GetAllGuidByCourseId(CourseId);
            ViewContentVM = await CourseService.GetViewContentById(GuidList[currentPosition]);
            StateHasChanged();
        }
        #endregion

        #region SelectItem
        protected async Task SelectItem(Guid guidItem)
        {
            ViewContentVM = await CourseService.GetViewContentById(guidItem);
            currentPosition = GuidList.FindIndex(x => x.Equals(guidItem));
            actualItemId = guidItem;
            ItemId = actualItemId;
        }
        #endregion

        #region Toggle SideBar
        private bool isInactive = false;
        private string CollapseMenu => isInactive ? "active" : null;
        private void ToggleNavMenu()
        {
            isInactive = !isInactive;
        }
        #endregion

        #region Previous / Next / Finish

        public Guid actualItemId = new();
        public int currentPosition = 0; //Position actuelle
        public Guid ItemId = new();

        protected async Task Previous(Guid itemId)
        {
            currentPosition = GuidList.FindIndex(x => x.Equals(itemId));
            currentPosition--;
            actualItemId = GuidList[currentPosition];
            ItemId = actualItemId;

            ViewContentVM = await CourseService.GetViewContentById(ItemId);

            StateHasChanged();
        }

        protected async Task Next(Guid itemId)
        {
            currentPosition = GuidList.FindIndex(x => x.Equals(itemId));

            if (currentPosition == GuidList.Count - 1)
            {
                Finish();
                return;
            }
            else
            {
                ItemId = itemId;

                currentPosition++;
                actualItemId = GuidList[currentPosition];
                ItemId = actualItemId;
                ViewContentVM = await CourseService.GetViewContentById(ItemId);
            }

            StateHasChanged();
        }

        protected void Finish()
        {
            switch (HistoNav)
            {
                case "courselist":
                    NavigationManager.NavigateTo("courselistoverview");
                    break;
                case "orgapathadmin":
                    NavigationManager.NavigateTo($"organizepath/{PathWayId}/Admin");
                    break;
                case "orgapathteacher":
                    NavigationManager.NavigateTo($"organizepath/{PathWayId}/Teacher");
                    break;
                case "addcourses":
                    NavigationManager.NavigateTo($"/addcourses/{PathWayId}");
                    break;
            }
        }
        #endregion
    }
}
