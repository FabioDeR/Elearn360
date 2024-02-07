using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class LevelRepository : Repository<Guid, Level>, ILevelRepository
	{
		private readonly DbSet<Level> _entities;
		private readonly DbContext _context;

		public LevelRepository(DbContext context) : base(context)
		{
			_context = context;
			_entities = _context.Set<Level>();
		}
	}
}
