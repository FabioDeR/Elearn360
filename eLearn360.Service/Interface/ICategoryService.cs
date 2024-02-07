using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int categoryId);
        Task<Category> Add(Category category);
        Task<HttpResponseMessage> Delete(int categoryId);
        Task Update(Category category);
    }
}
