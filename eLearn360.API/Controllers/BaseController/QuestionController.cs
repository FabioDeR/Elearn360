using eLearn360.Data.Models;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class QuestionController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;

		public QuestionController(IUnitOfWork unitOfWork)
		{

			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Question> Questions= await _unitOfWork.QuestionRepository.Get();
				return Ok(Questions);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region GetById
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				Question Question = await _unitOfWork.QuestionRepository.GetById(id);
				return Ok(Question);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get By Id with Include
		[HttpGet("withinclude")]
		public async Task<IActionResult> GetByQuestionIdWithInclude(Guid id)
		{
			try
			{
				Question Question = await _unitOfWork.QuestionRepository.GetByQuestionIdWithInclude(id);
				return Ok(Question);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get Question By Lesson Id
		[HttpGet("bylessonid")]
		public async Task<IActionResult> GetByLessonId(Guid id)
		{
			try
			{
				List<Question> Questions = await _unitOfWork.QuestionRepository.GetByLessonId(id);
				return Ok(Questions);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get Question By Section Id
		[HttpGet("bysectionid")]
		public async Task<IActionResult> GetBySectionId(Guid id)
		{
			try
			{
				List<Question> Questions = await _unitOfWork.QuestionRepository.GetBySectionId(id);
				return Ok(Questions);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get Question By Course Id
		[HttpGet("bycourseid")]
		public async Task<IActionResult> GetByCourseId(Guid id)
		{
			try
			{
				List<Question> Questions = await _unitOfWork.QuestionRepository.GetByCourseId(id);
				return Ok(Questions);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get Question By Path Id
		[HttpGet("bypathid")]
		public async Task<IActionResult> GetByPathId(Guid id)
		{
			try
			{
				List<Question> Questions = await _unitOfWork.QuestionRepository.GetByPathId(id);
				return Ok(Questions);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Remove Question Has Lesson
		[HttpPost("removequestionhaslesson")]
		public async Task<IActionResult> RemoveQuestionHasLesson([FromBody] QuestionHasLesson questionHasLesson)
		{
			try
			{
				await _unitOfWork.QuestionRepository.RemoveQuestionHasLesson(questionHasLesson);
				return Ok("Item has been removed");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Remove Question Has Section
		[HttpPost("removequestionhassection")]
		public async Task<IActionResult> RemoveQuestionHasSection([FromBody] QuestionHasSection questionHasSection)
		{
			try
			{
				await _unitOfWork.QuestionRepository.RemoveQuestionHasSection(questionHasSection);
				return Ok("Item has been removed");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Remove Question Has Course
		[HttpPost("removequestionhascourse")]
		public async Task<IActionResult> RemoveQuestionHasCourse([FromBody] QuestionHasCourse questionHasCourse)
		{
			try
			{
				await _unitOfWork.QuestionRepository.RemoveQuestionHasCourse(questionHasCourse);
				return Ok("Item has been removed");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Remove Question Has PathWay
		[HttpPost("removequestionhaspathway")]
		public async Task<IActionResult> RemoveQuestionHasPathWay([FromBody] QuestionHasPathWay questionHasPathWay)
		{
			try
			{
				await _unitOfWork.QuestionRepository.RemoveQuestionHasPathWay(questionHasPathWay);
				return Ok("Item has been removed");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Post
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Question Question)
		{
			try
			{
				if (Question == null)
				{
					return BadRequest();
				}
				await _unitOfWork.QuestionRepository.Post(Question);
				return Ok();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Update
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] Question Question)
		{
			try
			{
				if (Question == null)
				{
					return BadRequest();
				}
				await _unitOfWork.QuestionRepository.Put(Question.Id, Question);
				return Ok();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Delete

		[HttpDelete("{questionid}")]
		public async Task<IActionResult> SoftDelete(Guid questionid)
		{
			try
			{
				Question Question = await _unitOfWork.QuestionRepository.GetById(questionid);
				if (Question == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.QuestionRepository.Delete(questionid, Question);
					return Ok("Item (soft) deleted !");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return BadRequest(ex.Message);
			}
		}
		#endregion
	}
}
