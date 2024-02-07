using eLearn360.Data.Models;
using eLearn360.Data.VM.UserVM;
using eLearn360.Service.Interface;
using Sotsera.Blazor.Toaster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLearn360.Service
{
    public class PathWayService : IPathWayService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public PathWayService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }

        public async Task<Guid> Add(PathWay pathWay)
        {
            try
            {
                var pathjson = new StringContent(JsonSerializer.Serialize(pathWay), Encoding.UTF8, "application/json");

                Guid newResult = await JsonSerializer.DeserializeAsync<Guid>(
                   await _httpClient.PostAsync("/api/pathway", pathjson).Result.Content.ReadAsStreamAsync(),
                   new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                Toaster.Success("Parcours enregistré !");
                return newResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }      

        public async Task<PathWay> GetById(Guid id)
        {
            return await JsonSerializer.DeserializeAsync<PathWay>(
                    await _httpClient.GetStreamAsync($"api/pathway/{id}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<PathWayHasCourse>> GetIncludePathWayHasCourse(Guid pathid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<PathWayHasCourse>>(
                    await _httpClient.GetStreamAsync($"api/pathway/pathwayhascourse/{pathid}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<PathWay>> GetPrivatePathWay(Guid userId)
        {
            List<PathWay> privatePathWay = await JsonSerializer.DeserializeAsync<List<PathWay>>(
                await _httpClient.GetStreamAsync($"api/pathway/privatepathway/{userId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return privatePathWay;
        }

        public async Task<List<PathWay>> GetPublicPathWay(Guid userId)
        {
            List<PathWay> publicPathWay = await JsonSerializer.DeserializeAsync<List<PathWay>>(
                        await _httpClient.GetStreamAsync($"api/pathway/publicpathway/{userId}"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return publicPathWay;
        }

        public async Task<List<Guid>> GetCourseGuid(Guid pathid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Guid>>(
                    await _httpClient.GetStreamAsync($"api/pathway/courseguid/{pathid}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Guid>> GetGroupGuid(Guid pathid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Guid>>(
                    await _httpClient.GetStreamAsync($"api/pathway/groupguid/{pathid}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task RemovePathWayHasCourse(PathWayHasCourse pathWayHasCourse)
        {
            try
            {
                var pathjson = new StringContent(JsonSerializer.Serialize(pathWayHasCourse), Encoding.UTF8, "application/json");
                HttpResponseMessage rep = await _httpClient.PostAsync("api/pathway/removepathwayhascourse", pathjson);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task RemovePathWayHasGroup(PathWayHasGroup pathWayHasGroup)
        {
            try
            {
                var pathjson = new StringContent(JsonSerializer.Serialize(pathWayHasGroup), Encoding.UTF8, "application/json");
                HttpResponseMessage rep = await _httpClient.PostAsync("api/pathway/removepathwayhasgroup", pathjson);
                Toaster.Error("Retrait de la liaison !");
            }
            catch (Exception ex)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Update(PathWay pathWay)
        {
            try
            {
                var pathjson = new StringContent(JsonSerializer.Serialize(pathWay), Encoding.UTF8, "application/json");
                var res = await _httpClient.PutAsync("api/pathway", pathjson);
                Toaster.Success("Modification enregistrée !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateOrDelete(List<PathWayHasCourse> pathWayHasCourses)
        {
            try
            {
                var pathjson = new StringContent(JsonSerializer.Serialize(pathWayHasCourses), Encoding.UTF8, "application/json");
                HttpResponseMessage rep = await _httpClient.PostAsync("api/pathway/updatepositionordelete", pathjson);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Delete
        public async Task<HttpResponseMessage> Delete(Guid pathid)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"api/pathway/{pathid}");
                Toaster.Success("L'élément a bien été supprimé !");
                return result;
            }
            catch (Exception err)
            {
                Toaster.Warning("Erreur lors de la suppression !");
                Console.WriteLine(err);
                throw;
            }

        }
        #endregion

        #region GetPath By PathId
        public async Task<List<PathWay>> GetPathByPathid(Guid pathId, Guid userId)
        {
            try
            {
                List<PathWay> paths = await JsonSerializer.DeserializeAsync<List<PathWay>>(
                                                          await _httpClient.GetStreamAsync($"api/pathway/pathbypathid/{pathId}/{userId}"),
                                                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return paths;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region GetPath By OrganizationId

        public async Task<List<PathWay>> GetPathByOrganizationId(Guid organizationid, Guid userid)
        {
            try
            {
                List<PathWay> paths = await JsonSerializer.DeserializeAsync<List<PathWay>>(
                await _httpClient.GetStreamAsync($"api/pathway/pathwaybyorganizationid/{organizationid}/{userid}"),
                                                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return paths;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion        
        
        #region GetPath By OrganizationIdUserID-Not Teacher

        public async Task<List<PathWay>> GetPathByOrgaIdAndUserIdNotStudent(Guid organizationid, Guid userid)
        {
            try
            {
                List<PathWay> paths = await JsonSerializer.DeserializeAsync<List<PathWay>>(
                await _httpClient.GetStreamAsync($"api/pathway/getteacherpathbyuserandorgaid/{userid}/{organizationid}"),
                                                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return paths;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region UploadImageCourse
        public async Task<string> UploadPathWayImage(MultipartFormDataContent content)
        {
            try
            {
                var postResult = await _httpClient.PostAsync("api/Images/pathimage", content);
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
