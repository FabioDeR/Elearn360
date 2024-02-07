using eLearn360.Data.Models;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]

    public class GenderController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;


		public GenderController(IUnitOfWork unitOfWork)
		{

			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Gender> gendersList = await _unitOfWork.GenderRepository.Get();
				return Ok(gendersList);
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
				Gender gender = await _unitOfWork.GenderRepository.GetById(id);
				return Ok(gender);
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
		public async Task<IActionResult> Post([FromBody] Gender gender)
		{
			try
			{
				if (gender == null)
				{
					return BadRequest();
				}
				await _unitOfWork.GenderRepository.Post(gender);
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
		public async Task<IActionResult> Put([FromBody] Gender gender)
		{
			try
			{
				if (gender == null)
				{
					return BadRequest();
				}
				await _unitOfWork.GenderRepository.Put(gender.Id, gender);
				return Ok();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region delete
		[HttpDelete("{genderid}")]
		public async Task<IActionResult> DeleteGender(Guid genderid)
		{
			try
			{
				Gender gender = await _unitOfWork.GenderRepository.GetById(genderid);
				if (gender == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.GenderRepository.Delete(genderid, gender);
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
