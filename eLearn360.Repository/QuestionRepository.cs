using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class QuestionRepository : Repository<Guid, Question>, IQuestionRepository
    {
        private readonly DbSet<Question> _entities;
        private readonly Elearn360DBContext _context;

        public QuestionRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Question>();
        }

        public async override Task<IEnumerable<Question>> Get()
        {
            try
            {
                IEnumerable<Question> Questions = await _entities.Where(x => x.DeleteDate == null).AsNoTracking().ToListAsync();
                return Questions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        #region Get By QuestionId With include
        public async Task<Question> GetByQuestionIdWithInclude(Guid id)
        {
            try
            {
                Question Question = await _entities.Where(x => x.Id == id).Include(x => x.QuestionHasLessons)
                                                                      .Include(x => x.QuestionHasSections)
                                                                      .Include(x => x.QuestionHasCourses)
                                                                      .Include(x => x.QuestionHasPathWays)
                                                                      .Include(x => x.Answers)
                                                                      .AsNoTracking()
                                                                      .FirstOrDefaultAsync();

                return Question;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Get By LessonId
        public async Task<List<Question>> GetByLessonId(Guid id)
        {
            try
            {
                List<Question> questions = await _entities.Where(x => x.DeleteDate == null && x.Lessons.Any(y => y.Id == id))
                                                                                                                .AsNoTracking()
                                                                                                                .ToListAsync();
                return questions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Get By SectionId
        public async Task<List<Question>> GetBySectionId(Guid id)
        {
            try
            {
                List<Question> questions = await _entities.Where(x => x.DeleteDate == null && x.Sections.Any(y => y.Id == id))
                                                                                                                   .AsNoTracking()
                                                                                                                   .ToListAsync();
                return questions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Get By CourseId
        public async Task<List<Question>> GetByCourseId(Guid id)
        {
            try
            {
                List<Question> questions = await _entities.Where(x => x.DeleteDate == null && x.Courses.Any(y => y.Id == id))
                                                                                                                   .AsNoTracking()
                                                                                                                   .ToListAsync();
                return questions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        #endregion

        #region Get By PathId
        public async Task<List<Question>> GetByPathId(Guid id)
        {
            try
            {
                List<Question> questions = await _entities.Where(x => x.DeleteDate == null && x.PathWays.Any(y => y.Id == id))
                                                                                                                   .AsNoTracking()
                                                                                                                   .ToListAsync();
                return questions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        #endregion

        #region Delete Question Has Lesson
        public async Task RemoveQuestionHasLesson(QuestionHasLesson questionHasLesson)
        {
            try
            {
                Question question = await _entities.Where(s => s.Id == questionHasLesson.QuestionId).Include(qhl => qhl.QuestionHasLessons).SingleAsync();
                question.QuestionHasLessons.RemoveAll(x => x.LessonId == questionHasLesson.LessonId && x.QuestionId == questionHasLesson.QuestionId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Question Has Section
        public async Task RemoveQuestionHasSection(QuestionHasSection questionHasSection)
        {
            try
            {
                Question question = await _entities.Where(s => s.Id == questionHasSection.QuestionId).Include(qhs => qhs.QuestionHasSections).SingleAsync();
                question.QuestionHasSections.RemoveAll(x => x.SectionId == questionHasSection.SectionId && x.QuestionId == questionHasSection.QuestionId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Question Has Course
        public async Task RemoveQuestionHasCourse(QuestionHasCourse questionHasCourse)
        {
            try
            {
                Question question = await _entities.Where(s => s.Id == questionHasCourse.QuestionId).Include(qhc => qhc.QuestionHasCourses).SingleAsync();
                question.QuestionHasCourses.RemoveAll(x => x.CourseId == questionHasCourse.CourseId && x.QuestionId == questionHasCourse.QuestionId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Question Has PathWay
        public async Task RemoveQuestionHasPathWay(QuestionHasPathWay questionHasPathWay)
        {
            try
            {
                Question question = await _entities.Where(s => s.Id == questionHasPathWay.QuestionId).Include(qhp => qhp.QuestionHasPathWays).SingleAsync();
                question.QuestionHasPathWays.RemoveAll(x => x.PathWayId == questionHasPathWay.PathWayId && x.QuestionId == questionHasPathWay.QuestionId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion



    }
}
