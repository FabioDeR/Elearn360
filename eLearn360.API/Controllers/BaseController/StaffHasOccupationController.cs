using eLearn360.Data.Models;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class StaffHasOccupationController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;


		public StaffHasOccupationController(IUnitOfWork unitOfWork)
		{

			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<StaffOccupation> staffOccupation = await _unitOfWork.StaffOccupationRepository.Get();
				return Ok(staffOccupation);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
        #endregion
        [HttpGet("getbyorganizationid/{organizationid}")]
		public async Task<IActionResult> GetStaffOccupationByOrganizationId(Guid organizationid)
		{
			try
			{
				List<StaffOccupation> staffOccupation = await _unitOfWork.StaffOccupationRepository.GetStaffOccupationByOrganizationId(organizationid);
				return Ok(staffOccupation);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}

		#region GetById
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				StaffOccupation staffOccupation = await _unitOfWork.StaffOccupationRepository.GetById(id);
				return Ok(staffOccupation);
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
		public async Task<IActionResult> Post([FromBody] StaffOccupation staffOccupation)
		{
			try
			{
				if (staffOccupation == null)
				{
					return BadRequest();
				}
				await _unitOfWork.StaffOccupationRepository.Post(staffOccupation);
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
		public async Task<IActionResult> Put([FromBody] StaffOccupation staffOccupation)
		{
			try
			{
				if (staffOccupation == null)
				{
					return BadRequest();
				}
				await _unitOfWork.StaffOccupationRepository.Put(staffOccupation.Id, staffOccupation);
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

		[HttpDelete("{staffid}")]
		public async Task<IActionResult> SoftDelete(Guid staffid)
		{
			try
			{
				StaffOccupation staffOccupation = await _unitOfWork.StaffOccupationRepository.GetById(staffid);
				if (staffOccupation == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.StaffOccupationRepository.Delete(staffid, staffOccupation);
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
