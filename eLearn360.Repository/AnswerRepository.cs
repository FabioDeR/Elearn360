using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class AnswerRepository : Repository<Guid, Answer>, IAnswerRepository
    {
        private DbSet<Answer> _entities;
        private Elearn360DBContext _context;

        public AnswerRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Answer>();
        }

        public async override Task<IEnumerable<Answer>> Get()
        {
            IEnumerable<Answer> Answers = await _entities.Where(x => x.DeleteDate == null).ToListAsync();
            return Answers;
        }

        public async override Task<Answer> GetById(Guid id)
        {
            Answer Answer = await _entities.Where(x => x.Id == id).FirstOrDefaultAsync();
            return Answer;
        }
    }
}
