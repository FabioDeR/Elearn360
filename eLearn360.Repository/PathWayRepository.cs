using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class PathWayRepository : Repository<Guid, PathWay>, IPathWayRepository
    {
        private readonly DbSet<PathWay> _entities;
        private readonly DbSet<Group> _entitiesGroup;
        private readonly DbSet<Organization> _entitiesOrganization;
        private readonly DbSet<HistoricLessonHasUser> _entitiesHistoricLesson;
        private readonly DbSet<HistoricPathWayHasUser> _entitiesHistoricPath;
        private readonly DbSet<PathWayHasGroup> _entitiesPathHasGroup;
        private readonly DbSet<StaffHasOccupationHasGroup> _entitiesStaffHasOccupationHasGroup;
        private readonly DbSet<UserHasGroup> _entitiesUserHasGroup;
        private readonly DbSet<UserHasOccupation> _entitesUserHasOccupation;
        private readonly DbSet<PathWayHasCourse> _entitiesPathHasCourse;

        private readonly DbContext _context;

        public PathWayRepository(DbContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<PathWay>();
            _entitiesGroup = _context.Set<Group>();
            _entitiesOrganization = _context.Set<Organization>();
            _entitiesHistoricLesson = _context.Set<HistoricLessonHasUser>();
            _entitiesHistoricPath = _context.Set<HistoricPathWayHasUser>();
            _entitiesPathHasGroup = _context.Set<PathWayHasGroup>();
            _entitiesStaffHasOccupationHasGroup = _context.Set<StaffHasOccupationHasGroup>();
            _entitiesUserHasGroup = _context.Set<UserHasGroup>();
            _entitiesPathHasCourse = _context.Set<PathWayHasCourse>();
            _entitesUserHasOccupation = _context.Set<UserHasOccupation>();
        }

        public async override Task<IEnumerable<PathWay>> Get()
        {
            IEnumerable<PathWay> paths = await _entities.Where(x => x.DeleteDate == null).AsNoTracking().ToListAsync();
            return paths;
        }
        public async override Task<PathWay> GetById(Guid id)
        {
            try
            {
                PathWay pathWay = await _entities.Where(s => s.Id == id).AsNoTracking().SingleAsync();
                return pathWay;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<PathWay>> GetPrivatePathWayByUserId(Guid userid)
        {
            try
            {
                List<PathWay> privatePathWay = await _entities.Where(l => l.CreationTrackingUserId == userid && l.DeleteDate == null).ToListAsync();
                return privatePathWay;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<PathWay>> GetPublicPathWayByUserId(Guid userid)
        {
            try
            {
                List<PathWay> publicPathWay = await _entities.Where(l => l.CreationTrackingUserId != userid && l.PrivatePath == false && l.DeleteDate == null).ToListAsync();
                return publicPathWay;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task RemovePathWayHasCourse(PathWayHasCourse pathWayHasCourse)
        {
            try
            {
                PathWay pathWayUpdated = await _entities.Where(s => s.Id == pathWayHasCourse.PathWayId).Include(shl => shl.PathWayHasCourses).SingleAsync();
                pathWayUpdated.PathWayHasCourses.RemoveAll(x => x.PathWayId == pathWayHasCourse.PathWayId && x.CourseId == pathWayHasCourse.CourseId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task RemovePathWayHasGroup(PathWayHasGroup pathWayHasGroup)
        {
            try
            {
                PathWay pathWayUpdated = await _entities.Where(s => s.Id == pathWayHasGroup.PathWayId).Include(shl => shl.PathWayHasGroups).SingleAsync();
                pathWayUpdated.PathWayHasGroups.RemoveAll(x => x.PathWayId == pathWayHasGroup.PathWayId && x.GroupId == pathWayHasGroup.GroupId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task UpdateOrDeleted(List<PathWayHasCourse> pathWayHasCourses)
        {
            try
            {
                PathWay updatePathWay = await _entities.Where(s => s.Id == pathWayHasCourses.Select(i => i.PathWayId).First()).Include(shl => shl.PathWayHasCourses).SingleAsync();
                updatePathWay.PathWayHasCourses.Clear();
                var result = pathWayHasCourses.Select(e => new PathWayHasCourse
                {


                    Position = e.Position,
                    PathWayId = e.PathWayId,
                    CourseId = e.CourseId



                }).ToList();
                updatePathWay.PathWayHasCourses.AddRange(result);
                await base.Put(updatePathWay.Id, updatePathWay);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<PathWay>> GetPathWaysByGroupId(Guid GroupId)
        {
            try
            {
                List<PathWay> pathWays = await _entitiesGroup.Where(g => g.Id == GroupId).Include(p => p.PathWays).SelectMany(p => p.PathWays.Where(d => d.DeleteDate == null)).ToListAsync();
                return pathWays;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<PathWay>> GetPathWaysByOrgaId(Guid OrgaId, Guid Userid)
        {
            try
            {
                List<PathWay> pathWays = await _entitiesGroup.Where(g => g.OrganizationId == OrgaId)
                                                             .Include(p => p.PathWayHasGroups)
                                                             .Include(p => p.PathWays).Include(u => u.Users)
                                                             .SelectMany(p => p.PathWayHasGroups.Where(d => d.DeleteDate == null && d.PathWay.DeleteDate == null))
                                                             .Select(p => new PathWay()
                                                             {
                                                                 Name = p.PathWay.Name,
                                                                 Description = p.PathWay.Description,
                                                                 Id = p.PathWay.Id
                                                             }).Distinct().ToListAsync();



                return pathWays;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<Group>> GetGroupByPathWayId(Guid pathWayId, Guid orgnizationId)
        {
            try
            {
                List<Group> groups = await _entities.Where(g => g.Id == pathWayId).Include(g => g.Groups)
                                                    .SelectMany(g => g.Groups.Where(o => o.OrganizationId == orgnizationId))
                                                    .ToListAsync();
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<Guid>> GetCourseGuid(Guid pathid)
        {
            try
            {
                List<Guid> courseGuid = await _entities.Where(s => s.Id == pathid && s.DeleteDate == null)
                                                       .Include(shl => shl.PathWayHasCourses)
                                                       .SelectMany(i => i.PathWayHasCourses.Where(d => d.DeleteDate == null))
                                                       .Select(s => s.CourseId).ToListAsync();
                return courseGuid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Guid>> GetGroupGuid(Guid pathid)
        {
            try
            {
                List<Guid> groupGuid = await _entities.Where(s => s.Id == pathid && s.DeleteDate == null)
                                                       .Include(shl => shl.PathWayHasGroups)
                                                       .SelectMany(i => i.PathWayHasGroups.Where(d => d.DeleteDate == null))
                                                       .Select(s => s.GroupId).ToListAsync();
                return groupGuid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<PathWayHasCourse>> GetIncludePathWayHasCourse(Guid pathid)
        {

            try
            {
                List<PathWayHasCourse> pathWayHasCourses = await _entities.Where(s => s.Id == pathid)
                                                                         .Include(chs => chs.PathWayHasCourses)
                                                                         .ThenInclude(s => s.Course).AsNoTracking()
                                                                         .SelectMany(chs => chs.PathWayHasCourses.Where(d => d.DeleteDate == null))
                                                                         .Select(x => new PathWayHasCourse()
                                                                         {
                                                                             Id = x.Id,
                                                                             Course = new Course()
                                                                             {
                                                                                 Id = x.Course.Id,
                                                                                 Name = x.Course.Name,
                                                                                 Description = x.Course.Description,
                                                                                 Content = "x"
                                                                             },
                                                                             PathWayId = x.PathWayId,
                                                                             Position = x.Position,
                                                                             CourseId = x.CourseId
                                                                         }).ToListAsync();

                return pathWayHasCourses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region GetPathWayByPathId
        public async Task<List<PathWay>> GetPathByPathid(Guid pathId, Guid userId)
        {
            try
            {
                List<PathWay> paths = await _entities.Where(x => x.DeleteDate == null && x.Id == pathId).ToListAsync();
                List<Guid> pathHistoricId = await _entitiesHistoricPath.Where(u => u.UserId == userId).Select(l => l.PathWayId).ToListAsync();

                List<PathWay> path1 = paths.Where(x => pathHistoricId.Contains(x.Id)).ToList();

                return path1;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Path By UserId No Student
        public async Task<List<PathWay>> GetPathByUserAndOrgaIdNoStudent(Guid userId, Guid organizationId)
        {
            try
            {

                List<UserHasGroup> userHasGroups = await _entitiesUserHasGroup.Where(x => x.DeleteDate == null && x.UserId == userId).ToListAsync();

                List<Guid> GroupStaffs = await _entitiesStaffHasOccupationHasGroup.Where(x => x.DeleteDate == null && x.StaffOccupation.OrganizationId == organizationId && userHasGroups.Any(y => y == x.UserHasGroup))
                                                                                                   .Include(x => x.StaffOccupation)
                                                                                                   .Include(x => x.UserHasGroup)
                                                                                                   .ThenInclude(x => x.Group)
                                                                                                   .Select(x => x.UserHasGroup.GroupId)
                                                                                                   .ToListAsync();

                List<PathWay> Paths = await _entitiesPathHasGroup.Where(x => x.DeleteDate == null && GroupStaffs.Any(g => g == x.GroupId)).Select(x => x.PathWay).ToListAsync();

                return Paths;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Get Path Percentage
        public async Task<int> GetPathPercentage(Guid pathId, Guid userId)
        {
            try
            {
                var LessonsInPathGuid = await _entitiesPathHasCourse.Where(x => x.PathWayId == pathId)
                                                                         .Include(c => c.Course)
                                                                         .ThenInclude(chs => chs.CourseHasSections)
                                                                         .ThenInclude(s => s.Section)
                                                                         .ThenInclude(shl => shl.SectionHasLessons)
                                                                         .ThenInclude(l => l.Lesson)
                                                                         .AsNoTracking()
                                                                         .SelectMany(c => c.Course.CourseHasSections)
                                                                            .SelectMany(s => s.Section.SectionHasLessons)
                                                                                .Select(l => l.LessonId).ToListAsync();

                int TotalLessonSeen = await _entitiesHistoricLesson.Where(x => x.DeleteDate == null &&
                                                                               x.UserId == userId &&
                                                                               x.EndDate != null &&
                                                                               LessonsInPathGuid.Contains(x.LessonId)).CountAsync();

                double percentage = (100 / LessonsInPathGuid.Count()) * TotalLessonSeen;
                int percent = (int)Math.Ceiling(percentage);

                return percent;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion


    }
}
