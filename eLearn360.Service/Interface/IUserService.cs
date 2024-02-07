using eLearn360.Data.Models;
using eLearn360.Data.VM;
using eLearn360.Data.VM.AccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IUserService
    {
        Task <List<Organization>> GetAllOrganizationByUserId(Guid userid);
        Task<AccountRegisterEditVM> GetUserProfile(Guid userid);
        Task<HttpResponseMessage> UserProfileUpdate(AccountRegisterEditVM user);
        Task<string> UploadUserImage(MultipartFormDataContent content);
    }
}
