using eLearn360.Data.VM.CourseVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class CourseRepository : Repository<Guid,Course>, ICourseRepository
    {
        private readonly DbSet<Course> _entities;
        private readonly DbSet<Section> _entitiesSection;
        private readonly DbSet<Lesson> _entitiesLesson;
        private readonly DbSet<Group> _entitiesGroup;
        private readonly DbSet<User> _entitiesUser;
        private readonly DbSet<PathWayHasCourse> _entitiesPathWayHasCourse;
        private readonly DbSet<HistoricLessonHasUser> _entitiesHistoricLesson;
        private readonly DbSet<HistoricSectionHasUser> _entitiesHistoricHasSection;
        private readonly DbSet<HistoricCourseHasUser> _entitiesHistoricCourse;
        private readonly DbSet<CourseHasSection> _entitiesCourseHasSection;
                                                                                                                                                              
        private readonly Elearn360DBContext _context;


		public CourseRepository(Elearn360DBContext context) : base(context)
		{
			_context = context;
			_entities = _context.Set<Course>();
            _entitiesGroup = _context.Set<Group>();
            _entitiesSection = _context.Set<Section>();
            _entitiesLesson = _context.Set<Lesson>();
            _entitiesUser = _context.Set<User>();
            _entitiesPathWayHasCourse = _context.Set<PathWayHasCourse>();
            _entitiesHistoricLesson = _context.Set<HistoricLessonHasUser>();
            _entitiesHistoricCourse = _context.Set<HistoricCourseHasUser>();
            _entitiesHistoricHasSection = _context.Set<HistoricSectionHasUser>();
            _entitiesCourseHasSection = _context.Set<CourseHasSection>();
        }

        public async override Task<IEnumerable<Course>> Get()
        {
            IEnumerable<Course> courses = await _entities.Where(x => x.DeleteDate == null).AsNoTracking().ToListAsync();
            return courses;
        }

        public async override Task<Course> GetById(Guid id)
        {
            try
            {
                Course course = await _entities.Where(s => s.Id == id).AsNoTracking().SingleAsync();
                return course;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }


        #region DuplicateCourse
        public async Task DuplicateCourse(Guid courseid, Guid userid)
        {
            var mytransation = _context.Database.BeginTransaction();
            try
            {
                Course newCourse = await _entities.Where(c => c.Id == courseid)
                                                  .Include(chs => chs.CourseHasSections)
                                                  .ThenInclude(s => s.Section)
                                                  .ThenInclude(shl => shl.SectionHasLessons)
                                                  .ThenInclude(l => l.Lesson)
                                                  .AsNoTracking()
                                                  .Select(oc => new Course()
                                                  {
                                                      Name = $"{oc.Name}-copie",
                                                      Content = oc.Content,
                                                      CategoryId = oc.CategoryId,
                                                      ImageUrl = oc.ImageUrl,
                                                      LevelId = oc.LevelId,
                                                      CreationTrackingUserId = userid,
                                                      CourseHasSections = oc.CourseHasSections.Where(d => d.DeleteDate == null).Select(ochs => new CourseHasSection()
                                                      {
                                                          Position = ochs.Position,
                                                          Section = new Section()
                                                          {
                                                              Name = $"{ochs.Section.Name}-copie",
                                                              Content = ochs.Section.Content,
                                                              Description = ochs.Section.Description,
                                                              PrivateSection = ochs.Section.PrivateSection,
                                                              CreationTrackingUserId = userid,
                                                              SectionHasLessons = ochs.Section.SectionHasLessons.Where(d => d.DeleteDate == null).Select(oshl => new SectionHasLesson()
                                                              {
                                                                  Position = oshl.Position,
                                                                  Lesson = new Lesson()
                                                                  {
                                                                      Name = $"{oshl.Lesson.Name}-copie",
                                                                      Content = oshl.Lesson.Content,
                                                                      CreationDate = oshl.Lesson.CreationDate,
                                                                      CreationTrackingUserId = userid,
                                                                      PrivateLesson = oshl.Lesson.PrivateLesson,
                                                                      Description = oshl.Lesson.Description
                                                                  }
                                                                  
                                                              }).ToList(),
                                                          },
                                                      }).ToList(),                                                     
                                                  })
                                                  .FirstOrDefaultAsync();
                await base.Post(newCourse);
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
        #region Get Private/Public Section
        public async Task<List<Course>> GetPrivateCourseByUserId(Guid userid)

        {
            try
            {
                List<Course> privateCourses = await _entities.Where(l => l.CreationTrackingUserId == userid && l.DeleteDate == null).ToListAsync();
                return privateCourses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Course>> GetPublicCourseByUserId(Guid userid)
        {
            try
            {
                List<Course> publicCourse = await _entities.Where(l => l.CreationTrackingUserId != userid && l.PrivateCourse == false && l.DeleteDate == null).ToListAsync();
                return publicCourse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove CourseHasSection
        public async Task RemoveCouseHasSection(CourseHasSection courseHasSection)
        {
            try
            {
                Course courseUpdated = await _entities.Where(s => s.Id == courseHasSection.CourseId).Include(shl => shl.CourseHasSections).SingleAsync();
                courseUpdated.CourseHasSections.RemoveAll(x => x.SectionId == courseHasSection.SectionId && x.CourseId == courseHasSection.CourseId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region UpdatePosition/Delete CourseHasSections
        public async Task UpdateOrDeleted(List<CourseHasSection> courseHasSections)
        {
            try
            {
                Course updateCourse = await _entities.Where(s => s.Id == courseHasSections.Select(i => i.CourseId).First()).Include(shl => shl.CourseHasSections).SingleAsync();
                updateCourse.CourseHasSections.Clear();
                var result = courseHasSections.Select(e => new CourseHasSection
                {                   
               
              
                    Position = e.Position,
                    SectionId = e.SectionId,
                    CourseId = e.CourseId

                }).ToList();
                updateCourse.CourseHasSections.AddRange(result);
                await base.Put(updateCourse.Id, updateCourse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion       
        #region Get Course By GroupId
        public async Task<(List<Course>,int)> GetCoursesByGroupeId(Guid groupeId)
        {
            try
            {
                List<Course> courses = await _entitiesGroup.Where(c => c.Id == groupeId)
                                                           .Include(c => c.Courses)
                                                           .SelectMany(c => c.Courses)
                                                           .ToListAsync();
                
                return (courses,courses.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        #endregion
        public async Task<List<Guid>> GetSectionGuid(Guid courseid)
        {
            try
            {
                List<Guid> sectionGuid = await _entities.Where(s => s.Id == courseid && s.DeleteDate == null)
                                                       .Include(shl => shl.CourseHasSections)
                                                       .SelectMany(i => i.CourseHasSections.Where(d => d.DeleteDate == null))
                                                       .Select(s => s.SectionId).ToListAsync();
                return sectionGuid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<CourseHasSection>> GetIncludeCourseHasSection(Guid courseid)
        {

            try
            {
                List<CourseHasSection> courseHasSections = await _entities.Where(s => s.Id == courseid)
                                                                         .Include(chs => chs.CourseHasSections)
                                                                         .ThenInclude(s => s.Section).AsNoTracking()
                                                                         .SelectMany(chs => chs.CourseHasSections.Where(d => d.DeleteDate == null))
                                                                         .Select(x => new CourseHasSection()
                                                                         {
                                                                             Id = x.Id,
                                                                             Section = new Section()
                                                                             {

                                                                                 Name = x.Section.Name,
                                                                                 Description = x.Section.Description,
                                                                                 Content = "x"
                                                                             },
                                                                             SectionId = x.SectionId,
                                                                             Position = x.Position,
                                                                             CourseId = courseid
                                                                         }).ToListAsync();

                return courseHasSections;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Course> GetCourseTolesson(Guid courseId)
        {
            try
            {
                Course course = (await _entities.Where(c => c.Id == courseId)
                                               .Include(s => s.CourseHasSections)
                                               .ThenInclude(s => s.Section)
                                               .ThenInclude(l => l.SectionHasLessons)
                                               .ThenInclude(l => l.Lesson)
                                               .AsNoTracking().ToListAsync()).Select(x => new Course()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sections = x.CourseHasSections.Where(d => d.DeleteDate == null && d.Section.DeleteDate == null).OrderBy(p => p.Position).Select(s => s.Section).Distinct().Select(s => new Section()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Lessons = s.SectionHasLessons.Where(d => d.DeleteDate == null && d.Lesson.DeleteDate == null).OrderBy(p => p.Position).Select(l => l.Lesson).Distinct().Select(l => new Lesson()
                        {
                            Id = l.Id,
                            Name = l.Name

                        }).ToList()

                    }).ToList()
                }).FirstOrDefault();
                return course;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<Guid>> GetAllGuidByCourseId(Guid courseId)
        {
            try
            {
                List<Guid> guids = new List<Guid>();
                guids.Add(courseId);
                Course course = await _entities.Where(c => c.Id == courseId)
                                               .Include(s => s.CourseHasSections)
                                               .ThenInclude(s => s.Section)
                                               .ThenInclude(l => l.SectionHasLessons)
                                               .ThenInclude(l => l.Lesson)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync();
               
                var Allguids = course.CourseHasSections.Where(x => x.DeleteDate == null).DistinctBy(x => x.SectionId).OrderBy(p => p.Position).Select(x => new
                {
                    SectionId = x.SectionId,
                    LessonId = x.Section.SectionHasLessons.Where(x => x.DeleteDate == null).OrderBy(p => p.Position).DistinctBy(z => z.LessonId).Select(l => l.LessonId).ToList()

                }).ToList();

                foreach (var item in Allguids)
                {
                    guids.Add(item.SectionId);
                    guids.AddRange(item.LessonId);
                }
                return guids;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ViewContentVM> GetViewContentById(Guid id)
        {
            try
            {
                ViewContentVM viewContentVM = new();
                bool IsCourse = await _entities.AnyAsync(c => c.Id == id);
                bool IsSection = await _entitiesSection.AnyAsync(s => s.Id == id);
                bool IsLesson = await _entitiesLesson.AnyAsync(l => l.Id == id);
                if (IsCourse)
                {
                    viewContentVM = await _entities.Where(e => e.Id == id).Select(c => new ViewContentVM()
                    {
                        Content = c.Content,
                        Description = c.Description,
                        Id = c.Id,
                        ImageUrl = c.ImageUrl,
                        Name = c.Name,
                        TypeOfContent = TypeOfContent.Course

                    }).FirstOrDefaultAsync();
                }
                if (IsSection)
                {
                    viewContentVM = await _entitiesSection.Where(e => e.Id == id).Select(s => new ViewContentVM()
                    {
                        Content = s.Content,
                        Description = s.Description,
                        Id = s.Id,                     
                        Name = s.Name,
                        TypeOfContent = TypeOfContent.Section

                    }).FirstOrDefaultAsync();
                }
                if (IsLesson)
                {
                    viewContentVM = await _entitiesLesson.Where(e => e.Id == id).Select(l => new ViewContentVM()
                    {
                        Content = l.Content,
                        Description = l.Description,
                        Id = l.Id,                       
                        Name = l.Name,
                        TypeOfContent = TypeOfContent.Lesson

                    }).FirstOrDefaultAsync();
                }
               
                return viewContentVM;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        public async Task<List<Guid>> AllGuidSeen(Guid courseId, Guid userId)
        {
            try
            {
                List<Guid> guids = new List<Guid>();                
                Course course = await _entities.Where(c => c.Id == courseId)
                                               .Include(s => s.CourseHasSections)
                                               .ThenInclude(s => s.Section)
                                               .ThenInclude(l => l.SectionHasLessons)
                                               .ThenInclude(l => l.Lesson)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync();

                var allGuids = course.CourseHasSections.Where(x => x.DeleteDate == null).DistinctBy(x => x.SectionId).OrderBy(p => p.Position).Select(x => new
                {
                    SectionId = x.SectionId,
                    LessonId = x.Section.SectionHasLessons.Where(x => x.DeleteDate == null).OrderBy(p => p.Position).DistinctBy(z => z.LessonId).Select(l => l.LessonId).ToList()

                }).ToList();

                var allHistoricGuids = await _entitiesUser.Where(u => u.Id == userId)
                                               .Include(hl => hl.HistoricLessonHasUsers)
                                               .Include(hs => hs.HistoricSectionHasUsers)
                                               .Include(hc => hc.HistoricCourseHasUsers)
                                               .Select(o => new
                                               {
                                                  HistoriSectionGuid = o.HistoricSectionHasUsers.Where(d => d.EndDate != null)
                                                                                                .Select(s => s.SectionId).ToList(),
                                                  HistoricCourseGuid = o.HistoricCourseHasUsers.Where(d => d.EndDate != null && d.CourseId == courseId)
                                                                                               .Select(c => c.CourseId).Distinct().FirstOrDefault(),
                                                  HistoricLessonGuid = o.HistoricLessonHasUsers.Where(d => d.EndDate != null)
                                                                                               .Select(l => l.LessonId)
                                                                                               .Distinct()
                                                                                               .ToList()
                                               }).ToListAsync();


               guids.AddRange(allHistoricGuids.SelectMany(hs => hs.HistoriSectionGuid).Where(x => allGuids.Select(s => s.SectionId).Contains(x)).ToList());
               guids.AddRange(allHistoricGuids.SelectMany(hs => hs.HistoricLessonGuid).Where(x => allGuids.SelectMany(s => s.LessonId).Contains(x)).ToList());
               guids.Add(allHistoricGuids.Select(hs => hs.HistoricCourseGuid).FirstOrDefault());
               return guids;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #region Get Course By PathId And UserId
        public async Task<List<Course>> GetCourseByPathAndUserId(Guid pathId, Guid userId)
        {
            try
            {
                List<Course> courses = (await _entitiesPathWayHasCourse.Where(p => p.PathWayId == pathId)
                                                                   .Include(pc => pc.Course).ToListAsync())
                                                                   .SelectMany(c => new List<Course>()
                                                                   {
                                                                        new Course()
                                                                        {
                                                                            Id = c.CourseId,
                                                                            Name = c.Course.Name,
                                                                        }
                                                                   }).ToList();
                List<Guid> courseHistoricId = await _entitiesHistoricCourse.Where(u => u.UserId == userId).Select(l => l.CourseId).ToListAsync();
                List<Course> courses1 = courses.Where(x => courseHistoricId.Contains(x.Id)).ToList();
                return courses1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Course By PathId
        public async Task<List<Course>> GetCourseByPathAndUserIdWithHistoricCourse(Guid pathId, Guid userId)
        {
            try
            {
                List<Course> courses = (await _entitiesPathWayHasCourse.Where(p => p.PathWayId == pathId && p.DeleteDate == null && p.Course.DeleteDate == null)
                                                                       .Include(pc => pc.Course)
                                                                       .ThenInclude(hc => hc.HistoricCourseHasUsers)
                                                                       .Select(c => c.Course)
                                                                       .ToListAsync()).ToList(); ;
                                                                   
                return courses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Course Percentage
        public async Task<int> GetCoursePercentage(Guid courseId, Guid userId)
        {
            try
            {
                var LessonsInCourseGuid = await _entitiesCourseHasSection.Where(x => x.CourseId == courseId)
                                                                         .Include(s => s.Section)
                                                                         .ThenInclude(l => l.SectionHasLessons)
                                                                         .ThenInclude(l => l.Lesson)
                                                                         .AsNoTracking()
                                                                         .SelectMany(s => s.Section.SectionHasLessons)
                                                                         .Select(l => l.LessonId).ToListAsync();

                int TotalLessonSeen = await _entitiesHistoricLesson.Where(x => x.DeleteDate == null && 
                                                                               x.UserId == userId && 
                                                                               x.EndDate != null &&
                                                                               LessonsInCourseGuid.Contains(x.LessonId)).CountAsync();

                double percentage = (100 / LessonsInCourseGuid.Count()) * TotalLessonSeen;
                int percent = (int)Math.Ceiling(percentage);

                return percent;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion


    }
}
