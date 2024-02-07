using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class OccupationRepository : Repository<Guid, Occupation>, IOccupationRepository
	{
		private readonly DbSet<Occupation> _entities;
		private readonly DbSet<UserHasOccupation> _entitiesUserHasOccupation;
		private readonly DbContext _context;

		public OccupationRepository(DbContext context) : base(context)
		{
			_context = context;
			_entities = _context.Set<Occupation>();
			_entitiesUserHasOccupation = _context.Set<UserHasOccupation>();
		}

		public async Task<List<Occupation>> GetOccupationUserByOrga(Guid userId, Guid orgaId)
        {
            try
            {
				List<Occupation> occupations = await _entitiesUserHasOccupation.Where(x => x.DeleteDate == null &&
																						   x.User.DeleteDate == null &&
																						   x.Occupation.DeleteDate == null &&
																						   x.OrganizationId == orgaId &&
																						   x.UserId == userId)
																			   .Select(x => x.Occupation)
																			   .ToListAsync();
				return occupations;
            }catch(Exception e)
            {
				Console.WriteLine(e);
				throw;
            }
        }
	}
}
