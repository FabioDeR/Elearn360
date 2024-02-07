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
    public class SectionController : ControllerBase
    {
		private IUnitOfWork _unitOfWork;
		private UserManager<User> _userManager;


		public SectionController(UserManager<User> userManager, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}


		#region GetAll
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				IEnumerable<Section> sectionsList = await _unitOfWork.SectionRepository.Get();
				return Ok(sectionsList);
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
				Section section = await _unitOfWork.SectionRepository.GetById(id);
				return Ok(section);
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
		public async Task<IActionResult> Post([FromBody] Section section)
		{
			try
			{
				if (section == null)
				{
					return BadRequest();
				}
		        await _unitOfWork.SectionRepository.Post(section);
				return Ok(section.Id);
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
		public async Task<IActionResult> Put([FromBody] Section section)
		{
			try
			{
				if (section == null)
				{
					return BadRequest();
				}
				await _unitOfWork.SectionRepository.Put(section.Id, section);
				return Ok();
			}
			catch (Exception err)
			{
				Console.WriteLine(err);
				return BadRequest(err);
			}
		}
		#endregion
		#region DeleteSection
		[HttpDelete("{sectionid}")]
		public async Task<IActionResult> DeleteSection(Guid sectionid)
		{
			try
			{
				Section section = await _unitOfWork.SectionRepository.GetById(sectionid);
				if (section == null)
				{
					return NotFound("Item not found !");
				}
				else
				{
					await _unitOfWork.SectionRepository.Delete(sectionid, section);
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
        #region Get Public/Private Section By UserId
		/*api/Section/privatesection/sectionid*/
        [HttpGet("privatesection/{userid}")]
		public async Task<IActionResult> GetPrivateSection(Guid userid)
        {
            try
            {
				List<Section> privateSection = await _unitOfWork.SectionRepository.GetPrivateSection(userid);
				return Ok(privateSection);
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
		/*api/Section/publicsection/sectionid */
		[HttpGet("publicsection/{userid}")]
		public async Task<IActionResult> GetPublicSection(Guid userid)
		{
			try
			{
				List<Section> publicSection = await _unitOfWork.SectionRepository.GetPublicSection(userid);
				return Ok(publicSection);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		#endregion
		#region Duplicate Section
		/*api/Section/duplicatesection/userid/sectionid */
		[HttpGet("duplicatesection/{userid}/{sectionid}")]
		public async Task<IActionResult> DuplicationSection(Guid userid,Guid sectionid)
        {
            try
            {
				await _unitOfWork.SectionRepository.DuplicateSection(userid, sectionid);
				return Ok("Item has been duplicated");
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove SectionHasLesson
		/*api/Section/removesectionhaslesson*/
        [HttpPost("removesectionhaslesson")]
		public async Task<IActionResult> RemoveSectionHasLesson([FromBody] SectionHasLesson sectionHasLesson)
        {
            try
            {
				await _unitOfWork.SectionRepository.RemoveSectionHaslesson(sectionHasLesson);
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
		/*api/Section/updatepositionordelete*/
        [HttpPost("updatepositionordelete")]
		public async Task<IActionResult> UpdateOrDelete([FromBody] List<SectionHasLesson> sectionHasLessons)
        {
            try
            {
				await _unitOfWork.SectionRepository.UpdateOrDeleted(sectionHasLessons);
				return Ok("The Items have been modified");
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Lesson Guid
        [HttpGet("lessonguid/{sectionid}")]
		public async Task<IActionResult> GetLessonGuid(Guid sectionid)
        {
            try
            {
				List<Guid> lessonGuid = await _unitOfWork.SectionRepository.GetLessonGuid(sectionid);
				return Ok(lessonGuid);
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get SectionHaslesson by Section Id
        [HttpGet("sectionhaslesson/{sectionid}")]
		public async Task<IActionResult> GetSectionHasLesson(Guid sectionid)
        {
            try
            {
				List<SectionHasLesson> sectionHasLessons = await _unitOfWork.SectionRepository.GetIncludeSectionHasLesson(sectionid);
				return Ok(sectionHasLessons);
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                throw;
            }
        }
		#endregion
		#region Section By Path Id
		[HttpGet("sectionbypathid/{pathid}/{userid}")]
		public async Task<IActionResult> GetSectionByPathId(Guid pathid, Guid userid)
		{
			try
			{
				List<Section> section = await _unitOfWork.SectionRepository.GetSectionByPathid(pathid, userid);
				return Ok(section);
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
