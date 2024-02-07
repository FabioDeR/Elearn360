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
	public class LevelController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;
		public LevelController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Level> levelsList = await _unitOfWork.LevelRepository.Get();
				return Ok(levelsList);
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
				Level level = await _unitOfWork.LevelRepository.GetById(id);
				return Ok(level);
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
		public async Task<IActionResult> Post([FromBody] Level level)
		{
			try
			{
				if (level == null)
				{
					return BadRequest();
				}
				await _unitOfWork.LevelRepository.Post(level);
				return Ok();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Put
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] Level level)
		{
			try
			{
				if (level == null)
				{
					return BadRequest();
				}

				await _unitOfWork.LevelRepository.Put(level.Id, level);
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
		[HttpDelete("{levelid}")]
		public async Task<IActionResult> DeleteLevel(Guid levelid)
		{
			try
			{
				Level level = await _unitOfWork.LevelRepository.GetById(levelid);
				if (level == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.LevelRepository.Delete(levelid, level);
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
