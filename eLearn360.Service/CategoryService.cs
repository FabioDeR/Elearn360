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
    public class CategoryService : ICategoryService
	{
		private readonly HttpClient _httpClient;

		public CategoryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		#region Post Category
		public async Task<Category> Add(Category category)
		{
			try
			{
				var categoryjson = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
				var result = await _httpClient.PostAsync("api/category", categoryjson);

				return category;


			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}
		}
		#endregion

		#region Get ALL Category
		public async Task<IEnumerable<Category>> GetAll()
		{
			try
			{
				return await JsonSerializer.DeserializeAsync<IEnumerable<Category>>(
				await _httpClient.GetStreamAsync("api/category"),
				new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}

		}
		#endregion

		#region Get Category By Id
		public async Task<Category> GetById(int categoryId)
		{
			try
			{
				return await JsonSerializer.DeserializeAsync<Category>(
				await _httpClient.GetStreamAsync($"api/category/{categoryId}"),
				new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}

		}
		#endregion

		#region Update Category
		public async Task Update(Category category)
		{
			try
			{
				var categoryjson =
						  new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");

				await _httpClient.PutAsync("api/category", categoryjson);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}

		}
		#endregion

		#region Delete
		public async Task<HttpResponseMessage> Delete(int categoryId)
		{
			try
			{
				var result = await _httpClient.DeleteAsync($"/categorydelete/{categoryId}");
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
