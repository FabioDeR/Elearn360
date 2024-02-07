using eLearn360.Data.Models;
using eLearn360.Data.VM.CourseVM;
using Microsoft.AspNetCore.Components.Authorization;
using Sotsera.Blazor.Toaster;
using System.Security.Claims;

namespace eLearn360.UI.Pages.StudentPage
{
    public partial class ViewCourse
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public ILessonService LessonService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid PathId { get; set; }
        [Parameter] public Guid CourseId { get; set; }

        protected Course CourseMenu { get; set; } = new();
        protected ViewContentVM ViewContentVM { get; set; } = new();
        protected List<Guid> GuidList { get; set; } = new();
        protected List<Guid> GuidSeenList { get; set; } = new();

        protected string userId = string.Empty;

        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = (await AuthenticationStateTask).User;
            userId = authenticationState.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                _isLoading = true;
                await LoadList();
                await PostStartHistoric(CourseId);
                ItemId = CourseId;
                _isLoading = false;
            }
        }

        #region LoadList
        protected async Task LoadList()
        {
            CourseMenu = await CourseService.GetCourseTolesson(CourseId);
            GuidList = await CourseService.GetAllGuidByCourseId(CourseId);
            GuidSeenList = await CourseService.GetAllGuidSeen(CourseId, Guid.Parse(userId));
            ViewContentVM = await CourseService.GetViewContentById(GuidList[currentPosition]);
            StateHasChanged();
        }
        #endregion

        #region SelectItem
        protected async Task SelectItem(Guid guidItem)
        {
            if (ItemId != GuidList[0])
            {
                await PostEndHistoric(ItemId);
                await CheckItemState(guidItem);
            }

            ViewContentVM = await CourseService.GetViewContentById(guidItem);
            currentPosition = GuidList.FindIndex(x => x.Equals(guidItem));
            actualItemId = guidItem;
            ItemId = actualItemId;

            await PostStartHistoric(actualItemId);
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

                await PostEndHistoric(ItemId);
                await CheckItemState(itemId);

                currentPosition++;
                actualItemId = GuidList[currentPosition];
                ItemId = actualItemId;
                ViewContentVM = await CourseService.GetViewContentById(ItemId);

                await PostStartHistoric(ItemId);
            }

            StateHasChanged();
        }

        protected async void Finish()
        {
            await PostEndHistoric(ItemId);
            NavigationManager.NavigateTo($"pathhascourseoverview/{PathId}");
        }
        #endregion

        #region Historic
        protected async Task PostStartHistoric(Guid itemId)
        {
            try
            {
                var res = await LessonService.PostStartHistoric(Guid.Parse(userId), itemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        protected async Task PostEndHistoric(Guid itemId)
        {
            try
            {
                var res = await LessonService.PostEndHistoric(Guid.Parse(userId), itemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }        
        
        protected async Task CheckItemState(Guid itemid)
        {
            IsItemSeen(itemid);
            GuidSeenList = await CourseService.GetAllGuidSeen(CourseId, Guid.Parse(userId));
            StateHasChanged();
        }

        protected bool IsItemSeen(Guid itemid)
        {
            if (GuidSeenList.Contains(itemid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion



    }
}
