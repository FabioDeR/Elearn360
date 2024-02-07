using eLearn360.Data.Models;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public AnswerController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Answer> Answers = await _unitOfWork.AnswerRepository.Get();
                return Ok(Answers);
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
                Answer Answer = await _unitOfWork.AnswerRepository.GetById(id);
                return Ok(Answer);
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
        public async Task<IActionResult> Post([FromBody] Answer Answer)
        {
            try
            {
                if (Answer == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.AnswerRepository.Post(Answer);
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
        public async Task<IActionResult> Put([FromBody] Answer Answer)
        {
            try
            {
                if (Answer == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.AnswerRepository.Put(Answer.Id, Answer);
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

        [HttpDelete("{answerid}")]
        public async Task<IActionResult> SoftDelete(Guid answerid)
        {
            try
            {
                Answer Answer = await _unitOfWork.AnswerRepository.GetById(answerid);
                if (Answer == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.AnswerRepository.Delete(answerid, Answer);
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
