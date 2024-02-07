using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class SectionRepository : Repository<Guid, Section>, ISectionRepository
    {
        private readonly DbSet<Section> _entities;
        private readonly DbSet<PathWayHasCourse> _entitiesPathWayHasCourse;
        private readonly DbSet<HistoricSectionHasUser> _entitiesHistoricSection;
        private readonly Elearn360DBContext _context;

        public SectionRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Section>();
            _entitiesPathWayHasCourse = _context.Set<PathWayHasCourse>();
            _entitiesHistoricSection = _context.Set<HistoricSectionHasUser>();

        }

        public async override Task<IEnumerable<Section>> Get()
        {
            IEnumerable<Section> sections = await _entities.Where(x => x.DeleteDate == null).AsNoTracking().ToListAsync();
            return sections;
        }

        #region Duplication Section
        public async Task DuplicateSection(Guid userid, Guid sectionid)
        {
            var mytransation = _context.Database.BeginTransaction();
            try
            {
                Section newSection = await _entities.Where(s => s.Id == sectionid).Include(s => s.SectionHasLessons).ThenInclude(l => l.Lesson).AsNoTracking().Select(os => new Section()
                {
                    Name = $"{os.Name}-copie",
                    Description = os.Description,
                    Content = os.Content,
                    PrivateSection = os.PrivateSection,
                    CreationTrackingUserId = userid,
                    SectionHasLessons = os.SectionHasLessons.Select(oshl => new SectionHasLesson()
                    {
                        Position = oshl.Position,
                        Lesson = new Lesson()
                        {
                            Name = $"{oshl.Lesson.Name}-copie",
                            Content = oshl.Lesson.Content,
                            CreationTrackingUserId = userid,
                            Description = oshl.Lesson.Description,
                            PrivateLesson = oshl.Lesson.PrivateLesson,
                        },
                    }).ToList(),

                }).FirstOrDefaultAsync();
                await base.Post(newSection);
                mytransation.Commit();
            }
            catch (Exception ex)
            {
                mytransation.Rollback();
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get Private/Public Section By UserId    
        public async Task<List<Section>> GetPrivateSection(Guid userid)
        {
            try
            {
                List<Section> privateSections = await _entities.Where(l => l.CreationTrackingUserId == userid && l.DeleteDate == null).Select(s => new Section()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreationDate = s.CreationDate
                }).ToListAsync();
                return privateSections;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Section>> GetPublicSection(Guid userid)
        {
            try
            {
                List<Section> publicSections = await _entities.Where(l => l.CreationTrackingUserId != userid && l.PrivateSection == false && l.DeleteDate == null).Select(s => new Section()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreationDate = s.CreationDate

                }).ToListAsync();
                return publicSections;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region UpdatePosition/Delete SectionHasLesson
        public async Task UpdateOrDeleted(List<SectionHasLesson> sectionHasLessons)
        {
            try
            {
                Section updateSection = await _entities.Where(s => s.Id == sectionHasLessons.Select(i => i.SectionId).First()).Include(shl => shl.SectionHasLessons).SingleAsync();
                updateSection.SectionHasLessons.Clear();
                var result = sectionHasLessons.Where( i => i.DeleteDate == null).Select(e => new SectionHasLesson
                {                   
                    CreationDate = e.CreationDate,
                    CreationTrackingUserId = e.CreationTrackingUserId,                   
                    Position = e.Position,
                    SectionId = e.SectionId,
                    LessonId = e.LessonId

                }).ToList();
                updateSection.SectionHasLessons.AddRange(result);
                await base.Put(updateSection.Id,updateSection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove SecionHaslesson
        public async Task RemoveSectionHaslesson(SectionHasLesson sectionHasLesson)
        {
            try
            {
                Section sectionUpdated = await _entities.Where(s => s.Id == sectionHasLesson.SectionId).Include(shl => shl.SectionHasLessons).SingleAsync();
                sectionUpdated.SectionHasLessons.RemoveAll(x => x.SectionId == sectionHasLesson.SectionId && x.LessonId == sectionHasLesson.LessonId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        public async Task<List<SectionHasLesson>> GetIncludeSectionHasLesson(Guid sectionId)
        {
            try
            {
                List<SectionHasLesson> sectionHasLessons = await _entities.Where(s => s.Id == sectionId)
                                                                          .Include(shl => shl.SectionHasLessons)
                                                                          .ThenInclude(l => l.Lesson).AsNoTracking()
                                                                          .SelectMany(shl => shl.SectionHasLessons.Where(d => d.DeleteDate == null))
                                                                          .Select(x => new SectionHasLesson()
                                                                          {
                                                                              Id = x.Id,
                                                                              Lesson = new Lesson()
                                                                              {
                                                                                  
                                                                                  Name = x.Lesson.Name,
                                                                                  Description = x.Lesson.Description,
                                                                                  Content = "x"
                                                                              },
                                                                              LessonId = x.LessonId,
                                                                              Position = x.Position,
                                                                              SectionId = sectionId
                                                                          }).ToListAsync();
                                                                        
                return sectionHasLessons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<List<Guid>> GetLessonGuid(Guid sectionid)
        {
            try
            {
                List<Guid> lessonGuid = await _entities.Where(s => s.Id == sectionid && s.DeleteDate == null)
                                                       .Include(shl => shl.SectionHasLessons)
                                                       .SelectMany(i => i.SectionHasLessons.Where(d => d.DeleteDate == null)).Select(s => s.LessonId).ToListAsync();
                return lessonGuid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Get Section By PathId
        public async Task<List<Section>> GetSectionByPathid(Guid pathId, Guid userId)
        {
            try
            {
                List<Section> sections = (await _entitiesPathWayHasCourse.Where(p => p.PathWayId == pathId)
                                                                   .Include(pc => pc.Course)
                                                                   .ThenInclude(cs => cs.CourseHasSections)
                                                                   .ThenInclude(c => c.Section).ToListAsync())
                                                                   .SelectMany(c => c.Course.CourseHasSections)
                                                                   .SelectMany(l => new List<Section>()
                                                                    {
                                                                        new Section()
                                                                        {
                                                                            Id = l.SectionId,
                                                                            Name = l.Section.Name,
                                                                        }
                                                                    }).ToList();

                List<Guid> historicSectionId = await _entitiesHistoricSection.Where(u => u.UserId == userId).Select(l => l.SectionId).ToListAsync();
                List<Section> sections1 = sections.Where(x => historicSectionId.Contains(x.Id)).ToList();
                return sections1;
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
