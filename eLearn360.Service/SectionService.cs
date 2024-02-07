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
    public class SectionService : ISectionService
    {
        private readonly HttpClient _httpClient;
		protected IToaster Toaster { get; set; }
		public SectionService(HttpClient httpClient,IToaster toaster)
        {
            _httpClient = httpClient;
			Toaster = toaster;
        }

		#region Get Private Section
		public async Task<List<Section>> GetPrivateSection(Guid userId)
		{
			try
			{
				List<Section> privateSection = await JsonSerializer.DeserializeAsync<List<Section>>(
					await _httpClient.GetStreamAsync($"api/Section/privatesection/{userId}"),
					new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				return privateSection;
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}
		}
		#endregion
		#region Get Public Section
		public async Task<List<Section>> GetPublicSection(Guid userId)
		{
			try
			{
				List<Section> publicSection = await JsonSerializer.DeserializeAsync<List<Section>>(
					await _httpClient.GetStreamAsync($"api/Section/publicsection/{userId}"),
					new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				return publicSection;
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}
		}
		#endregion
		#region Duplicate Section By SectionId
		public async Task<HttpResponseMessage> DuplicateSection(Guid sectionid, Guid userid)
		{
			try
			{
				var result = await _httpClient.GetAsync($"api/Section/duplicatesection/{sectionid}/{userid}");
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
		#endregion
		#region Get By Id
		public async Task<Section> GetById(Guid id)
		{
			try
			{
				return await JsonSerializer.DeserializeAsync<Section>(
					await _httpClient.GetStreamAsync($"api/section/{id}"),
					new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}
		}
		#endregion
		#region Add
		public async Task<Guid> Add(Section section)
		{
			try
			{
				var sectionjson = new StringContent(JsonSerializer.Serialize(section), Encoding.UTF8, "application/json");

				Guid newResult = await JsonSerializer.DeserializeAsync<Guid>(
				   await _httpClient.PostAsync("/api/section", sectionjson).Result.Content.ReadAsStreamAsync(),
				   new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                Toaster.Success("Section enregistrée !");
                return newResult;
			}
			catch (Exception err)
			{
                Toaster.Error("Erreur lors de l'enregistrement !");
                Console.WriteLine(err);
				throw;
			}
		}
		#endregion
		#region Update
		public async Task Update(Section section)
		{
            try
            {
				var sectionjson = new StringContent(JsonSerializer.Serialize(section), Encoding.UTF8, "application/json");
				await _httpClient.PutAsync("api/section", sectionjson);
                Toaster.Success("La section a correctement été modifiée !");
            }
            catch (Exception ex)
            {
                Toaster.Error("Erreur lors de la mofication !");
                Console.WriteLine(ex.Message);
                throw;
            }
		
		}


		#endregion
		public async Task<List<Guid>> GetLessonGuid(Guid sectionId)
		{
            try
            {
				return await JsonSerializer.DeserializeAsync<List<Guid>>(
					await _httpClient.GetStreamAsync($"api/section/lessonguid/{sectionId}"),
					new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
		}
		public async Task<List<SectionHasLesson>> GetIncludeSectionHasLesson(Guid sectionId)
        {
            try
            {
				return await JsonSerializer.DeserializeAsync<List<SectionHasLesson>>(
					await _httpClient.GetStreamAsync($"api/section/sectionhaslesson/{sectionId}"),
					new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
		#region Update Position/Delete relation				
		public async Task UpdateOrDelete( List<SectionHasLesson> sectionHasLessons)
		{
			try
			{
				var sectionjson = new StringContent(JsonSerializer.Serialize(sectionHasLessons), Encoding.UTF8, "application/json");			
				HttpResponseMessage rep = await _httpClient.PostAsync("api/section/updatepositionordelete", sectionjson);
                Toaster.Success("Modification enregistrée !");
            }
			catch (Exception ex)
			{
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion
		#region Remove SectionHasLesson	
		public async Task RemoveSectionHasLesson(SectionHasLesson sectionHasLesson)
		{
			try
			{
				var sectionjson = new StringContent(JsonSerializer.Serialize(sectionHasLesson), Encoding.UTF8, "application/json");
				HttpResponseMessage rep = await _httpClient.PostAsync("api/section/removesectionhaslesson", sectionjson);
				Toaster.Error("Leçon supprimée !");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion
		#region Get Section By PathId
		public async Task<List<Section>> GetSectionByPathId(Guid pathId, Guid userId)
		{
			try
			{
				List<Section> sections = await JsonSerializer.DeserializeAsync<List<Section>>(
														  await _httpClient.GetStreamAsync($"api/section/sectionbypathid/{pathId}/{userId}"),
														  new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				return sections;
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
