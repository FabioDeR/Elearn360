using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface ILevelService
    {
        Task<Level> Add(Level level);
        Task<HttpResponseMessage> Delete(int levelId);
        Task<IEnumerable<Level>> GetAll();
        Task<Level> GetById(int levelId);
        Task Update(Level level);
    }
}
