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
    public class OccupationService : IOccupationService
    {
        private readonly HttpClient _httpClient;

        public OccupationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Occupation>> GetAll()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Occupation>>(
                await _httpClient.GetStreamAsync($"api/Occupation/"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Get Occupation User By Orga Id
        public async Task<List<Occupation>> GetOccupationUserByOrga(Guid userid, Guid orgaid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<Occupation>>(
                await _httpClient.GetStreamAsync($"api/Occupation/getoccupationuserbyorgaid/{userid}/{orgaid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
