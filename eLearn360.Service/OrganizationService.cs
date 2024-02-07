using eLearn360.Data.Models;
using eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM;
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
    public class OrganizationService : IOrganizationService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public OrganizationService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }

        #region Post
        public async Task<HttpResponseMessage> Add(Organization organization)
        {
            try
            {
                var organizationjson = new StringContent(JsonSerializer.Serialize(organization), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync("/api/Organization", organizationjson);
                Toaster.Success("Organisme créé !");
                return result;
            }
            catch (Exception ex)
            {
                Toaster.Warning("Erreur pendant l'ajout");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Put
        public async Task<HttpResponseMessage> Update(Organization organization)
        {
            try
            {
                var organizationjson = new StringContent(JsonSerializer.Serialize(organization), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PutAsync("api/Organization", organizationjson);
                Toaster.Info("Organisme modifié !");
                return result;
            }
            catch (Exception err)
            {
                Toaster.Warning("Erreur pendant la modification");
                Console.WriteLine(err.Message);
                throw;
            }
        }
        #endregion

        #region GetAll
        public async Task<List<Organization>> GetAll()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Organization>>(
                await _httpClient.GetStreamAsync($"api/Organization/"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetByID
        public async Task<Organization> GetById(Guid organizationid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Organization>(
                await _httpClient.GetStreamAsync($"api/Organization/{organizationid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete
        public async Task<HttpResponseMessage> Delete(Guid organizationid)
        {
            try
            {
                return await _httpClient.DeleteAsync($"api/Organization/organizationdelete/{organizationid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetUsersByOrgaId
        public async Task<Organization> GetAllUserByOrganizationId(Guid organizationid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Organization>(
                await _httpClient.GetStreamAsync($"api/Organization/getalluser/{organizationid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion


        public async Task<HttpResponseMessage> AddNewUserAndOccupationByOrganizationId(UserHasOccupationVM userHasOccupationvm)
        {
            try
            {
                var userHasOccupationvmjson = new StringContent(JsonSerializer.Serialize(userHasOccupationvm), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync("/api/Organization/addnewuser", userHasOccupationvmjson);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Upload Image Organization
        public async Task<string> UploadOrganizationImage(MultipartFormDataContent content)
        {
            try
            {
                var postResult = await _httpClient.PostAsync("api/Images/organizationimage", content);
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
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }

        }
        #endregion
    }
}
