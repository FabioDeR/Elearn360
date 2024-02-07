using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class GroupAdd
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] IGroupService GroupService { get; set; }
        [Inject] protected IToaster Toaster { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid OrganizationId { get; set; }

        public Group Group { get; set; } = new();
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
            _isLoading = false;
        }

        protected async Task HandleValidRequest()
        {
            Group.OrganizationId = OrganizationId;
            var res = await GroupService.Add(Group);

            if (res.IsSuccessStatusCode)
            {               
                NavigationManager.NavigateTo("grouplistoverview");
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
