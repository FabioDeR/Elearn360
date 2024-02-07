using elearn.Data.ViewModel.QuestionVM;
using eLearn360.Data.Models;
using eLearn360.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLearn360.Service
{
    public class QuizzService : IQuizzService
    {
        private readonly HttpClient _httpClient;
        public QuizzService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

		#region Quizz Generation
		public async Task<QuizzVM> GenerateQuizz(QuizzVM quizzvm)
		{
			try
			{
                var quizzjson = new StringContent(JsonSerializer.Serialize(quizzvm), Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync("api/quizz/quizzgeneration", quizzjson);
				var responseObject = await result.Content.ReadAsAsync<QuizzVM>();

				return responseObject;

			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				throw;
			}
		}
		#endregion

		#region UpdateQuizz
		public async Task<QuizzVM> UpdateRating(QuizzVM quizzvm)
		{
			try
			{
				var quizzjson = new StringContent(JsonSerializer.Serialize(quizzvm), Encoding.UTF8, "application/json");
				var result = await _httpClient.PutAsync("api/quizz/updaterating", quizzjson);
				var responseObject = await result.Content.ReadAsAsync<QuizzVM>();

				return responseObject;
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
