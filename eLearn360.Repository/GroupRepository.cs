using eLearn360.Data.VM.GroupVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class GroupRepository : Repository<Guid, Group>, IGroupRepository
    {
        private readonly DbSet<Group> _entities;
        private readonly DbSet<Organization> _entitiesOrganization;
        private readonly DbSet<User> _entitiesUser;
        private readonly DbSet<PathWayHasGroup> _entitiesPathHasGroup;
        private readonly DbSet<StaffHasOccupationHasGroup> _entitiesStaffHasOccupationHasGroup;
        private readonly DbSet<UserHasGroup> _entitiesUserHasGroup;

        private readonly Elearn360DBContext _context;

        public GroupRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Group>();
            _entitiesUser = _context.Set<User>();
            _entitiesOrganization = _context.Set<Organization>();
            _entitiesPathHasGroup = _context.Set<PathWayHasGroup>();
            _entitiesStaffHasOccupationHasGroup = _context.Set<StaffHasOccupationHasGroup>();
            _entitiesUserHasGroup = _context.Set<UserHasGroup>();
        }


        #region Get All User by GroupId
        public async Task<Group> GetAllUserByGroupId(Guid groupId)
        {
            try
            {
                Group group = (await _entities.Where(g => g.Id == groupId)
                    .Include(u => u.UserHasGroups)
                    .ThenInclude(u => u.User)
                    .Include(s => s.UserHasGroups)
                    .ThenInclude(s => s.StaffOccupations).ToListAsync())
                    .Select(g => new Group()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        UserHasGroups = g.UserHasGroups.Where(d => d.DeleteDate == null).SelectMany(u => new List<UserHasGroup>()
                        {
                            new UserHasGroup()
                            {
                                User = new User()
                            {
                                Id = u.User.Id,
                                FirstName = u.User.FirstName,
                                LastName = u.User.LastName,
                                Birthday = u.User.Birthday,
                                LoginMail = u.User.LoginMail,
                                Phone = u.User.Phone,
                            },
                            StaffOccupations = u.StaffOccupations.Select(st => new StaffOccupation()
                            {
                                Name = st.Name
                            }).ToList(),
                            IsHeadTeacher = u.IsHeadTeacher,                            
                            }

                        }).ToList(),

                        OrganizationId = g.OrganizationId,
                        ImageUrl = g.ImageUrl,
                        CreationTrackingUserId = g.CreationTrackingUserId

                    }).First();
                return group;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Student Group by Orga & UserId
        public async Task<List<Group>> GetStudentGroupsByUserAndOrgaId(Guid organizationId, Guid userId)
        {
            try
            {
                //Select group where userid isn't professor by with orgaId and userId
                List<Group> groups = (await _entitiesUserHasGroup.Where(x => x.DeleteDate == null &&
                                                                             x.Group.PathWayHasGroups.Any(a => a.DeleteDate == null) &&
                                                                             x.UserId == userId &&
                                                                             x.Group.OrganizationId == organizationId &&
                                                                             x.StaffOccupations.Count == 0)
                                                                 .Include(x => x.Group)
                                                                 .ThenInclude(x => x.PathWays)
                                                                 .ThenInclude(x => x.HistoricPathWayHasUsers)
                                                                 .Include(x => x.StaffOccupations).ToListAsync())
                                                                 .SelectMany(y => new List<Group>() { new Group()
                                                                    {
                                                                        Id = y.GroupId,
                                                                        Name = y.Group.Name,
                                                                        OrganizationId = organizationId,
                                                                        ImageUrl = y.Group.ImageUrl,
                                                                        PathWays = y.Group.PathWays.SelectMany(z => new List<PathWay>()
                                                                            {
                                                                                new PathWay()
                                                                                {
                                                                                    Id = z.Id,
                                                                                    CategoryId = z.CategoryId,
                                                                                    Description = z.Description,
                                                                                    ImageUrl = z.ImageUrl,
                                                                                    LevelId = z.LevelId,
                                                                                    Name = z.Name,
                                                                                    HistoricPathWayHasUsers = z.HistoricPathWayHasUsers
                                                                                }
                                                                            }).ToList()
                                                                     }
                                                                 })
                                                                 .Distinct()
                                                                 .ToList();

                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Get All Group by Organizationid
        public async Task<List<Group>> GetGroupsByOrganizationId(Guid OrganizationId)
        {
            try
            {
                List<Group> groups = await _entities.Where(u => u.OrganizationId == OrganizationId)
                                                    .ToListAsync();
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove Users
        public async Task RemoveUserHasGroup(UserHasGroup userHasGroup)
        {
            try
            {
                Group group = await _entities.Where(g => g.Id == userHasGroup.GroupId).Include(uhg => uhg.UserHasGroups).SingleAsync();
                group.UserHasGroups.RemoveAll(x => x.GroupId == userHasGroup.GroupId && x.UserId == userHasGroup.UserId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Promote
        public async Task Promote(PromoteVM promoteVM)
        {
            try
            {
                UserHasGroup oldUserHasGroupUpdated = CreateOldUserHasGroup(promoteVM.OldUserHasGroup);
                Group group = await _entities.Where(u => u.Id == promoteVM.OldUserHasGroup.GroupId).Include(o => o.UserHasGroups).FirstOrDefaultAsync();
                group.UserHasGroups.RemoveAll(x => x.UserId == promoteVM.OldUserHasGroup.UserId && x.GroupId == promoteVM.OldUserHasGroup.GroupId);
                await _context.SaveChangesAsync();
                group.UserHasGroups.Add(oldUserHasGroupUpdated);

                promoteVM.NewUserHasGroup.StartDate = DateTime.Now;
                promoteVM.NewUserHasGroup.EndDate = DateTime.Parse("31-12-9999");
                group.UserHasGroups.Add(promoteVM.NewUserHasGroup);

                await base.Put(group.Id, group);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static UserHasGroup CreateOldUserHasGroup(UserHasGroup userHasGroup)
        {
            return new()
            {
                CreationDate = userHasGroup.CreationDate,
                DeleteDate = DateTime.Now,
                CreationTrackingUserId = userHasGroup.CreationTrackingUserId,
                DeleteTrackingUserId = userHasGroup.DeleteTrackingUserId,
                EndDate = userHasGroup.EndDate,
                GroupId = userHasGroup.GroupId,
                UserId = userHasGroup.UserId,
                StartDate = userHasGroup.StartDate,
                IsHeadTeacher = userHasGroup.IsHeadTeacher,
            };
        }
        #endregion
        #region Graduate
        public async Task Graduate(UserHasGroup userHasGroup)
        {
            var myTransaction = _context.Database.BeginTransaction();
            try
            {
                UserHasGroup oldUserHasGroupUpdated = CreateOldUserHasGroup(userHasGroup);
                Group group = await _entities.Where(u => u.Id == userHasGroup.GroupId).Include(o => o.UserHasGroups).FirstOrDefaultAsync();
                group.UserHasGroups.RemoveAll(x => x.UserId == userHasGroup.UserId && x.GroupId == userHasGroup.GroupId);
                _context.SaveChanges();
                group.UserHasGroups.Add(oldUserHasGroupUpdated);
                await base.Put(group.Id, group);

                User user = await _entitiesUser.Where(u => u.Id == oldUserHasGroupUpdated.UserId).Include(u => u.UserHasOccupations).SingleAsync();
                user.UserHasOccupations.RemoveAll(x => x.OrganizationId == group.OrganizationId);
                _context.SaveChanges();
                myTransaction.Commit();
            }
            catch (Exception ex)
            {
                myTransaction.Rollback();
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        public async Task<Group> GetTeacherList(Guid groupId)
        {
            try
            {
                Group userTeacher = await GetAllUserByGroupId(groupId);
                userTeacher.UserHasGroups.RemoveAll(x => x.StaffOccupations.Count == 0);
                return userTeacher;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<Group> GetStudentList(Guid groupId)
        {
            try
            {
                Group studentList = await GetAllUserByGroupId(groupId);
                studentList.UserHasGroups.RemoveAll(x => x.StaffOccupations.Count > 0);
                return studentList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<User>> GetUserstNotInGroup(Guid groupid, Guid organizationid)
        {
            try
            {
                List<User> userGroup = await _entities.Where(g => g.Id == groupid)
                                                      .Include(u => u.UserHasGroups)
                                                      .Include(u => u.Users)
                                                      .SelectMany(u => u.UserHasGroups.Where(d => d.DeleteDate == null))
                                                      .Select(u => u.User)
                                                      .AsNoTracking()
                                                      .ToListAsync();
                List<User> usersOrg = await _entitiesOrganization.Where(o => o.Id == organizationid)
                                                                 .Include(u => u.Users).SelectMany(u => u.Users).Distinct().ToListAsync();
                List<User> notingroup = (usersOrg.Where(z => !userGroup.Any(x => x.Id == z.Id)).ToList()).Select(u => new User()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Birthday = u.Birthday,
                    LoginMail = u.LoginMail,
                    Phone = u.Phone,

                }).ToList();
                


                return notingroup;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }           
        }

        public async Task<List<Group>> GetMyGroups(Guid userid, Guid organizationid)
        {
            try
            {                
                List<Group> groups = await _entitiesUserHasGroup.Where(x => x.DeleteDate == null &&
                                                                             x.UserId == userid &&
                                                                             x.Group.OrganizationId == organizationid &&
                                                                             x.StaffOccupations.Count != 0)
                                                                 .Include(x => x.Group)
                                                                 .Include(x => x.StaffOccupations)
                                                                 .Select(y => new Group()
                                                                 {
                                                                     Id = y.GroupId,
                                                                     Name = y.Group.Name,
                                                                     OrganizationId = organizationid,
                                                                     ImageUrl = y.Group.ImageUrl,

                                                                 }).Distinct().ToListAsync();
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
