using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class GenderRepository : Repository<Guid, Gender>, IGenderRepository
	{
		private readonly DbSet<Gender> _entities;
		private readonly DbContext _context;

		public GenderRepository(DbContext context) : base(context)
		{
			_context = context;
			_entities = _context.Set<Gender>();
		}
	}
}
