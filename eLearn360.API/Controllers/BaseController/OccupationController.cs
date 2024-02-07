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
	public class OccupationController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;
		public OccupationController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Occupation> occupationsList = await _unitOfWork.OccupationRepository.Get();
				return Ok(occupationsList);
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
				Occupation occupation = await _unitOfWork.OccupationRepository.GetById(id);
				return Ok(occupation);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get Occupation User By Orga
		[HttpGet("getoccupationuserbyorgaid/{userid}/{orgaid}")]
		public async Task<IActionResult> GetOccupationUserByOrga(Guid userid, Guid orgaid)
		{
			try
			{
				List<Occupation> occupations = await _unitOfWork.OccupationRepository.GetOccupationUserByOrga(userid, orgaid);
				return Ok(occupations);
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
		public async Task<IActionResult> Post([FromBody] Occupation occupation)
		{
			try
			{
				if (occupation == null)
				{
					return BadRequest();
				}
				await _unitOfWork.OccupationRepository.Post(occupation);
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
		public async Task<IActionResult> Put([FromBody] Occupation occupation)
		{
			try
			{
				if (occupation == null)
				{
					return BadRequest();
				}
				await _unitOfWork.OccupationRepository.Put(occupation.Id, occupation);

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
		[HttpDelete("{occupationid}")]
		public async Task<IActionResult> DeleteOccupation(Guid occupationid)
		{
			try
			{
				Occupation occupation = await _unitOfWork.OccupationRepository.GetById(occupationid);
				if (occupation == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.OccupationRepository.Delete(occupationid, occupation);
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
