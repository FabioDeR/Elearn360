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
    public class StaffHasOccupationService : IStaffHasOccupationService
    {
        private readonly HttpClient _httpClient;
        protected IToaster Toaster { get; set; }

        public StaffHasOccupationService(HttpClient httpClient, IToaster toaster)
        {
            _httpClient = httpClient;
            Toaster = toaster;
        }

        public async Task<HttpResponseMessage> Add(StaffOccupation staffoccupation)
        {
            try
            {
                var staffoccupationJson = new StringContent(JsonSerializer.Serialize(staffoccupation), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync("api/StaffHasOccupation", staffoccupationJson);
                Toaster.Success($"Élement ajouté !");
                return result;
            }
            catch (Exception ex)
            {
                Toaster.Warning("Erreur pendant l'ajout");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> Delete(Guid staffoccupationid)
        {
            try
            {
                HttpResponseMessage result = await _httpClient.DeleteAsync($"api/StaffHasOccupation/{staffoccupationid}");
                Toaster.Success("Élément supprimée");
                return result;
            }
            catch (Exception ex)
            {
                Toaster.Warning("Erreur lors de la suppression");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<StaffOccupation>> GetAll()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<StaffOccupation>>(
                await _httpClient.GetStreamAsync($"api/StaffHasOccupation/"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<StaffOccupation> GetById(Guid staffoccupationid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<StaffOccupation>(
                await _httpClient.GetStreamAsync($"api/StaffHasOccupation/{staffoccupationid}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> Update(StaffOccupation staffoccupation)
        {
            try
            {
                var staffoccupationJson = new StringContent(JsonSerializer.Serialize(staffoccupation), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PutAsync("api/StaffHasOccupation", staffoccupationJson);
                Toaster.Success("Élément modifié !");
                return result;
            }
            catch (Exception err)
            {
                Toaster.Warning("Erreur pendant la modification");
                Console.Error.WriteLine(err.Message);
                throw;
            }
        }

        public async Task<List<StaffOccupation>> GetByOrganizationId(Guid organizationid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<List<StaffOccupation>>(
                await _httpClient.GetStreamAsync($"api/StaffHasOccupation/getbyorganizationid/{organizationid}"),
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
