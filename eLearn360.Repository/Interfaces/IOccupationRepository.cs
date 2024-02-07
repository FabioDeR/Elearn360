using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IOccupationRepository : IRepository<Guid,Occupation>
    {
        Task<List<Occupation>> GetOccupationUserByOrga(Guid userId, Guid orgaId);
    }
}
