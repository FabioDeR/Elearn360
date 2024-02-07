using elearn.Data.ViewModel.QuestionVM;
using eLearn360.Data.Models;
using eLearn360.Data.VM.ReportVM;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizzController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public QuizzController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Quizz> Quizzs = await _unitOfWork.QuizzRepository.Get();
                return Ok(Quizzs);
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
                Quizz Quizz = await _unitOfWork.QuizzRepository.GetById(id);
                return Ok(Quizz);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Get By Id with include
        [HttpGet("withinclude/{id}")]
        public async Task<IActionResult> GetByIdWithInclude(Guid id)
        {
            try
            {
                Quizz Quizz = await _unitOfWork.QuizzRepository.GetByIdWithInclude(id);
                return Ok(Quizz);
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
        public async Task<IActionResult> Post([FromBody] Quizz Quizz)
        {
            try
            {
                if (Quizz == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.QuizzRepository.Post(Quizz);
                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Post Quizz
        [HttpPost("quizzgeneration")]
        public async Task<IActionResult> PostQuizz(QuizzVM quizzvm)
        {
            try
            {
                if (quizzvm == null)
                {
                    return BadRequest();
                }

                QuizzVM quizzVM = await _unitOfWork.QuizzRepository.QuizzGeneration(quizzvm);
                return Ok(quizzVM);
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
        public async Task<IActionResult> Put([FromBody] Quizz Quizz)
        {
            try
            {
                if (Quizz == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.QuizzRepository.Put(Quizz.Id, Quizz);
                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Update Rating
        [HttpPut("updaterating")]
        public async Task<IActionResult> PutRating([FromBody] QuizzVM Quizz)
        {
            try
            {
                if (Quizz == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.QuizzRepository.UpdateQuizzRating(Quizz);
                return Ok(Quizz);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        #region Remove Quizz Has Lesson
        [HttpPost("removequizzhaslesson")]
        public async Task<IActionResult> RemoveQuizzHasLesson([FromBody] QuizzHasLesson quizzHasLesson)
        {
            try
            {
                await _unitOfWork.QuizzRepository.RemoveQuizzHasLesson(quizzHasLesson);
                return Ok("Item has been removed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Remove Quizz Has Section
        [HttpPost("removequizzhassection")]
        public async Task<IActionResult> RemoveQuizzHasSection([FromBody] QuizzHasSection quizzHasSection)
        {
            try
            {
                await _unitOfWork.QuizzRepository.RemoveQuizzHasSection(quizzHasSection);
                return Ok("Item has been removed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Remove Quizz Has Course
        [HttpPost("removequizzhascourse")]
        public async Task<IActionResult> RemoveQuestionHasCourse([FromBody] QuizzHasCourse quizzHasCourse)
        {
            try
            {
                await _unitOfWork.QuizzRepository.RemoveQuizzHasCourse(quizzHasCourse);
                return Ok("Item has been removed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Remove Quizz Has PathWay
        [HttpPost("removequizzhaspathway")]
        public async Task<IActionResult> RemoveQuizzHasPathWay([FromBody] QuizzHasPathWay quizzHasPathWay)
        {
            try
            {
                await _unitOfWork.QuizzRepository.RemoveQuizzHasPathWay(quizzHasPathWay);
                return Ok("Item has been removed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete

        [HttpDelete("{quizzid}")]
        public async Task<IActionResult> SoftDelete(Guid quizzid)
        {
            try
            {
                Quizz Quizz = await _unitOfWork.QuizzRepository.GetById(quizzid);
                if (Quizz == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.QuizzRepository.Delete(quizzid, Quizz);
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
