using eLearn360.Data.Models;
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
    public class GroupService : IGroupService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public GroupService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }

        #region Post
        public async Task<HttpResponseMessage> Add(Group group)
        {
            try
            {
                var groupjson = new StringContent(JsonSerializer.Serialize(group), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync("/api/Group", groupjson);
                Toaster.Success("Classe / Groupe créé(e) !");
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

        #region Delete
        public async Task<HttpResponseMessage> Delete(Guid groupid)
        {
            try
            {
                return await _httpClient.DeleteAsync($"api/Group/groupdelete/{groupid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetAll
        public async Task<List<Group>> GetAll()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Group>>(
                await _httpClient.GetStreamAsync($"api/Group/"),
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
        public async Task<Group> GetById(Guid groupid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Group>(
                await _httpClient.GetStreamAsync($"api/Group/{groupid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Put
        public async Task<HttpResponseMessage> Update(Group group)
        {
            try
            {
                var groupjson = new StringContent(JsonSerializer.Serialize(group), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PutAsync("api/Group", groupjson);
                Toaster.Success("Modification effectuée !");
                return result;
            }
            catch (Exception err)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.Error.WriteLine(err.Message);
                throw;
            }
        }
        #endregion

        #region GetAllByOrgaId
        public async Task<List<Group>> GetByOrganizationId(Guid organizationId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Group>>(
                await _httpClient.GetStreamAsync($"api/Group/getbyorganizationid/{organizationId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }
        #endregion

        #region Upload Image Group
        public async Task<string> UploadGroupImage(MultipartFormDataContent content)
        {
            try
            {
                var postResult = await _httpClient.PostAsync("api/Images/groupimage", content);
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

        #region GetUserByGroupId
        public async Task<Group> GetAllUserByGroupId(Guid groupid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Group>(
                await _httpClient.GetStreamAsync($"api/Group/alluser/{groupid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region RemoveUserHasGroup
        public async Task<HttpResponseMessage> RemoveUserHasGroup(UserHasGroup userHasGroup)
        {
            try
            {
                var userHasGroupjson = new StringContent(JsonSerializer.Serialize(userHasGroup), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync("/api/Group/removeuserhasgroup", userHasGroupjson);
                Toaster.Error("Utilisateur supprimé !");
                return result;
            }
            catch (Exception ex)
            {
                Toaster.Warning("Erreur pendant la suppression");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion

        #region GetStudent/TeacherByGroupId      

        public async Task<Group> GetTeacherByGroupId(Guid groupid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Group>(
                await _httpClient.GetStreamAsync($"api/Group/getteacher/{groupid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Group> GetStudentByGroupId(Guid groupid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Group>(
                await _httpClient.GetStreamAsync($"api/Group/getstudent/{groupid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<User>> GetallUsersNotInGroup(Guid groupid, Guid organizationid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<User>>(
                await _httpClient.GetStreamAsync($"api/Group/allusersnotingroup/{groupid}/{organizationid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Group By Orga & User
        public async Task<List<Group>> GetGroupByOrgaAndUserId(Guid organizationid, Guid userid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Group>>(
                await _httpClient.GetStreamAsync($"api/Group/getstudentgroupbyuserandorga/{organizationid}/{userid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        #endregion

        public async Task<List<Group>> GetMyGroups(Guid userid, Guid organizationid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Group>>(
                await _httpClient.GetStreamAsync($"api/Group/getmygroups/{userid}/{organizationid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
