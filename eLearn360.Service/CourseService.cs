using eLearn360.Data.Models;
using eLearn360.Data.VM.CourseVM;
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
    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public CourseService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }

        public async Task<Guid> Add(Course course)
        {
            try
            {
                var coursejson = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");

                Guid newResult = await JsonSerializer.DeserializeAsync<Guid>(
                   await _httpClient.PostAsync("/api/course", coursejson).Result.Content.ReadAsStreamAsync(),
                   new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                Toaster.Success("Le cours a correctement été enregistré !");
                return newResult;
            }
            catch (Exception ex)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }

        public async Task<HttpResponseMessage> DuplicateCourse(Guid courseid, Guid userid)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Course/duplicatecourse/{courseid}/{userid}");
                Toaster.Success("Section copiée !");
                return result;
            }
            catch (Exception err)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(err);
                throw;
            }
        }

        public async Task<Course> GetById(Guid id)
        {
            return await JsonSerializer.DeserializeAsync<Course>(
                    await _httpClient.GetStreamAsync($"api/Course/{id}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<CourseHasSection>> GetIncludeCourseHasSection(Guid courseId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<CourseHasSection>>(
                    await _httpClient.GetStreamAsync($"api/course/coursehassection/{courseId}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Course>> GetPrivateCourse(Guid userId)
        {
            List<Course> privateCourse = await JsonSerializer.DeserializeAsync<List<Course>>(
                await _httpClient.GetStreamAsync($"api/Course/privatecourse/{userId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return privateCourse;
        }

        public async Task<List<Course>> GetPublicCourse(Guid userId)
        {
            List<Course> publicCourse = await JsonSerializer.DeserializeAsync<List<Course>>(
                        await _httpClient.GetStreamAsync($"api/Course/publiccourse/{userId}"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return publicCourse;
        }

        public async Task<List<Guid>> GetSectionGuid(Guid courseid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Guid>>(
                    await _httpClient.GetStreamAsync($"api/course/sectionguid/{courseid}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task RemoveCourseHasSection(CourseHasSection courseHasSection)
        {
            try
            {
                var coursejson = new StringContent(JsonSerializer.Serialize(courseHasSection), Encoding.UTF8, "application/json");
                HttpResponseMessage rep = await _httpClient.PostAsync("api/course/removecoursehasscourse", coursejson);
                Toaster.Success("Le cours a correctement été modifiée !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Update(Course course)
        {
            try
            {
                var coursejson = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
                await _httpClient.PutAsync("api/course", coursejson);
                Toaster.Success("Le cours a correctement été modifiée !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateOrDelete(List<CourseHasSection> courseHasSections)
        {
            try
            {
                var coursejson = new StringContent(JsonSerializer.Serialize(courseHasSections), Encoding.UTF8, "application/json");
                HttpResponseMessage rep = await _httpClient.PostAsync("api/course/updatepositionordelete", coursejson);
                Toaster.Success("Cours modifié !");
            }
            catch (Exception ex)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #region UploadImageCourse
        public async Task<string> UploadCourseImage(MultipartFormDataContent content)
        {
            try
            {
                var postResult = await _httpClient.PostAsync("api/Images/courseimage", content);
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

        #region GetCourse By PathId And UserId
        public async Task<List<Course>> GetCourseByPathAndUserId(Guid pathid, Guid userid)
        {
            try
            {
                List<Course> courses = await JsonSerializer.DeserializeAsync<List<Course>>(
                                                          await _httpClient.GetStreamAsync($"api/course/coursebypathanduserid/{pathid}/{userid}"),
                                                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return courses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetCourse By PathId
        public async Task<List<Course>> GetCourseByPathAndUserIdWithHistoricCourse(Guid pathid, Guid userid)
        {
            try
            {
                List<Course> courses = await JsonSerializer.DeserializeAsync<List<Course>>(
                                                          await _httpClient.GetStreamAsync($"api/course/coursebypathanduseridwithhistoriccourse/{pathid}/{userid}"),
                                                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return courses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion

        
        public async Task<ViewContentVM> GetViewContentById(Guid id)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<ViewContentVM>(
                        await _httpClient.GetStreamAsync($"api/Course/getviewcontentvm/{id}"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Guid>> GetAllGuidByCourseId(Guid courseid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Guid>>(
                    await _httpClient.GetStreamAsync($"api/Course/getallguids/{courseid}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Course> GetCourseTolesson(Guid courseid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Course>(
                        await _httpClient.GetStreamAsync($"api/Course/getcoursetolesson/{courseid}"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Guid>> GetAllGuidSeen(Guid courseid, Guid userid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Guid>>(
                    await _httpClient.GetStreamAsync($"api/Course/allguidseen/{courseid}/{userid}"),
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
