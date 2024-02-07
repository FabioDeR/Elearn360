using eLearn360.Data.Models;
using eLearn360.Data.VM;
using eLearn360.Data.VM.AccountVM;
using eLearn360.Service.Interface;
using Sotsera.Blazor.Toaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLearn360.Service
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public UserService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }

        public async Task<List<Organization>> GetAllOrganizationByUserId(Guid userid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Organization>>(
                await _httpClient.GetStreamAsync($"api/User/getallorganization/{userid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Get User Profile
        public async Task<AccountRegisterEditVM> GetUserProfile(Guid userid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<AccountRegisterEditVM>(
                await _httpClient.GetStreamAsync($"api/User/getuserprofile/{userid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region User Profile Update
        public async Task<HttpResponseMessage> UserProfileUpdate(AccountRegisterEditVM user)
        {
            try
            {
                var userjson = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PutAsync("api/User/userprofileupdate", userjson);
                Toaster.Success($"Modification effectuée !");
                return result; ;
            }
            catch (Exception err)
            {
                Toaster.Error("Erreur dans la modification !");
                Console.WriteLine(err);
                throw;
            }

        }
        #endregion

        #region Upload Image User
        public async Task<string> UploadUserImage(MultipartFormDataContent content)
        {
            var postResult = await _httpClient.PostAsync("api/Images/userimage", content);
            var postContent = await postResult.Content.ReadAsStringAsync();
            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }
            else
            {

                return postContent;
            }
        }
        #endregion


    }
}
