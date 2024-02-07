using eLearn360.Data.VM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class LessonRepository : Repository<Guid, Lesson>, ILessonRepository
    {
        private readonly DbSet<Lesson> _entities;
        private readonly DbSet<Section> _entitiesSection;
        private readonly DbSet<Course> _entitiesCourse;
        private readonly DbSet<PathWay> _entitiesPath;
        private readonly DbSet<HistoricLessonHasUser> _entitiesHistoricLessonHasUser;
        private readonly DbSet<HistoricSectionHasUser> _entitiesHistoricSectionHasUser;
        private readonly DbSet<HistoricCourseHasUser> _entitiesHistoricCourseHasUser;
        private readonly DbSet<HistoricPathWayHasUser> _entitiesHistoricPathHasUser;
        private readonly DbSet<User> _entitesUser;
        private readonly DbSet<SectionHasLesson> _entitiesSectionHasLesson;
        private readonly DbSet<CourseHasSection> _entitiesCourseHasSection;
        private readonly DbSet<PathWayHasCourse> _entitiesPathHasCourse;
        private readonly DbSet<PathWayHasGroup> _entitiesPathWayHasGroup;
        private readonly Elearn360DBContext _context;


        public LessonRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Lesson>();
            _entitiesSection = _context.Set<Section>();
            _entitiesCourse = _context.Set<Course>();
            _entitiesPath = _context.Set<PathWay>();
            _entitiesHistoricLessonHasUser = _context.Set<HistoricLessonHasUser>();
            _entitiesHistoricSectionHasUser = _context.Set<HistoricSectionHasUser>();
            _entitiesHistoricCourseHasUser = _context.Set<HistoricCourseHasUser>();
            _entitiesHistoricPathHasUser = _context.Set<HistoricPathWayHasUser>();
            _entitiesSectionHasLesson = _context.Set<SectionHasLesson>();
            _entitiesCourseHasSection = _context.Set<CourseHasSection>();
            _entitiesPathHasCourse = _context.Set<PathWayHasCourse>();
            _entitesUser = _context.Set<User>();
            _entitiesPathWayHasGroup = _context.Set<PathWayHasGroup>();
        }

        public async override Task<IEnumerable<Lesson>> Get()
        {
            IEnumerable<Lesson> lessons = await _entities.Where(x => x.DeleteDate == null).AsNoTracking().ToListAsync();
            return lessons;
        }

        #region Post Start Historic
        public async Task PostStartHistoric(Guid userId, Guid ItemId)
        {
            try
            {
                //Determine Type && Create
                Guid LessonControl = await _entities.Where(x => x.Id == ItemId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid SectionControl = await _entitiesSection.Where(x => x.Id == ItemId && x.DeleteDate == null && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid CourseControl = await _entitiesCourse.Where(x => x.Id == ItemId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid PathControl = await _entitiesPath.Where(x => x.Id == ItemId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid HistoricLessonControl = await _entitiesHistoricLessonHasUser.Where(x => x.LessonId == ItemId && x.UserId == userId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid HistoricSectionControl = await _entitiesHistoricSectionHasUser.Where(x => x.SectionId == ItemId && x.UserId == userId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid HistoricCourseControl = await _entitiesHistoricCourseHasUser.Where(x => x.CourseId == ItemId && x.UserId == userId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();
                Guid HistoricPathControl = await _entitiesHistoricPathHasUser.Where(x => x.PathWayId == ItemId && x.UserId == userId && x.DeleteDate == null).Select(x => x.Id).FirstOrDefaultAsync();

                if (LessonControl != Guid.Empty && HistoricLessonControl == Guid.Empty)
                {
                    HistoricLessonHasUser hl = new()
                    {
                        LessonId = LessonControl,
                        UserId = userId,
                        StartDate = DateTime.Now
                    };
                    await _entitiesHistoricLessonHasUser.AddAsync(hl);
                    await _context.SaveChangesAsync();
                }

                if (SectionControl != Guid.Empty && HistoricSectionControl == Guid.Empty)
                {
                    HistoricSectionHasUser hs = new()
                    {
                        SectionId = SectionControl,
                        UserId = userId,
                        StartDate = DateTime.Now
                    };
                    await _entitiesHistoricSectionHasUser.AddAsync(hs);
                    await _context.SaveChangesAsync();
                }

                if (CourseControl != Guid.Empty && HistoricCourseControl == Guid.Empty)
                {
                    HistoricCourseHasUser hc = new()
                    {
                        CourseId = CourseControl,
                        UserId = userId,
                        StartDate = DateTime.Now
                    };
                    await _entitiesHistoricCourseHasUser.AddAsync(hc);
                    await _context.SaveChangesAsync();
                }


                if (PathControl != Guid.Empty && HistoricPathControl == Guid.Empty)
                {
                    HistoricPathWayHasUser hp = new()
                    {
                        PathWayId = PathControl,
                        UserId = userId,
                        StartDate = DateTime.Now
                    };
                    await _entitiesHistoricPathHasUser.AddAsync(hp);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Update End Historic
        public async Task UpdateEndHistoric(Guid userId, Guid lessonId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                Guid LessonIdControl = await _entities.Where(x => x.DeleteDate == null && x.Id == lessonId).Select(x => x.Id).FirstOrDefaultAsync();

                //If lesson  => algorythm, else nothing
                if (!LessonIdControl.Equals(Guid.Empty))
                {
                    //Load && Update HistoricLesson
                    HistoricLessonHasUser HistoricLesson = await _entitiesHistoricLessonHasUser.Where(x => x.DeleteDate == null && x.UserId == userId && x.LessonId == lessonId).FirstOrDefaultAsync();
                    HistoricLesson.EndDate = DateTime.Now;

                    _context.Update(HistoricLesson);
                    await _context.SaveChangesAsync();

                    //Get SectionIds From LessionId
                    List<Guid> Sections = _entitiesSectionHasLesson.Where(x => x.DeleteDate == null && x.LessonId == lessonId).Select(x => x.SectionId).ToList();
                    //Section section = await _entitiesSection.Where(x => x.DeleteDate == null && x.Lessons.Any(x => x.Id == lessonId)).Include(x => x.HistoricSectionHasUsers).FirstOrDefaultAsync();

                    foreach (var section in Sections)
                    {
                        //All LessonId in Section && Count lesson view without EndDate by userid
                        List<Guid> totalLessonsInSection = await _entitiesSectionHasLesson.Where(x => x.DeleteDate == null && x.Lesson.DeleteDate == null && x.SectionId == section)
                                                                                          .Include(x => x.Lesson)
                                                                                          .Select(x => x.LessonId)
                                                                                          .Distinct().ToListAsync();

                        List<Guid> totalLessonView = await _entitiesHistoricLessonHasUser.Where(x => x.UserId == userId && totalLessonsInSection.Contains(x.LessonId) && x.EndDate != null)
                                                                                         .Select(x => x.LessonId)
                                                                                         .Distinct().ToListAsync();

                        //Compare 2 lists
                        if (!totalLessonsInSection.Except(totalLessonView).Any() && section != null)
                        {
                            //Load Historic Section & update
                            HistoricSectionHasUser HistoricSection = await _entitiesHistoricSectionHasUser.Where(x => x.DeleteDate == null && x.SectionId == section && x.UserId == userId).FirstOrDefaultAsync();
                            HistoricSection.EndDate = DateTime.Now;
                            _context.Update(HistoricSection);
                            await _context.SaveChangesAsync();

                            //Get CourseIds By SectionId
                            List<Guid> Courses = await _entitiesCourseHasSection.Where(x => x.DeleteDate == null && x.SectionId == section).Select(x => x.CourseId).ToListAsync();
                            foreach (var course in Courses)
                            {
                                List<Guid> totalSectionInCourse = await _entitiesCourseHasSection.Where(x => x.DeleteDate == null && x.Section.DeleteDate == null && x.CourseId == course).Include(x => x.Section)
                                                                                           .Select(x => x.SectionId)
                                                                                           .Distinct().ToListAsync();

                                List<Guid> totalSectionView = await _entitiesHistoricSectionHasUser.Where(x => x.DeleteDate == null && x.UserId == userId && totalSectionInCourse.Contains(x.SectionId) && x.EndDate != null)
                                                                                                   .Select(x => x.SectionId)
                                                                                                   .Distinct().ToListAsync();

                                //Compare 2 lists
                                if (!totalSectionInCourse.Except(totalSectionView).Any() && course != null)
                                {
                                    //Load HistoricCourse && update
                                    HistoricCourseHasUser HistoricCourse = await _entitiesHistoricCourseHasUser.Where(x => x.DeleteDate == null && x.CourseId == course && x.UserId == userId).FirstOrDefaultAsync();
                                    HistoricCourse.EndDate = DateTime.Now;
                                    _context.Update(HistoricCourse);
                                    await _context.SaveChangesAsync();

                                    //Get PathIds by CourseId
                                    List<Guid> paths = await _entitiesPathHasCourse.Where(x => x.DeleteDate == null && x.CourseId == course).Select(x => x.PathWayId).ToListAsync();
                                    foreach (var path in paths)
                                    {

                                        //All course in path && Count Course view without EndDate by userid
                                        List<Guid> totalCourseInPath = await _entitiesPathHasCourse.Where(x => x.DeleteDate == null && x.Course.DeleteDate == null && x.PathWayId == path)
                                                                                                   .Include(x => x.Course)
                                                                                                   .Select(x => x.CourseId)
                                                                                                   .Distinct().ToListAsync();

                                        List<Guid> totalCourseView = await _entitiesHistoricCourseHasUser.Where(x => x.DeleteDate == null && x.UserId == userId && totalCourseInPath.Contains(x.CourseId) && x.EndDate != null)
                                                                                                         .Select(x => x.CourseId)
                                                                                                         .Distinct().ToListAsync();

                                        //Compare 2 lists
                                        if (!totalCourseInPath.Except(totalCourseView).Any() && path != null)
                                        {
                                            //Load HistoricPath && update
                                            HistoricPathWayHasUser HistoricPath = await _entitiesHistoricPathHasUser.Where(x => x.DeleteDate == null && x.PathWayId == path && x.UserId == userId).FirstOrDefaultAsync();
                                            HistoricPath.EndDate = DateTime.Now;

                                            _context.Update(HistoricPath);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                transaction.Commit();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        public async Task DuplicateLesson(Guid lesssonid, Guid userid)
        {
            try
            {
                Lesson newLesson = await _entities.Where(l => l.Id == lesssonid).Select(ol => new Lesson()
                {
                    Name = $"{ol.Name}-copie",
                    Content = ol.Content,
                    CreationDate = ol.CreationDate,
                    CreationTrackingUserId = userid,
                    DeleteDate = ol.DeleteDate,
                    DeleteTrackingUserId = ol.DeleteTrackingUserId,
                    Description = ol.Description,
                    PrivateLesson = ol.PrivateLesson,
                    UpdateDate = ol.UpdateDate,
                    UpdateTrackingUserId = ol.UpdateTrackingUserId

                }).FirstOrDefaultAsync();

                await base.Post(newLesson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<List<Lesson>> GetPrivateLessonByUserId(Guid userid)
        {
            try
            {
                List<Lesson> privateLesssons = await _entities.Where(l => l.CreationTrackingUserId == userid && l.DeleteDate == null).Select(l => new Lesson()
                {
                    Id = l.Id,
                    Description = l.Description,
                    Name = l.Name,
                    CreationDate = l.CreationDate

                }).ToListAsync();
                return privateLesssons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Lesson>> GetPublicLessonByUserId(Guid userid)
        {
            try
            {
                List<Lesson> publicLessons = await _entities.Where(l => l.CreationTrackingUserId != userid && l.PrivateLesson == false && l.DeleteDate == null).Select(l => new Lesson()
                {
                    Id = l.Id,
                    Description = l.Description,
                    Name = l.Name,
                    CreationDate = l.CreationDate,

                }).ToListAsync();
                return publicLessons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Get Lesson Report
        public async Task<LessonReportVM> GetLessonReport(Guid userid, Guid groupid)
        {
            LessonReportVM lessonReportVM = new();
            List<ReportPathVM> reportPaths = new();
            //Guid Listing to historics tables with end dates
            List<Guid> historicPathGuid = await (_entitiesHistoricPathHasUser.Where(h => h.UserId == userid && h.EndDate != null).Select(x => x.PathWayId)).Distinct().ToListAsync();
            List<Guid> historicCoursesGuid = await (_entitiesHistoricCourseHasUser.Where(h => h.UserId == userid && h.EndDate != null).Select(x => x.CourseId)).Distinct().ToListAsync();
            List<Guid> historicSectionsGuid = await (_entitiesHistoricSectionHasUser.Where(h => h.UserId == userid && h.EndDate != null).Select(x => x.SectionId)).Distinct().ToListAsync();
            List<Guid> historicLessonsGuid = await (_entitiesHistoricLessonHasUser.Where(h => h.UserId == userid && h.EndDate != null).Select(x => x.LessonId)).Distinct().ToListAsync();


            var PathToLesson = await _entitiesPathWayHasGroup.Where(x => x.GroupId == groupid &&
                                                                         x.DeleteDate == null)
                                                              .Include(p => p.PathWay)
                                                              .ThenInclude(phc => phc.PathWayHasCourses)
                                                              .ThenInclude(c => c.Course)
                                                              .ThenInclude(chs => chs.CourseHasSections)
                                                              .ThenInclude(s => s.Section)
                                                              .ThenInclude(shl => shl.SectionHasLessons)
                                                              .ThenInclude(l => l.Lesson)
                                                              .AsNoTracking()
                                                              .ToListAsync();

            var paths = PathToLesson.SelectMany(np => new List<ReportPathVM>(){
                new ReportPathVM()
                {
                    PathVM = new PathVM()
                    {
                        Img = np.PathWay.ImageUrl,
                        Name = np.PathWay.Name,
                        PathId = np.PathWayId
                    },
                    PathSeen = historicPathGuid.Contains(np.PathWayId) ? true : false,
                    // 100 / Total Lesson in Path * Total lesson Seen (historicCourseGuid)
                    //PercentComplete = (int)(((double)100) / (double) np.PathWay.PathWayHasCourses.Where(x => x.PathWayId == np.PathWayId && x.DeleteDate == null).Select(y => y.Course.CourseHasSections.Where(z => z.CourseId == y.CourseId && z.DeleteDate == null).Where(b => b.Section.SectionHasLessons.Any(a => a.DeleteDate == null)).Select(a => a.Section.SectionHasLessons.Count()).Sum()).Sum() * np.PathWay.PathWayHasCourses.Where(x => x.PathWayId == np.PathWayId && x.DeleteDate == null).Select(y => y.Course.CourseHasSections.Where(z => z.CourseId == y.CourseId && z.DeleteDate == null).Select(a => a.Section.SectionHasLessons.Where(a => a.DeleteDate == null && historicLessonsGuid.Contains(a.LessonId)).Count()).Sum()).Sum()),
                    ReportCourses = np.PathWay.PathWayHasCourses.Distinct().Where(x => x.DeleteDate == null).SelectMany(nc => new List<ReportCourseVM>()
                    {
                        new ReportCourseVM()
                        {
                            CourseVM = new CourseVM2()
                            {
                                Name = nc.Course.Name,
                                CourseId = nc.Course.Id,
                                Img = nc.Course.ImageUrl
                            },
                            // 100 / Total Lesson in Course * Total lesson Seen (historicCourseGuid)
                            //PercentComplete = (int)(((double)100) / (double)nc.Course.CourseHasSections.Where(x => x.CourseId == nc.CourseId && x.DeleteDate == null).Select(y => y.Section.SectionHasLessons.Where(x => x.DeleteDate == null).Select(x => x.LessonId)).Count()) * nc.Course.CourseHasSections.Where(x => x.CourseId == nc.CourseId && x.DeleteDate == null).Select(x => x.Section.SectionHasLessons.Where(y => historicLessonsGuid.Contains(y.LessonId)).Count()).Sum(),
                            CourseSeen = historicCoursesGuid.Contains(nc.CourseId) ? true : false,
                            ReportSections = nc.Course.CourseHasSections.Distinct().Where(x => x.DeleteDate == null).SelectMany(ns => new List<ReportSectionVM>()
                            {
                                new ReportSectionVM()
                                {
                                    SectionVM = new SectionVM2()
                                    {
                                        SectionId = ns.SectionId,
                                        Name = ns.Section.Name
                                    },
                                    SectionSeen = historicSectionsGuid.Contains(ns.SectionId) ? true : false,

                                    RapportLessons = ns.Section.SectionHasLessons.Distinct().Where(x => x.DeleteDate==null).SelectMany(nl => new List<ReportLessonVM>()
                                    {
                                        new ReportLessonVM()
                                        {
                                            LessonVM = new LessonVM2()
                                            {
                                                LessonId = nl.LessonId,
                                                Name = nl.Lesson.Name
                                            },
                                            LessonSeen = historicLessonsGuid.Contains(nl.LessonId) ? true : false
                                        }
                                    }).ToList()
                                }}).ToList()
                        }}).ToList()
                }}).ToList();

            User user = await _entitesUser.Where(x => x.DeleteDate == null && x.Id == userid).FirstOrDefaultAsync();
            lessonReportVM.Name = $"{user.FirstName} {user.LastName}";
            reportPaths = paths;
            lessonReportVM.ReportPaths = reportPaths;

            return lessonReportVM;
        }
        #endregion

        #region Get Lesson By PathId
        public async Task<List<Lesson>> GetLessonByPathid(Guid pathid, Guid userId)
        {
            try
            {
                List<Lesson> lessons = (await _entitiesPathHasCourse.Where(p => p.PathWayId == pathid)
                                                                    .Include(pc => pc.Course)
                                                                    .ThenInclude(cs => cs.CourseHasSections)
                                                                    .ThenInclude(c => c.Section)
                                                                    .ThenInclude(p => p.SectionHasLessons)
                                                                    .ThenInclude(l => l.Lesson).AsNoTracking()
                                                                    .ToListAsync())
                                                                    .SelectMany(l => l.Course.CourseHasSections)
                                                                    .SelectMany(l => l.Section.SectionHasLessons)
                                                                    .SelectMany(l => new List<Lesson>()
                                                                    {
                                                                        new Lesson()
                                                                        {
                                                                            Id = l.LessonId,
                                                                            Name = l.Lesson.Name
                                                                        }
                                                                    }).ToList();
                List<Guid> historicLessonId = await _entitiesHistoricLessonHasUser.Where(u => u.UserId == userId).Select(l => l.LessonId).ToListAsync();
                List<Lesson> lessons1 = lessons.Where(x => historicLessonId.Contains(x.Id)).ToList();
                return lessons1;

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
