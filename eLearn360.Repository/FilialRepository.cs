using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class FilialRepository : Repository<Guid,Filial>,IFilialRepository
    {
		private readonly DbSet<Filial> _entities;
		private readonly Elearn360DBContext _context;

		public FilialRepository(Elearn360DBContext context) : base(context)
		{
			_context = context;
			_entities = _context.Set<Filial>();
		}
	}
}
