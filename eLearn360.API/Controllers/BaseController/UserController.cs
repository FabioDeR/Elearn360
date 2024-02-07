using eLearn360.Data.Models;
using eLearn360.Data.VM;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class UserController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;


		public UserController(IUnitOfWork unitOfWork)
		{

			_unitOfWork = unitOfWork;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<User> categorysList = await _unitOfWork.UserRepository.Get();
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
				User user = await _unitOfWork.UserRepository.GetById(id);
				return Ok(user);
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
		public async Task<IActionResult> Post([FromBody] User user)
		{
			try
			{
				if (user == null)
				{
					return BadRequest();
				}
				await _unitOfWork.UserRepository.Post(user);
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
		public async Task<IActionResult> Put([FromBody] User user)
		{
			try
			{
				if (user == null)
				{
					return BadRequest();
				}
				await _unitOfWork.UserRepository.Put(user.Id, user);
				return Ok();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
        #endregion

        #region Get All Organization By UserId
        [HttpGet("getallorganization/{userid}")]
		public async Task<IActionResult> GetAllOrganizationByUserId(Guid userid)
        {
            try
            {
				List<Organization> organizations = await _unitOfWork.UserRepository.GetAllOrganizationByUserId(userid);
				return Ok(organizations);
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Remove User In Organization
        [HttpGet("removeuser/{userid}/{organizationid}")]
		public async Task<IActionResult> RemoveUser(Guid userid, Guid organizationid)
        {
            try
            {
				await _unitOfWork.OrganizationRepository.RemoveUser(userid, organizationid);
				return Ok("User has deleted");
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
		#endregion

		#region Get Profile
		[HttpGet("getuserprofile/{userid}")]
		public async Task<IActionResult> GetUserProfile(Guid userid)
		{
			try
			{
				AccountRegisterEditVM accountRegisterEditVM = await _unitOfWork.UserRepository.GetUserProfile(userid);
				return Ok(accountRegisterEditVM);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Update User Profile
		[HttpPut("userprofileupdate")]
		public async Task<IActionResult> UserProfileUpdate([FromBody] AccountRegisterEditVM user)
		{
			try
			{
				if (user == null)
				{
					return BadRequest();
				}
				await _unitOfWork.UserRepository.UserProfileUpdate(user);
				return Ok();
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
