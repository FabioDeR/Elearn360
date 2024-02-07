using elearn360.API.Config.Email.EmailServices;
using eLearn360.Data.Models;
using eLearn360.Data.VM.AccountVM;
using eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eLearn360.API.Controllers.BaseController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private UserManager<User> _userManager;


        public OrganizationController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Organization> organizationsList = await _unitOfWork.OrganizationRepository.Get();
                return Ok(organizationsList);
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
        public async Task<IActionResult> GetByNameAndCp(Guid id)
        {
            try
            {
                Organization organization = await _unitOfWork.OrganizationRepository.GetById(id);
                return Ok(organization);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }
        }
        #endregion

        /*api/Oraganization/getalluser/{organizationid}*/
        #region GetAll User By Id
        [HttpGet("getalluser/{organizationid}")]
        public async Task<IActionResult> GetAllUserById(Guid organizationid)
        {
            try
            {
                Organization organization = await _unitOfWork.OrganizationRepository.GetAllUserByOrganizationId(organizationid);
                return Ok(organization);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Organization organization)
        {
            try
            {
                string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
                if (organization == null)
                {
                    return BadRequest();
                }
                if (organization.ImageUrl == null)
                {
                    organization.ImageUrl = $"https://{currentUrl}/api/Images/ImageByDefault/95f98557-de39-45e7-9319-2a3dc088f8c2-defaultImg.png";
                }
                await _unitOfWork.OrganizationRepository.Post(organization);
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
        public async Task<IActionResult> Put([FromBody] Organization organization)
        {
            try
            {
                if (organization == null)
                {
                    return BadRequest();
                }
                await _unitOfWork.OrganizationRepository.Put(organization.Id, organization);
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
        [HttpDelete("{organizationid}")]
        public async Task<IActionResult> DeleteOrganization(Guid organizationid)
        {
            try
            {
                Organization organization = await _unitOfWork.OrganizationRepository.GetById(organizationid);
                if (organization == null)
                {
                    return NotFound("Item not found !");
                }
                else
                {
                    await _unitOfWork.OrganizationRepository.Delete(organizationid, organization);
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
        /*api/Oragnization/addnewuser*/
        #region Add New User And Occupation
        [HttpPost("addnewuser")]
        public async Task<IActionResult> AddNewUserAndOccupationByOrganizationId([FromBody] UserHasOccupationVM userHasOccupationvm)
        {
            try
            {
                string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
                (bool,string) success = await _unitOfWork.OrganizationRepository.AddNewUserByOrganizationId(userHasOccupationvm);
                if (success.Item1)
                {
                    User newUser = await _userManager.FindByEmailAsync(userHasOccupationvm.LoginMail);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = newUser.Email}, Request.Scheme);
                    var message = new Message(new string[] { newUser.Email }, "Email de confirmation", confirmationLink, null);
                    await _emailSender.SendMailConfirmation(message,success.Item2);
                }
                else
                {
                    Organization organization = await _unitOfWork.OrganizationRepository.GetById(userHasOccupationvm.OrganizationId);
                    var message = new Message(new string[] { userHasOccupationvm.LoginMail }, $"{organization.Name}", organization.Name, null);
                    await _emailSender.SendMailOrganizationAssociation(message);
                }
                return Ok("User has been added !!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Update UserOccupation
        [HttpPost("updateuserhasoccupation")]
        public async Task<IActionResult> UpdateUserOccupation([FromBody] UserHasOccupation userHasOccupation)
        {
            try
            {
                await _unitOfWork.OrganizationRepository.UpdateUserOccupation(userHasOccupation);
                return Ok("Item has updated !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Remove UserHasOccupation
        [HttpPost("remouserhasoccupation")]
        public async Task<IActionResult> RemoveUserHasOccupation([FromBody] UserHasOccupation userHasOccupation)
        {
            try
            {
                await _unitOfWork.OrganizationRepository.RemoveUserHasOccupation(userHasOccupation);
                return Ok("Item has updated !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region ConfirmEmail
        [HttpGet("confirmationemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    /*"https://{currentUrl}/login"*/
                    return Redirect("https://elearn360ui.azurewebsites.net/login");
                }
                else
                {
                    return BadRequest(new RegisterResultVM()
                    {
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }

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
