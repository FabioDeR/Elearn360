using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace eLearn360.UI.Pages.ProfessorPage.CoursePage
{
    public partial class CourseEditPage
    {
        [Parameter] public Guid Id { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public ISectionService SectionService { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public ILevelService LevelService { get; set; }  
     
        public List<Category> CategoryList { get; set; } = new();
        public List<Level> LevelList { get; set; } = new();
        public Course CourseData { get; set; } = new();        
   
        public Guid CourseId { get; set; }

        protected string ImgUrl { get; set; }
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

        private async Task LoadList()
        {
            CategoryList = (await CategoryService.GetAll()).ToList();
            LevelList = (await LevelService.GetAll()).ToList();
            if (Id != new Guid())
            {
                var itemId = Id;
                CourseData = await CourseService.GetById(itemId);
                ImgUrl = CourseData.ImageUrl;
                CourseId = CourseData.Id;

            }
        }

        #region SelectedImage
        private async void SelectedImage(InputFileChangeEventArgs e)
        {
            var imageFiles = e.File;

            if (imageFiles != null)
            {
                var resizedFile = await imageFiles.RequestImageFileAsync("image/png", 300, 300);

                using var ms = resizedFile.OpenReadStream(resizedFile.Size);
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", imageFiles.Name);
                ImgUrl = await CourseService.UploadCourseImage(content);
                CourseData.ImageUrl = ImgUrl;
                StateHasChanged();
            }
        }
        #endregion
        #region HandleValidRequest
        protected async Task HandleValidRequest()
        {
            if (Id == new Guid()) // We need to add the item
            {                         
                    Guid newId = await CourseService.Add(CourseData);
                    CourseId = newId;                
            }
            else // We are updating the item
            {                
                await CourseService.Update(CourseData);               
            }
            ShowAddSections();
        }
        #endregion
        #region Tabs
        protected bool IsShowCreateCourse = true;
        protected bool IsInactive = true;
        protected bool IsShowAddSections = false;
        protected bool IsShowOrganizeSections = false;
        protected string InactiveClassAdd = "inactive";
        protected string InactiveClassOrganize = "inactive";

        protected void ShowCreateCourse()
        {
            IsShowCreateCourse = true;
            IsShowAddSections = false;
            IsShowOrganizeSections = false;
        }

        protected void ShowAddSections()
        {
            IsShowCreateCourse = false;
            IsShowAddSections = true;
            IsShowOrganizeSections = false;
            IsInactive = false;
            InactiveClassAdd = "";
        }

        protected void ShowOrganizeSections()
        {
            IsShowCreateCourse = false;
            IsShowAddSections = false;
            IsShowOrganizeSections = true;
            InactiveClassOrganize = "";
        }
        #endregion
       
    }
}
