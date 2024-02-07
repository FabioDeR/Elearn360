using eLearn360.Data.Models;
using eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IOrganizationService
    {
        Task<HttpResponseMessage> Add(Organization organization);
        Task<HttpResponseMessage> Update(Organization organization);
        Task<List<Organization>> GetAll();
        Task<Organization> GetById(Guid organizationid);
        Task<HttpResponseMessage> Delete(Guid organizationid);

        Task<Organization> GetAllUserByOrganizationId(Guid organizationid);
        Task<HttpResponseMessage> AddNewUserAndOccupationByOrganizationId(UserHasOccupationVM userHasOccupationvm);
        Task<string> UploadOrganizationImage(MultipartFormDataContent content);
    }
}
