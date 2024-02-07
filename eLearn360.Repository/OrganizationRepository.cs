using eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM;
using eLearn360.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class OrganizationRepository : Repository<Guid, Organization>, IOrganizationRepository
    {
        private readonly DbSet<Organization> _entities;
        private UserManager<User> _userManager;  
        private readonly Elearn360DBContext _context;
        private readonly DbSet<Organization> _entitiesOrganizations;
        private readonly DbSet<Occupation> _entitiesOccupation;
      

        public OrganizationRepository(Elearn360DBContext context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _entities = _context.Set<Organization>();
            _userManager = userManager;
            _entitiesOrganizations = _context.Set<Organization>();
            _entitiesOccupation = _context.Set<Occupation>();           
        }

        #region Add New User By OrganizationId
        public async Task<(bool,string)> AddNewUserByOrganizationId(UserHasOccupationVM userHasOccupationvm)
        {
            var myTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(userHasOccupationvm.LoginMail);
                if (existingUser == null)
                {
                    var newUser = new User()
                    {
                        LoginMail = userHasOccupationvm.LoginMail,
                        Address = "à completer",
                        FirstName = userHasOccupationvm.FirstName,
                        LastName = userHasOccupationvm.LastName,
                        Country = "à completer",
                        ZipCode = "à completer",
                        City = "à completer",
                        Phone = userHasOccupationvm.Phone,
                        GenderId = userHasOccupationvm.GenderId,
                        ImageUrl = "image par défaut"           
                        
                    };
                    string passwordString = PasswordGestion.PasswordGeneration(11);
                    var isCreated = await _userManager.CreateAsync(newUser, passwordString);
                    if (isCreated.Succeeded)
                    {
                        await PostUserHasOccupation(userHasOccupationvm, newUser);
                        myTransaction.Commit();
                    }
                    return (true,passwordString);
                }
                else
                {
                    await PostUserHasOccupation(userHasOccupationvm, existingUser);
                    myTransaction.Commit();
                    return (false,null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await myTransaction.RollbackAsync();
                throw;
            }
        }

        #region Post UserHasOccupation
        private async Task PostUserHasOccupation(UserHasOccupationVM userHasOccupationvm, User existingUser)
        {
            List<Occupation> occupations = await _entitiesOccupation.Where(o => userHasOccupationvm.OccupationId.Any(x => x == o.Id)).ToListAsync();
            Organization organization = await _entitiesOrganizations.Where(e => e.Id == userHasOccupationvm.OrganizationId).Include(u => u.UserHasOccupations).FirstOrDefaultAsync();
            List<UserHasOccupation> newUserHasOccupation = occupations.Select(x => new UserHasOccupation()
            {
                OccupationId = x.Id,
                OrganizationId = organization.Id,
                UserId = existingUser.Id

            })
            .ToList();
            organization.UserHasOccupations.AddRange(newUserHasOccupation);
            _entitiesOrganizations.Update(organization);
            _context.SaveChanges();
        }
        #endregion
        #endregion
        #region Get All User By       
        public async Task<Organization> GetAllUserByOrganizationId(Guid organizationId)
        {
            try
            {

                Organization organization = await _entities.Where(o => o.Id == organizationId)
                    .Include(u => u.UserHasOccupations)
                    .ThenInclude(o => o.User)                                    
                    .Select(o => new Organization()
                    {
                        Id = organizationId,
                        Name = o.Name,
                        Address = o.Address,
                        City = o.City,
                        Country = o.Country,
                        Email = o.Email,
                        ImageUrl = o.ImageUrl,
                        Phone = o.Phone,
                        Website = o.Website,
                        ZipCode = o.ZipCode,
                        Users = o.Users.Distinct().Select(u => new User()
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            City = u.City,
                            Phone = u.Phone,
                            LoginMail = u.LoginMail,    
                            Birthday = u.Birthday,
                            Occupations = o.Occupations.Distinct().Where(X => X.UserHasOccupations.Any(z => z.UserId == u.Id && z.OrganizationId == organizationId)).ToList()
                        }).ToList(),
                    }).AsNoTracking().FirstOrDefaultAsync();          
                return organization;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        public async override Task<Organization> GetById(Guid id)
        {
            try
            {
                Organization organization = await _entities.Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync();
                return organization;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #region Update UserHasOccupation
        public async Task UpdateUserOccupation(UserHasOccupation userHasOccupation)
        {
            try
            {

                var userHasOccupationUpdated = await _entities.Where(u => u.Id == userHasOccupation.OrganizationId).Include(u => u.UserHasOccupations).SingleAsync();
                userHasOccupationUpdated.UserHasOccupations.Remove(userHasOccupationUpdated.UserHasOccupations.Where(i => i.Id == userHasOccupation.Id).Single());
                userHasOccupationUpdated.UserHasOccupations.Add(new UserHasOccupation()
                {
                    OccupationId = userHasOccupation.OccupationId,
                    OrganizationId = userHasOccupation.OrganizationId,
                    UserId = userHasOccupation.UserId
                });


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion
        #region Remove User
        public async Task RemoveUser(Guid userid, Guid organizationid)
        {
            try
            {
                var userHasOccupationRemoved = await _entities.Where(u => u.Id == organizationid).Include(u => u.UserHasOccupations).SingleAsync();
                userHasOccupationRemoved.UserHasOccupations.RemoveAll(e => e.UserId == userid);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
        #region Remove UserHasOccupation
        public async Task RemoveUserHasOccupation(UserHasOccupation userHasOccupation)
        {
            try
            {              
                var userHasOccupationUpdated = await _entities.Where(u => u.Id == userHasOccupation.OrganizationId).Include(u => u.UserHasOccupations).SingleAsync();
                userHasOccupationUpdated.UserHasOccupations.Remove(userHasOccupationUpdated.UserHasOccupations.Where(i => i.Id == userHasOccupation.Id).Single());
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion


    }
}
