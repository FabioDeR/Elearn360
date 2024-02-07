using eLearn360.Data.Models;
using eLearn360.Data.VM.GroupVM;
using eLearn360.Data.VM.Policies;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Group> groupsList = await _unitOfWork.GroupRepository.Get();
                return Ok(groupsList);
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
                Group group = await _unitOfWork.GroupRepository.GetById(id);
                return Ok(group);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }

        #endregion
        #region GetByOrganizationId
        [HttpGet("getbyorganizationid/{organizationid}")]
        public async Task<IActionResult> GetByOrganizationId(Guid organizationid)
        {
            try
            {
                List<Group> group = await _unitOfWork.GroupRepository.GetGroupsByOrganizationId(organizationid);
                return Ok(group);
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
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> Post([FromBody] Group group)
        {
            try
            {
                string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
                if (group.ImageUrl == null)
                {
                    group.ImageUrl = $"https://{currentUrl}/api/Images/ImageByDefault/95f98557-de39-45e7-9319-2a3dc088f8c2-defaultImg.png";
                }
                if (group == null)
                {
                    return BadRequest();
                }

                await _unitOfWork.GroupRepository.Post(group);

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
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> Put([FromBody] Group group)
        {
            try
            {
                if (group == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.GroupRepository.Put(group.Id, group);
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
        [HttpDelete("{groupid}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> DeleteGroup(Guid groupid)
        {
            try
            {
                Group group = await _unitOfWork.GroupRepository.GetById(groupid);
                if (group == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.GroupRepository.Delete(groupid, group);
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
        #region Promote
        [HttpPost("promote")]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> Promote([FromBody] PromoteVM promoteVM)
        {
            try
            {
                await _unitOfWork.GroupRepository.Promote(promoteVM);
                return Ok("User has been promoted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Graduate
        [HttpPost("graduate")]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> Graduate([FromBody] UserHasGroup userHasGroup)
        {
            try
            {
                await _unitOfWork.GroupRepository.Graduate(userHasGroup);
                return Ok("Item has been updated");  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove
        [HttpPost("removeuserhasgroup")]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> RemoveUserHasGroup([FromBody] UserHasGroup userHasGroup)
        {
            try
            {
                await _unitOfWork.GroupRepository.RemoveUserHasGroup(userHasGroup);
                return Ok("Item has been removed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion      
        #region Get All User by GroupId
        [HttpGet("getteacher/{groupid}")]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> GetTeacherList(Guid groupid)
        {
            try
            {
                Group group = await _unitOfWork.GroupRepository.GetTeacherList(groupid);
                return Ok(group);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get All User by GroupId
        [HttpGet("getstudent/{groupid}")]
        [Authorize(Roles = "Admin,Professor")]
        public async Task<IActionResult> GetStudentList(Guid groupid)
        {
            try
            {
                Group group = await _unitOfWork.GroupRepository.GetStudentList(groupid);
                return Ok(group);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get All User Not in Group
        [HttpGet("allusersnotingroup/{groupid}/{organizationid}")]
        [Authorize(Roles = "Admin,Professor" )]
        public async Task<IActionResult> GetallUsersNotInGroup(Guid groupid, Guid organizationid)
        {
            try
            {
                List<User> users = await _unitOfWork.GroupRepository.GetUserstNotInGroup(groupid, organizationid);
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Student Group By UserId && OrganizationId
        [HttpGet("getstudentgroupbyuserandorga/{organizationid}/{userid}")]
        [Authorize(Policy = Policies.IsStudent)]
        public async Task<IActionResult> GetGroupsByUserId(Guid organizationid, Guid userid)
        {
            try
            {
                List<Group> groupss = await _unitOfWork.GroupRepository.GetStudentGroupsByUserAndOrgaId(organizationid, userid);
                return Ok(groupss);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Get All Group by UserId && OrganizationId
        [HttpGet("getmygroups/{userid}/{organizationid}")]
       
        public async Task<IActionResult> GetMyGroups(Guid userid,Guid organizationid)
        {
            try
            {
                List<Group> groups = await _unitOfWork.GroupRepository.GetMyGroups(userid, organizationid);
                return Ok(groups);
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
