using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IOccupationService
    {
        Task<List<Occupation>> GetAll();
        Task<List<Occupation>> GetOccupationUserByOrga(Guid userid, Guid orgaid);
    }
}
