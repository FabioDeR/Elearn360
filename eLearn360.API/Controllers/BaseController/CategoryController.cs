using eLearn360.Data.Models;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private IUnitOfWork _unitOfWork;


		public CategoryController(IUnitOfWork unitOfWork)
		{

			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Category> categorysList = await _unitOfWork.CategoryRepository.Get();
				return Ok(categorysList);
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
				Category category = await _unitOfWork.CategoryRepository.GetById(id);
				return Ok(category);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Post
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Category category)
		{
			try
			{
				if (category == null)
				{
					return BadRequest();
				}
				category.CreationDate = DateTime.Now;
				await _unitOfWork.CategoryRepository.Post(category);
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
		public async Task<IActionResult> Put([FromBody] Category category)
		{
			try
			{
				if (category == null)
				{
					return BadRequest();
				}
				await _unitOfWork.CategoryRepository.Put(category.Id, category);
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

		[HttpDelete("{categoryid}")]
		public async Task<IActionResult> SoftDelete(Guid categoryid)
		{
			try
			{
				Category category = await _unitOfWork.CategoryRepository.GetById(categoryid);
				if (category == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.CategoryRepository.Delete(categoryid, category);
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
