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
    public class LessonController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;

        public LessonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Lesson> lessonsList = await _unitOfWork.LessonRepository.Get();
                return Ok(lessonsList);
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
                Lesson lesson = await _unitOfWork.LessonRepository.GetById(id);
                return Ok(lesson);
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
        public async Task<IActionResult> Post([FromBody] Lesson lesson)
        {
            try
            {
                if (lesson == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.LessonRepository.Post(lesson);
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
        public async Task<IActionResult> Put([FromBody] Lesson lesson)
        {
            try
            {
                if (lesson == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.LessonRepository.Put(lesson.Id, lesson);
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
        [HttpDelete("{lessonid}")]
        public async Task<IActionResult> DeleteLesson(Guid lessonid)
        {
            try
            {
                try
                {
                    Lesson lesson = await _unitOfWork.LessonRepository.GetById(lessonid);
                    if (lesson == null)
                    {
                        return NotFound("Item not found !");
                    }
                    else
                    {
                        await _unitOfWork.LessonRepository.Delete(lessonid, lesson);
                        return Ok("Item (soft) deleted !");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion


        #region Get Private/Public Lesson By UserId
        /*api/Lesson/privatelesson/userid*/
        [HttpGet("privatelesson/{userid}")]
        public async Task<IActionResult> GetPrivatelessonByUserId(Guid userid)
        {
            try
            {
                List<Lesson> privatelessons = await _unitOfWork.LessonRepository.GetPrivateLessonByUserId(userid);
                return Ok(privatelessons);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /*api/Lesson/publiclesson/userid*/
        [HttpGet("publiclesson/{userid}")]
        public async Task<IActionResult> GetPubliclessonByUserId(Guid userid)
        {
            try
            {
                List<Lesson> publiclessons = await _unitOfWork.LessonRepository.GetPublicLessonByUserId(userid);
                return Ok(publiclessons);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion
        #region Duplicate
        /*api/Lesson/duplicatelesson/userid/lessonid*/
        [HttpGet("duplicatelesson/{userid}/{lessonid}")]
        public async Task<IActionResult> DuplicateLesson(Guid userid,Guid lessonid)
        {
            try
            {
                await _unitOfWork.LessonRepository.DuplicateLesson(lessonid,userid);
                return Ok("Item has been duplicated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Post Start Historic
        [HttpGet("poststarthistoric")]
        public async Task<IActionResult> PostStartHistoric(Guid userid, Guid itemid)
        {
            try
            {
                if (userid == Guid.Empty || itemid == Guid.Empty)
                {
                    return BadRequest();
                }
                await _unitOfWork.LessonRepository.PostStartHistoric(userid, itemid);
                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion
        #region Update End Historic
        [HttpGet("updateendhistoric")]
        public async Task<IActionResult> UpdateEndHistoric(Guid userid, Guid itemid)
        {
            try
            {
                if (userid == Guid.Empty || itemid == Guid.Empty)
                {
                    return BadRequest();
                }
                await _unitOfWork.LessonRepository.UpdateEndHistoric(userid, itemid);
                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion
        #region Get Lessons by PathId
        [HttpGet("lessonbypathid/{pathid}/{userid}")]
        public async Task<IActionResult> GetLessonByPathId(Guid pathid, Guid userid)
        {
            try
            {
                List<Lesson> lessonsList = await _unitOfWork.LessonRepository.GetLessonByPathid(pathid, userid);
                return Ok(lessonsList);
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
