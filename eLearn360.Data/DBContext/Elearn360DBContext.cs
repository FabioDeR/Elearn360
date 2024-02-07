using eLearn360.Data.IdentityConfig;
using eLearn360.Data.Models;
using eLearn360.Data.ModelsConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.DBContext
{
    public class Elearn360DBContext : MyIdentityContext<User, Occupation, IdentityUserRole<Guid>, Guid>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public Elearn360DBContext(DbContextOptions<Elearn360DBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<PathWay> PathWays { get; set; }
        public DbSet<Filial> Filials { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<StaffOccupation> StaffHasOccupations { get; set; }
        public DbSet<UserHasFamilyLink> UserHasFamilyLink { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Quizz> Quizzs { get; set; }








        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {

            Tracking();
          
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

         public void Tracking()
        {
            //UserId claim for tracking
            Guid userId = GetUserId();

            //CreationDate
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property("CreationDate").CurrentValue = DateTime.Now;
                E.Property("CreationTrackingUserId").CurrentValue = userId;
            });

            //UpdateDate
            var EditedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();
            var DeletedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Deleted).ToList();
            EditedEntities.ForEach(E =>
            {
                E.Property("UpdateDate").CurrentValue = DateTime.Now;
                E.Property("UpdateTrackingUserId").CurrentValue = userId;

            });

            //DeleteDate
            DeletedEntities.ForEach(E =>
            {
                E.State = EntityState.Modified;
                E.Property("DeleteDate").CurrentValue = DateTime.Now;
                E.Property("DeleteTrackingUserId").CurrentValue = userId;
            });
        }

        public Guid GetUserId()
        {
            try
            {
                return new Guid(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Guid.Empty;
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Builder config          
            builder.ApplyConfiguration(new SectionsConfiguration());         
            builder.ApplyConfiguration(new UserHasOccupationConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new PathWayConfiguration());
            builder.ApplyConfiguration(new OrganizationConfiguration());      
            builder.ApplyConfiguration(new GroupConfiguration());
            builder.ApplyConfiguration(new StaffOccupationConfiguration());
            builder.ApplyConfiguration(new HistoricLessonHasUsersConfiguration());
            builder.ApplyConfiguration(new HistoricSectionHasUserConfiguration());
            builder.ApplyConfiguration(new HistoricCourseHasUserConfiguration());
            builder.ApplyConfiguration(new HistoricPathWayHasUserConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new QuizzConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new LevelConfiguration());
            #endregion
            #region Seed Educcassit
            Guid groupGuid = Guid.NewGuid();
            Guid OrganismeGuid = Guid.NewGuid();
            Guid GenderGuid = Guid.NewGuid();
            Guid UserGuid = Guid.NewGuid();
            Guid OccupationGuid = Guid.NewGuid();
            builder.Entity<Occupation>().HasData(
           new Occupation
           {
               Id = OccupationGuid,
               OccupationName = "SuperAdmin",
               NormalizedName = "SuperAdmin"
           },
            new Occupation
            {
                Id = Guid.NewGuid(),
                OccupationName = "Admin",
                NormalizedName = "Admin"
            },
             new Occupation
             {
                 Id = Guid.NewGuid(),
                 OccupationName = "Professor",
                 NormalizedName = "Professeur"
             },
              new Occupation
              {
                  Id = Guid.NewGuid(),
                  OccupationName = "Visitor",
                  NormalizedName = "Visiteur"
              },
               new Occupation
               {
                   Id = Guid.NewGuid(),
                   OccupationName = "Student",
                   NormalizedName = "Étudiant"
               },
                new Occupation
                {
                    Id = Guid.NewGuid(),
                    OccupationName = "Tutor",
                    NormalizedName = "Tuteur"
                }
             );
            builder.Entity<Organization>().HasData(
              new Organization { Id = OrganismeGuid,ImageUrl ="Bonsoir ceci est un Test" ,Name = "ASBL Educassist", Address = "Rue de Tubize 40", ZipCode = "1460", City = " Ittre", Country = "Be", Email = "contact@educassist.com", Phone = "068659787" }
            );
            builder.Entity<Group>().HasData(
               new Group { Id = groupGuid, Name = "Public", OrganizationId = OrganismeGuid }
            );
            builder.Entity<Gender>().HasData(
              new Gender { Id = Guid.NewGuid(), Name = "Femme", Abbreviated = "Mme." },
              new Gender { Id = GenderGuid, Name = "Homme", Abbreviated = "M." },
              new Gender { Id = Guid.NewGuid(), Name = "Autre", Abbreviated = "X." }
          );

            builder.Entity<User>().HasData(
                new User
                {
                    FirstName = "Kevin",
                    LastName = "Jouret",
                    Birthday = DateTime.Now,
                    Country = "Be",
                    Address = "Rue de Virginal 40",
                    ZipCode = "1460",                   
                    City = "Virginal",
                    Phone = "0202020202",
                    LoginMail = "kjouret@educassist.be",
                    CreationDate = DateTime.Now,
                    DeleteDate = null,
                    UpdateDate = null,
                    GenderId = GenderGuid,
                    Gender = null,
                    NormalizedEmail = "KJOURET@EDUCASSIST.BE",
                    Email = "kjouret@educassist.be",
                    UserName = "kjouret@educassist.be",
                    PhoneNumber = "0202020202",
                    Id = UserGuid,
                    NormalizedUserName = "KJOURET@EDUCASSIST.BE",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEEkr3VDDQoZs+mcc1PKqb+kqQDK/FCpRUQt5pevvTHravL3Q6dDqK0GU6AG8yiHeEg==",
                    SecurityStamp = "REMB6AADPUCZ7YLKWCN6MFX3HOMII7WL",
                    ConcurrencyStamp = "e186463d-5e90-4174-b04f-8ee1240f6bf8",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                }               
            );
            builder.Entity<UserHasGroup>().HasData(
               new UserHasGroup
               {
                   Id = Guid.NewGuid(),
                   GroupId = groupGuid,
                   UserId = UserGuid,
               }
           );
            builder.Entity<UserHasOccupation>().HasData(
               new UserHasOccupation
               {
                   Id = Guid.NewGuid(),
                   OccupationId = OccupationGuid,
                   UserId = UserGuid,
                   OrganizationId = OrganismeGuid
               }
           );
            #endregion


            base.OnModelCreating(builder);
        }
    }
}
