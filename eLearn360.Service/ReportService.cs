using eLearn360.Data.VM;
using eLearn360.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLearn360.Service
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;

        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Get Report Course
        public async Task<LessonReportVM> StudentCourseReport(Guid userid, Guid groupid)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<LessonReportVM>(
                    await _httpClient.GetStreamAsync($"/api/Report/lessonstudentreport?userid={userid}&groupid={groupid}"),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
