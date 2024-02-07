using eLearn360.Data.VM;
using eLearn360.Data.VM.ReportVM;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.ReportController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get Lesson Report
        [HttpGet("lessonstudentreport")]
        public async Task<IActionResult> GetLessonStudentReport(Guid userid, Guid groupid)
        {
            try
            {
                LessonReportVM reports = await _unitOfWork.LessonRepository.GetLessonReport(userid, groupid);
                return Ok(reports);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Get Quizz Report By UserId
        [HttpGet("getquizzreportbyuserid/{id}")]
        public async Task<IActionResult> GetReportByUserId(Guid id)
        {
            try
            {
                List<QuizzReportVM> QuizzReportVMs = await _unitOfWork.QuizzRepository.GetReportByUserId(id);
                return Ok(QuizzReportVMs);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Get Quizz Report By GroupId
        [HttpGet("getquizzreportbygroupid/{id}")]
        public async Task<IActionResult> GetReportBygroupId(Guid id)
        {
            try
            {
                List<QuizzReportVM> QuizzReportVMs = await _unitOfWork.QuizzRepository.GetReportByGroupId(id);
                return Ok(QuizzReportVMs);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion


    }
}
