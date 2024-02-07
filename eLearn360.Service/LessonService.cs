using eLearn360.Data.Models;
using eLearn360.Data.VM.UserVM;
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
    public class LessonService : ILessonService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public LessonService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }
        #region Post Lesson
        public async Task<Lesson> Add(Lesson lesson)
        {
            try
            {
                var lessonjson = new StringContent(JsonSerializer.Serialize(lesson), Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync("api/lesson", lessonjson);
                Toaster.Success("Leçon enregistrée !");
                return lesson;

            }
            catch (Exception)
            {
                Toaster.Error("Une erreur est apparue !");
                throw;
            }
        }
        #endregion

        #region Duplicate Lesson
        public async Task<HttpResponseMessage> DuplicateLesson(Guid lessonId, Guid userId)
        {
            try
            {
                HttpResponseMessage result = await _httpClient.GetAsync($"api/lesson/duplicatelesson/{userId}/{lessonId}");
                Toaster.Success("Leçon copiée !");
                return result;
            }
            catch (Exception ex)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Lesson By Id
        public async Task<Lesson> GetById(Guid lessonId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Lesson>(
                await _httpClient.GetStreamAsync($"api/lesson/{lessonId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }

        }
        #endregion
        #region Update Lessons
        public async Task Update(Lesson lesson)
        {
            try
            {
                var lessonjson =
             new StringContent(JsonSerializer.Serialize(lesson), Encoding.UTF8, "application/json");

                await _httpClient.PutAsync("api/lesson", lessonjson);
                Toaster.Success("La leçon a correctement été modifiée !");
            }
            catch (Exception err)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(err);
                throw;
            }

        }
        #endregion
        #region GetPrivateLesson
        public async Task<List<Lesson>> GetPrivateLesson(Guid userId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Lesson>>(
                await _httpClient.GetStreamAsync($"api/lesson/privatelesson/{userId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region GetPublicLesson
        public async Task<List<Lesson>> GetPublicLesson(Guid userId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Lesson>>(
                await _httpClient.GetStreamAsync($"api/lesson/publiclesson/{userId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Lesson By PathId
        public async Task<List<Lesson>> GetLessonByPathId(Guid pathId, Guid userId)
        {
            try
            {
                List<Lesson> lessons = await JsonSerializer.DeserializeAsync<List<Lesson>>(
                                                          await _httpClient.GetStreamAsync($"api/lesson/lessonbypathid/{pathId}/{userId}"),
                                                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return lessons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion

        #region Post Historic
        public async Task<HttpResponseMessage> PostHistoric(Guid userId, Guid itemId)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Lesson/posthistoric?userid={userId}&lessonId={itemId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }        
        
        public async Task<HttpResponseMessage> PostStartHistoric(Guid userId, Guid itemId)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Lesson/poststarthistoric?userid={userId}&itemid={itemId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }     
        
        public async Task<HttpResponseMessage> PostEndHistoric(Guid userId, Guid itemId)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Lesson/updateendhistoric?userid={userId}&itemid={itemId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
