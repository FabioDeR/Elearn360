using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class CategoryRepository : Repository<Guid, Category>, ICategoryRepository
    {
		private readonly DbSet<Category> _entities;

        public CategoryRepository(DbContext context) : base(context)
        {
        }
    }
}
