using eLearn360.Data.VM;
using eLearn360.Data.VM.AccountVM;
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
    public class UserRepository : Repository<Guid, User>, IUserRepository
    {
        private readonly DbSet<User> _entities;       
        private readonly DbSet<Organization> _entitiesOrganizations;
        private readonly DbSet<Occupation> _entitiesOccupation;
        private readonly DbContext _context;
        private UserManager<User> _userManager;

        private DbSet<Group> _entitiesGroup;
        public UserRepository(DbContext context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _entities = _context.Set<User>();
            _userManager = userManager;
            _entitiesGroup = _context.Set<Group>();
            _entitiesOrganizations = _context.Set<Organization>();
            _entitiesOccupation = _context.Set<Occupation>();
        }

        #region Create Claims
        public async Task<List<Claim>> CreateClaimsByUser(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));
                var groupOrgAndOccup = (await _entities.Where(u => u.Id == user.Id)
                                                       .Include(o => o.Organizations)
                                                       .ThenInclude(uho => uho.UserHasOccupations.Where(u => u.UserId == user.Id))
                                                       .ThenInclude(o => o.Occupation).AsNoTracking().FirstOrDefaultAsync())
                                                       .Organizations.DistinctBy(o => o.Id).Select(x => new
                                                       {
                                                           OrganizationId = x.Id,
                                                           OrganizationName = x.Name,
                                                           OrganizationImg = x.ImageUrl,
                                                           OccupationList = x.UserHasOccupations.Select(o => o.Occupation.Name).ToList()

                                                       }).FirstOrDefault();


                if (groupOrgAndOccup != null)
                {
                    foreach (var item in groupOrgAndOccup.OccupationList)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
                    }

                    claims.Add(new Claim("OrganizationId", groupOrgAndOccup.OrganizationId.ToString()));
                    claims.Add(new Claim("OrganizationName", groupOrgAndOccup.OrganizationName));
                    claims.Add(new Claim("OrganizationImg", groupOrgAndOccup.OrganizationImg));
                }

                return claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion


        #region Register   
        public async Task<RegisterResultVM> Register(RegisterVM registervm)
        {
            var myTransaction = await _context.Database.BeginTransactionAsync();
            try
            {                
                Group updateGroup = await _entitiesGroup.Where(gn => gn.Name == "Public").Include(o => o.UserHasGroups).FirstOrDefaultAsync();
                Occupation occupation = await _entitiesOccupation.Where(o => o.OccupationName == "Student").FirstOrDefaultAsync();
                Organization organization = await _entitiesOrganizations.Where(e => e.Name == "ASBL Educassist").Include(u => u.UserHasOccupations).FirstOrDefaultAsync();
                var existingUser = await _userManager.FindByEmailAsync(registervm.LoginMail);                
                if (existingUser != null)
                {
                    return new RegisterResultVM()
                    {
                        Errors = new List<string>() {
                                "Email already in use"
                        },
                    };
                }
                var newUser = new User()
                {
                    LoginMail = registervm.LoginMail,
                    Address = registervm.Address,
                    FirstName = registervm.FirstName,
                    LastName = registervm.LastName,
                    Birthday = registervm.Birthday,
                    Country = registervm.Country,
                    ZipCode = registervm.ZipCode,
                    City = registervm.City,
                    Phone = registervm.Phone,
                    GenderId = registervm.GenderId                    
                    
                };
                var isCreated = await _userManager.CreateAsync(newUser, registervm.Password);
                if (isCreated.Succeeded)
                {
                    /*Associate User ==> Student Occupation*/
                    organization.UserHasOccupations.Add(new UserHasOccupation() { OrganizationId = organization.Id, CreationTrackingUserId = newUser.Id, Occupation = occupation,UserId = newUser.Id});
                    _entitiesOrganizations.Update(organization);
                    updateGroup.UserHasGroups.Add(new UserHasGroup() { CreationTrackingUserId = newUser.Id, StartDate = DateTime.Now, EndDate = DateTime.Parse("31-12-9999"), User = newUser });
                    _entitiesGroup.Update(updateGroup);
                    await _context.SaveChangesAsync();
                    await myTransaction.CommitAsync();
                    return new RegisterResultVM()
                    {
                        Success = true
                    };
                }
                else
                {
                    return new RegisterResultVM()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    };
                }

            }
            catch (Exception ex)
            {
                await myTransaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        #endregion

        public async override Task<User> GetById(Guid id)
        {
            try
            {
                User user = await _entities.Where(u => u.Id == id).Include(o => o.Organizations).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Organization>> GetAllOrganizationByUserId(Guid userid)
        {
            try
            {
                List<Organization> organizations = await _entities.Where(u => u.Id == userid).Include(o => o.Organizations).SelectMany(o => o.Organizations).Distinct().ToListAsync();
                return organizations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Claim>> RefreshClaims(RefreshTokenVM refreshTokenVM)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(refreshTokenVM.UserId.ToString());            
               
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, existingUser.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()));              
                claims.Add(new Claim(ClaimTypes.Name, existingUser.FirstName + " " + existingUser.LastName));            
                var groupOrgAndOccup = (await _entities.Where(u => u.Id == refreshTokenVM.UserId)
                                                      .Include(o => o.Organizations.Where(u => u.Id == refreshTokenVM.OrganizationId))
                                                      .ThenInclude(uho => uho.UserHasOccupations.Where(u => u.UserId == refreshTokenVM.UserId))
                                                      .ThenInclude(o => o.Occupation).AsNoTracking().FirstOrDefaultAsync())
                                                      .Organizations.DistinctBy(o => o.Id).Select(x => new
                                                      {
                                                          OrganizationId = x.Id,
                                                          OrganizationName = x.Name,
                                                          OrganizationImg = x.ImageUrl,
                                                          OccupationList = x.UserHasOccupations.Select(o => o.Occupation.Name).ToList()

                                                      }).FirstOrDefault();
                if (groupOrgAndOccup != null)
                {
                    foreach (var item in groupOrgAndOccup.OccupationList)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
                    }

                    claims.Add(new Claim("OrganizationId", groupOrgAndOccup.OrganizationId.ToString()));
                    claims.Add(new Claim("OrganizationName", groupOrgAndOccup.OrganizationName));
                    claims.Add(new Claim("OrganizationImg", groupOrgAndOccup.OrganizationImg));
                }


                return claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Get User Profile
        public async Task<AccountRegisterEditVM> GetUserProfile(Guid id)
        {
            try
            {
                User user = await _entities.FindAsync(id);
                AccountRegisterEditVM accountRegisterEditVM = new()
                {
                    Address = user.Address,
                    Birthday = user.Birthday,
                    City = user.City,
                    Country = user.City,
                    FirstName = user.FirstName,
                    GenderId = user.GenderId,
                    Id = user.Id,
                    ImageUrl = user.ImageUrl,
                    LastName = user.LastName,
                    LoginMail = user.LoginMail,
                    Phone = user.Phone,
                    ZipCode = user.ZipCode
                };
                return accountRegisterEditVM;

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Profile Update
        public async Task<AccountRegisterEditVM> UserProfileUpdate(AccountRegisterEditVM accountRegisterEditVM)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(accountRegisterEditVM.Id.ToString());

                user.FirstName = accountRegisterEditVM.FirstName;
                user.LastName = accountRegisterEditVM.LastName;
                user.Country = accountRegisterEditVM.Country;
                user.Address = accountRegisterEditVM.Address;
                user.ZipCode = accountRegisterEditVM.ZipCode;
                user.Birthday = accountRegisterEditVM.Birthday;
                user.City = accountRegisterEditVM.City;
                //user.LoginMail = user.LoginMail;
                user.Phone = accountRegisterEditVM.Phone;
                user.GenderId = accountRegisterEditVM.GenderId;
                user.ImageUrl = accountRegisterEditVM.ImageUrl;

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();

                return accountRegisterEditVM;
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
