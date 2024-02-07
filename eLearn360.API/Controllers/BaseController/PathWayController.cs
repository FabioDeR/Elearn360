using eLearn360.Data.Models;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class PathWayController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;
		private UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PathWayController(UserManager<User> userManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<PathWay> pathsList = await _unitOfWork.PathWayRepository.Get();
				return Ok(pathsList);
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
				PathWay path = await _unitOfWork.PathWayRepository.GetById(id);
				return Ok(path);
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
		public async Task<IActionResult> Post([FromBody] PathWay pathWay)
		{
			string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
			try
			{
				if (pathWay == null)
				{
					return BadRequest();
				}
				if (pathWay.ImageUrl == null)
				{
					pathWay.ImageUrl = $"https://{currentUrl}/api/Images/ImageByDefault/95f98557-de39-45e7-9319-2a3dc088f8c2-defaultImg.png";
				}

				PathWay newPath = await _unitOfWork.PathWayRepository.Post(pathWay);
				return Ok(pathWay.Id);
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
		public async Task<IActionResult> Put([FromBody] PathWay path)
		{
			try
			{
				if (path == null)
				{
					return BadRequest();
				}
				await _unitOfWork.PathWayRepository.Put(path.Id, path);
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
		[HttpDelete("{pathid}")]
		public async Task<IActionResult> DeletePath(Guid pathid)
		{
			try
			{
				PathWay pathWay = await _unitOfWork.PathWayRepository.GetById(pathid);
				if (pathWay == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.PathWayRepository.Delete(pathid, pathWay);
					return Ok("Item (soft) deleted !");
				}
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion
		#region Get Private/Public Section By UserId
		/*api/PathWay/privatepathway/userid*/
		[HttpGet("privatepathway/{userid}")]
		public async Task<IActionResult> GetPrivatePathWayByUserId(Guid userid)
		{
			try
			{
				List<PathWay> privatePathWay = await _unitOfWork.PathWayRepository.GetPrivatePathWayByUserId(userid);
				return Ok(privatePathWay);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		/*api/PathWay/publicpathway/userid*/
		[HttpGet("publicpathway/{userid}")]
		public async Task<IActionResult> GetPublicPathWayByUserId(Guid userid)
		{
			try
			{
				List<PathWay> publicPathWay = await _unitOfWork.PathWayRepository.GetPublicPathWayByUserId(userid);
				return Ok(publicPathWay);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion
		#region Remove PathWayHasCourse
		/*api/PathWay/removepathwayhascourse*/
		[HttpPost("removepathwayhascourse")]
		public async Task<IActionResult> RemovePathWayHasCourse([FromBody] PathWayHasCourse pathWayHasCourse)
		{
			try
			{
				await _unitOfWork.PathWayRepository.RemovePathWayHasCourse(pathWayHasCourse);
				return Ok("Item has been removed");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
        #endregion
        #region Remove PathWayHasGroup
        /*api/PathWay/removepathwayhasgroup*/
        [HttpPost("removepathwayhasgroup")]
        public async Task<IActionResult> RemovePathWayHasGroup([FromBody] PathWayHasGroup pathWayHasGroup)
        {
            try
            {
                await _unitOfWork.PathWayRepository.RemovePathWayHasGroup(pathWayHasGroup);
                return Ok("Item has been removed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Update Position/Delete relation
        /*api/PathWay/updatepositionordelete*/
        [HttpPost("updatepositionordelete")]
		public async Task<IActionResult> UpdateOrDelete([FromBody] List<PathWayHasCourse> pathWayHasCourses)
		{
			try
			{
				await _unitOfWork.PathWayRepository.UpdateOrDeleted(pathWayHasCourses);
				return Ok("The Items have been modified");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
        #endregion
        #region Get PathWay By GroupId
        [HttpGet("pathwaybygroupid/{groupid}")]
		public async Task<IActionResult> GetPathWaysById(Guid groupid)
        {
            try
            {
				List<PathWay> pathWays = await _unitOfWork.PathWayRepository.GetPathWaysByGroupId(groupid);
				return Ok(pathWays);			
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }

		#endregion        
		
		#region Get PathWay By OrganizationId
        [HttpGet("pathwaybyorganizationid/{organizationid}/{userid}")]
		public async Task<IActionResult> GetPathWaysByOrganizationId(Guid organizationid, Guid userid)
        {
            try
            {
				List<PathWay> pathWays = await _unitOfWork.PathWayRepository.GetPathWaysByOrgaId(organizationid, userid);
				return Ok(pathWays);			
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }

		#endregion
		#region Get PathWayHasCourse by PathId
		[HttpGet("pathwayhascourse/{pathwayid}")]
		public async Task<IActionResult> GetPathWayHasCourse(Guid pathwayid)
		{
			try
			{
				List<PathWayHasCourse> pathWayHasCourses = await _unitOfWork.PathWayRepository.GetIncludePathWayHasCourse(pathwayid);
				return Ok(pathWayHasCourses);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion
		#region Get Course Guid
		[HttpGet("courseguid/{pathid}")]
		public async Task<IActionResult> GetLessonGuid(Guid pathid)
		{
			try
			{
				List<Guid> courseGuid = await _unitOfWork.PathWayRepository.GetCourseGuid(pathid);
				return Ok(courseGuid);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion		
		#region Get Group Guid
		[HttpGet("groupguid/{pathid}")]
		public async Task<IActionResult> GetGroupGuid(Guid pathid)
		{
			try
			{
				List<Guid> groupGuid = await _unitOfWork.PathWayRepository.GetGroupGuid(pathid);
				return Ok(groupGuid);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion

		#region Get Path by PathId
		[HttpGet("pathbypathid/{pathid}/{userid}")]
		public async Task<IActionResult> GetPathById(Guid pathid, Guid userid)
		{
			try
			{
				List<PathWay> paths = await _unitOfWork.PathWayRepository.GetPathByPathid(pathid, userid);
				return Ok(paths);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region  Get Teacher Path By User And OrgaId
		[HttpGet("getteacherpathbyuserandorgaid/{userid}/{organizationid}")]
		public async Task<IActionResult> GetPathByUserAndOrgaIdNoStudent(Guid userid, Guid organizationid)
		{
			try
			{
				List<PathWay> paths = await _unitOfWork.PathWayRepository.GetPathByUserAndOrgaIdNoStudent(userid, organizationid);
				return Ok(paths);
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion

		#region Get path percentage
		[HttpGet("getpathpercentage/{pathid}/{userid}")]
		public async Task<IActionResult> GetCoursePercentage(Guid pathid, Guid userid)
		{
			try
			{
				int percentage = await _unitOfWork.PathWayRepository.GetPathPercentage(pathid, userid);
				return Ok(percentage);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion
	}
}
