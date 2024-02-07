using eLearn360.Data.Models;
using eLearn360.Data.VM.CourseVM;
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
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = Policies.IsSuperAdmin)]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Course> shifts = await _unitOfWork.CourseRepository.Get();
                return Ok(shifts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{courseid}")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> GetById(Guid courseid)
        {
            try
            {
                Course course = await _unitOfWork.CourseRepository.GetById(courseid);
                if (course == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    return Ok(course);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getallguids/{courseid}")]
        [Authorize]
        public async Task<IActionResult> GetAllGuid(Guid courseid)
        {
            try
            {
                List<Guid> guids = await _unitOfWork.CourseRepository.GetAllGuidByCourseId(courseid);
                return Ok(guids);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getviewcontentvm/{id}")]
        [Authorize]
        public async Task<IActionResult> GetViewContent(Guid id)
        {
            try
            {
                ViewContentVM viewContentVM = await _unitOfWork.CourseRepository.GetViewContentById(id);
                return Ok(viewContentVM);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getcoursetolesson/{courseid}")]
        [Authorize]
        public async Task<IActionResult> GetCourseToLesson(Guid courseid)
        {
            try
            {
                Course course = await _unitOfWork.CourseRepository.GetCourseTolesson(courseid);
                return Ok(course);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> Post([FromBody] Course course)
        {
            try
            {
                
                if (course == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.CourseRepository.Post(course);
                    return Ok(course.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> Put([FromBody] Course course)
        {
            try
            {
                if (course == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.CourseRepository.Put(course.Id, course);
                    return Ok("Item updated !");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

      
        [HttpDelete("{courseid}")]
        [Authorize(Roles = "Admin, SuperAdmin, Professor")]
        public async Task<IActionResult> Delete(Guid courseid)
        {
            try
            {
                Course course = await _unitOfWork.CourseRepository.GetById(courseid);
                if (course == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.CourseRepository.Delete(courseid, course);
                    return Ok("Item (soft) deleted !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        #region Get Private/Public Section By UserId
        /*api/Course/privatecourse/userid*/
        [HttpGet("privatecourse/{userid}")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> GetPrivateCourseByUserId(Guid userid)
        {
            try
            {
                List<Course> privateCourse = await _unitOfWork.CourseRepository.GetPrivateCourseByUserId(userid);
                return Ok(privateCourse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /*api/Course/publiccourse/userid*/
        [HttpGet("publiccourse/{userid}")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> GetPublicCourseByUserId(Guid userid)
        {
            try
            {
                List<Course> publicCourse = await _unitOfWork.CourseRepository.GetPublicCourseByUserId(userid);
                return Ok(publicCourse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Duplicate
        /*api/Course/duplicatecourse/userid/courseid*/
        [HttpGet("duplicatecourse/{courseid}/{userid}")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> DuplicateLesson(Guid userid, Guid courseid)
        {
            try
            {
                await _unitOfWork.CourseRepository.DuplicateCourse(courseid, userid);
                return Ok("Item has been duplicated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove CourseHasSection
        /*api/Course/removecoursehascourse*/
        [HttpPost("removecoursehasscourse")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> RemoveCourseHasSection([FromBody] CourseHasSection courseHasSection)
        {
            try
            {
                await _unitOfWork.CourseRepository.RemoveCouseHasSection(courseHasSection);
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
        /*api/Course/updatepositionordelete*/
        [HttpPost("updatepositionordelete")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> UpdateOrDelete([FromBody] List<CourseHasSection> courseHasSections)
        {
            try
            {
                await _unitOfWork.CourseRepository.UpdateOrDeleted(courseHasSections);
                return Ok("The Items have been modified");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Courses by GroupId
        [HttpGet("coursesbygroupid/{groupid}")]
        public async Task<IActionResult> GetCoursesbyGroupid(Guid groupid)
        {
            try
            {
                (List<Course> courses,int numberofcourse) = await _unitOfWork.CourseRepository.GetCoursesByGroupeId(groupid);
                return Ok(new {courses,numberofcourse});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Section Guid
        [HttpGet("sectionguid/{courseid}")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> GetSectionGuid(Guid courseid)
        {
            try
            {
                List<Guid> sectionGuid = await _unitOfWork.CourseRepository.GetSectionGuid(courseid);
                return Ok(sectionGuid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get SectionHaslesson by Section Id
        [HttpGet("coursehassection/{courseid}")]
        [Authorize(Policy = Policies.IsProfessor)]
        public async Task<IActionResult> GetCourseHasSection(Guid courseid)
        {
            try
            {
                List<CourseHasSection> courseHasSections = await _unitOfWork.CourseRepository.GetIncludeCourseHasSection(courseid);
                return Ok(courseHasSections);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get course By PathId And UserId
        [HttpGet("coursebypathanduserid/{pathid}/{userid}")]

        public async Task<IActionResult> GetCourseByPathAndUserId(Guid pathid, Guid userid)
        {
            try
            {
                List<Course> courses = await _unitOfWork.CourseRepository.GetCourseByPathAndUserId(pathid, userid);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get course By PathId And UserId With Historiccourse
        [HttpGet("coursebypathanduseridwithhistoriccourse/{pathid}/{userid}")]
        public async Task<IActionResult> GetCourseByPathAndUserIdWithHistoricCourse(Guid pathid, Guid userid)
        {
            try
            {
                List<Course> courses = await _unitOfWork.CourseRepository.GetCourseByPathAndUserIdWithHistoricCourse(pathid, userid);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get course percentage
        [HttpGet("getcoursepercentage/{courseid}/{userid}")]
        public async Task<IActionResult> GetCoursePercentage(Guid courseid, Guid userid)
        {
            try
            {
                int percentage = await _unitOfWork.CourseRepository.GetCoursePercentage(courseid, userid);
                return Ok(percentage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        [HttpGet("allguidseen/{courseid}/{userid}")]
        [Authorize(Policy = Policies.IsStudent)]
        public async Task<IActionResult> AllGuidSeen(Guid courseid, Guid userid)
        {
            try
            {
                List<Guid> guids = await _unitOfWork.CourseRepository.AllGuidSeen(courseid, userid);
                return Ok(guids);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
