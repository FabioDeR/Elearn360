using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace eLearn360.UI.Pages.OrganizationPage
{
    public partial class OrganizationEdit
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject] public IOrganizationService OrganizationService { get; set; }
        [Parameter] public Guid Id { get; set; }
        public Organization Organization { get; set; }
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

        protected async Task LoadList()
        {
            try
            {
                Organization = await OrganizationService.GetById(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        protected async Task HandleValidRequest()
        {
            var res = await OrganizationService.Update(Organization);
            if (res.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo($"organizationoverview");
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
                ImgUrl = await OrganizationService.UploadOrganizationImage(content);
                Organization.ImageUrl = ImgUrl;
                StateHasChanged();
            }

        }
        #endregion
    }
}
