using eLearn360.Data.Models;
using eLearn360.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLearn360.Service
{
    public class GenderService : IGenderService
    {
        private readonly HttpClient _httpClient;

        public GenderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Add(Gender gender)
        {
            try
            {
                var genderJson = new StringContent(JsonSerializer.Serialize(gender), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync("api/Gender", genderJson);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> Delete(Guid genderid, Guid userid)
        {
            try
            {
                HttpResponseMessage result = await _httpClient.DeleteAsync($"api/Gender/genderdelete/{genderid}/{userid}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Gender>> GetAll()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Gender>>(
                await _httpClient.GetStreamAsync($"api/Gender/"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Gender> GetById(Guid genderid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Gender>(
                await _httpClient.GetStreamAsync($"api/Gender/{genderid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> Update(Gender gender)
        {
            try
            {
                var genderJson = new StringContent(JsonSerializer.Serialize(gender), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PutAsync("api/Gender", genderJson);
                return result;
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
                throw;
            }
        }
    }
}
