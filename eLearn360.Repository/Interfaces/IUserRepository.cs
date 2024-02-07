using eLearn360.Data.VM;
using eLearn360.Data.VM.AccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IUserRepository : IRepository<Guid, User>
    {
        Task<RegisterResultVM> Register(RegisterVM registervm);
        Task<List<Claim>> CreateClaimsByUser(User user);
        Task<List<Organization>> GetAllOrganizationByUserId(Guid userid);
        Task<List<Claim>> RefreshClaims(RefreshTokenVM refreshTokenVM);
        Task<AccountRegisterEditVM> GetUserProfile(Guid id);
        Task<AccountRegisterEditVM> UserProfileUpdate(AccountRegisterEditVM accountRegisterEditVM);
    }
}
