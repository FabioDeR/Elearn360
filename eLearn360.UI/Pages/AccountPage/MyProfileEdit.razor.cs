using eLearn360.Data.VM;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace eLearn360.UI.Pages.AccountPage
{
    public partial class MyProfileEdit
    {
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IGenderService Gender { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public Guid UserId { get; set; }
        [Parameter] public string Role { get; set; }
        [Parameter] public Guid GroupId { get; set; }

        public AccountRegisterEditVM Item { get; set; } = new();
        public List<Gender> GenderList { get; set; } = new();
        public string ImgUrl { get; set; } = string.Empty;
        private bool _genderFormError;
        private string _genderFormValidClass;

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
                await LoadList();
            }
            _isLoading = false;

            ImgUrl = Item.ImageUrl;
            
            if(Item.Birthday == new DateTime())
            {
                Item.Birthday = DateTime.Now;
            }
            StateHasChanged();
        }

        protected async Task LoadList()
        {
            GenderList = (await Gender.GetAll()).ToList();
            // Simulate Required field : Gender
            _genderFormError = false;
            _genderFormValidClass = string.Empty;

            Item = await UserService.GetUserProfile(UserId);
            if (Item.ImageUrl == "image par défaut")
            {
                Item.ImageUrl = "/image/DefaultImageUser.png";
            }
        }

        #region Choose Picture
        private async void ChoosePicture(InputFileChangeEventArgs e)
        {
            var imageFiles = e.File;

            if (imageFiles != null)
            {
                var resizedFile = await imageFiles.RequestImageFileAsync("image/png", 300, 300);

                using var ms = resizedFile.OpenReadStream(resizedFile.Size);
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", imageFiles.Name);
                ImgUrl = await UserService.UploadUserImage(content);
                Item.ImageUrl = ImgUrl;
                StateHasChanged();
            }
        }
        #endregion

        #region GenderSelect
        protected void GenderSelect(ChangeEventArgs e)
        {
            Item.GenderId = Guid.Parse(e.Value.ToString());
            // Simulate Required field : Gender
            _genderFormError = false;
            _genderFormValidClass = "modified valid";
        }
        #endregion

        protected async Task HandleValidRequest()
        {
            var res = await UserService.UserProfileUpdate(Item);
            if (res.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("myprofileoverview");
            }
        }
    }
}
