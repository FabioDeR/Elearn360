using elearn.Data.ViewModel.AccountVM;
using elearn360.API.Config.Email.EmailServices;
using eLearn360.Data.DBContext;
using eLearn360.Data.Models;
using eLearn360.Data.VM.AccountVM;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace eLearn360.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Elearn360DBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signinManager,
                              IHttpContextAccessor httpContextAccessor, Elearn360DBContext context, IEmailSender emailSender)

        {
            _userManager = userManager;
            _signInManager = signinManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registervm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
                    registervm.ImageUrl = $"https://{currentUrl}/api/Images/ImageByDefault/DefaultImageUser.png";
                    RegisterResultVM result = await _unitOfWork.UserRepository.Register(registervm);
                    if (result.Success)
                    {
                        User newUser = await _userManager.FindByEmailAsync(registervm.LoginMail);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = newUser.Email }, Request.Scheme);
                        var message = new Message(new string[] { newUser.Email }, "Email de confirmation", confirmationLink, null);
                        await _emailSender.SendMailConfirmation(message,registervm.Password);
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new RegisterResultVM()
                    {
                        Errors = new List<string>() {
                            "Invalid payload"
                        },
                        Success = false
                    });
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region ConfirmEmail        
        [HttpGet]
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
        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);

                    if (!result.Succeeded)
                        return BadRequest(new LoginResultVM
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "L'email ou le mot de passe sont incorrect"
                            }
                        });
                    var user = await _signInManager.UserManager.FindByEmailAsync(loginVM.Email);
                    if (user == null)
                    {
                        return BadRequest(new LoginResultVM()
                        {
                            Errors = new List<string>() {
                                "User don't exist"
                            },
                            Success = false
                        });
                    }
                    List<Claim> claims = await _unitOfWork.UserRepository.CreateClaimsByUser(user);
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
                    var token = new JwtSecurityToken(
                        _configuration["JwtIssuer"],
                        _configuration["JwtAudience"],
                        claims,
                        expires: expiry,
                        signingCredentials: creds
                    );
                    return Ok(new LoginResultVM()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Success = true
                    });

                }
                return BadRequest(new LoginResultVM()
                {
                    Errors = new List<string>() {
                        "Invalid payload"
                    },
                    Success = false
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region RefreshToken
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<Claim> claims = await _unitOfWork.UserRepository.RefreshClaims(refreshTokenVM);
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
                    var token = new JwtSecurityToken(
                        _configuration["JwtIssuer"],
                        _configuration["JwtAudience"],
                        claims,
                        expires: expiry,
                        signingCredentials: creds
                    );
                    return Ok(new RefreshTokenResultVM()
                    {
                        RefreshToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Success = true
                    });


                }
                return BadRequest(new RefreshTokenResultVM()
                {
                    Errors = new List<string>() {
                        "Invalid payload"
                    },
                    Success = false
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region ChangePassword

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] AccountChangePasswordVM changePasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userManager.FindByIdAsync(changePasswordRequest.UserId.ToString());

                if (user == null)
                {
                    return BadRequest();
                   
                }

                IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }

                //Send Mail 
                var message = new Message(new string[] { user.Email }, "Changement de mot de passe", null, null);
                await _emailSender.SendMailPasswordChange(message);

                return Ok();
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
        #endregion
        #region ReinitializePassword by guid
        [HttpGet("passwordefaultresetbyguid")]
        public async Task<IActionResult> PasswordDefaultResetByGuid(Guid userid)
        {

            try
            {
                User userSelected = await _userManager.FindByIdAsync(userid.ToString());
                if (userSelected == null)
                {
                    return BadRequest();
                }
                else
                {
                    //Generate Password 12 caracters
                    string passwordString = eLearn360.Utils.PasswordGestion.PasswordGeneration(11);

                    string token = await _userManager.GeneratePasswordResetTokenAsync(userSelected);
                    IdentityResult result = await _userManager.ResetPasswordAsync(userSelected, token, passwordString);

                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors);
                    }

                    //Send mail with new Password
                    var message = new Message(new string[] { userSelected.Email }, "Réinitialisation de votre mot de passe", passwordString, null);
                    await _emailSender.SendMailPasswordReset(message);

                    return Ok();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(err);
            }

        }
        #endregion
        #region ReinitializePassword by mail
        [HttpGet("passwordefaultresetbymail")]
        public async Task<IActionResult> PasswordDefaultResetByMail(string mail)
        {

            try
            {
                User userSelected = await _userManager.FindByEmailAsync(mail);
                if (userSelected == null)
                {
                    return BadRequest();
                }
                else
                {
                    //Generate Password 12 caracters
                    string passwordString = eLearn360.Utils.PasswordGestion.PasswordGeneration(11);

                    string token = await _userManager.GeneratePasswordResetTokenAsync(userSelected);
                    IdentityResult result = await _userManager.ResetPasswordAsync(userSelected, token, passwordString);

                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors);
                    }

                    //Send mail with new Password
                    var message = new Message(new string[] { userSelected.Email }, "Réinitialisation de votre mot de passe", passwordString, null);
                    await _emailSender.SendMailPasswordReset(message);

                    return Ok();
                }
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
