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
    public class LevelService : ILevelService
	{
		private readonly HttpClient _httpClient;

		public LevelService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		#region Level Post

		public async Task<Level> Add(Level level)
		{
			try
			{
				var leveljson = new StringContent(JsonSerializer.Serialize(level), Encoding.UTF8, "application/json");

				var result = await _httpClient.PostAsync("api/level", leveljson);

				return level;

			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion

		#region Get All Level

		public async Task<IEnumerable<Level>> GetAll()
		{
			try
			{
				return await JsonSerializer.DeserializeAsync<IEnumerable<Level>>(
				await _httpClient.GetStreamAsync("api/level"),
				new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}

		}
		#endregion

		#region Get Level by Id
		public async Task<Level> GetById(int levelId)
		{
			try
			{
				return await JsonSerializer.DeserializeAsync<Level>(
				await _httpClient.GetStreamAsync($"api/level/{levelId}"),
				new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}

		}
		#endregion

		#region Update Level

		public async Task Update(Level level)
		{
			try
			{
				var leveljson =
						  new StringContent(JsonSerializer.Serialize(level), Encoding.UTF8, "application/json");

				await _httpClient.PutAsync("api/level", leveljson);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}

		}
		#endregion

		#region Level delete

		public async Task<HttpResponseMessage> Delete(int levelId)
		{
			try
			{
				HttpResponseMessage result = await _httpClient.DeleteAsync($"api/level/leveldelete/{levelId}");
				return result;
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
