using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class GroupEdit
    {
        [Inject] IGroupService GroupService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public Guid GroupId { get; set; }

        public Group Group { get; set; } = new();
        protected string ImgUrl { get; set; }

        private bool _isLoading = false;

        protected async override Task OnInitializedAsync()
        {
            _isLoading = true;
            var authenticationState = (await AuthenticationStateTask).User;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                LoadList();
            }
            _isLoading = false;
        }

        protected async void LoadList()
        {
            Group = await GroupService.GetById(GroupId);
            StateHasChanged();
        }

        protected async Task HandleValidRequest()
        {
            var res = await GroupService.Update(Group);

            if (res.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo($"/groupoverview/{GroupId}");
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
                ImgUrl = await GroupService.UploadGroupImage(content);
                Group.ImageUrl = ImgUrl;
                StateHasChanged();
            }

        }
        #endregion
    }
}
