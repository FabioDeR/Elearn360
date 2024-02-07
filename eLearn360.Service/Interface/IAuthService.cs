using elearn.Data.ViewModel.AccountVM;
using eLearn360.Data.VM.AccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IAuthService
    {
        Task<LoginResultVM> Login(LoginVM login);
        Task<RegisterResultVM> Register(RegisterVM register);
        Task Logout();
        Task RefreshTokenAuthorize(RefreshTokenVM refreshTokenVM);
        Task<HttpResponseMessage> ChangePassword(AccountChangePasswordVM changePassword);
        Task<HttpResponseMessage> ResetPasswordByGuid(Guid userid);
        Task<HttpResponseMessage> ResetPasswordByMail(string mail);
    }
}
