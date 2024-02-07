using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IGenderService
    {
        Task<HttpResponseMessage> Add(Gender gender);
        Task<HttpResponseMessage> Delete(Guid genderid, Guid userid);
        Task<List<Gender>> GetAll();
        Task<Gender> GetById(Guid genderid);
        Task<HttpResponseMessage> Update(Gender gender);
    }
}
