using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace eLearn360.UI.Pages.AdminPage.PathPage
{
    public partial class PathWayEdit
    {
        [Inject] public IPathWayService PathService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public ILevelService LevelService { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        public Guid PathWayId { get; set; }

        protected string ImgUrl { get; set; }
        public string EditorValue { get; set; }
        public string StaffId { get; set; }

        public PathWay PathData { get; set; } = new();
        public List<Category> CategoryList { get; set; } = new();
        public List<Level> LevelList { get; set; } = new();

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
                await LoadCategoryList();
                await LoadLevelList();
                if (Id != new Guid())
                {
                    var itemId = Id;
                    PathData = await PathService.GetById(itemId);
                    EditorValue = PathData.Content;
                    ImgUrl = PathData.ImageUrl;
                    PathWayId = PathData.Id;
                }
            }
            _isLoading = false;
        }

        #region HandleValidRequest
        protected async Task HandleValidRequest()
        {
            if (Id == new Guid()) // We need to add the item
            {
                var newId = await PathService.Add(PathData);
                PathWayId = newId;
                NavigationManager.NavigateTo($"linkpathtogroup/{PathWayId}");
            }
            else // We are updating the item
            {
                await PathService.Update(PathData);
                NavigationManager.NavigateTo($"linkpathtogroup/{PathWayId}");
            }
        }
        #endregion


        #region LoadCategoryList
        public async Task LoadCategoryList()
        {
            CategoryList = (await CategoryService.GetAll()).ToList();
        }
        #endregion

        #region LoadLevelList
        public async Task LoadLevelList()
        {
            LevelList = (await LevelService.GetAll()).ToList();
        }
        #endregion

        #region UploadPathImage
        private async void HandleSelected(InputFileChangeEventArgs e)
        {

            var imageFiles = e.File;

            if (imageFiles != null)
            {
                var resizedFile = await imageFiles.RequestImageFileAsync("image/png", 300, 300);

                using var ms = resizedFile.OpenReadStream(resizedFile.Size);
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", imageFiles.Name);
                ImgUrl = await PathService.UploadPathWayImage(content);
                PathData.ImageUrl = ImgUrl;
            }

        }
        #endregion

    }
}
