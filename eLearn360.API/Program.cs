using elearn360.API.Config.Email.EmailServices;
using elearn360.API.Config.Middleware;
using eLearn360.API.Config.ConfigIdentity;
using eLearn360.API.Config.Email;
using eLearn360.Data.DBContext;
using eLearn360.Data.Models;
using eLearn360.Data.VM.Policies;
using eLearn360.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Instance DBContext
builder.Services.AddDbContext<Elearn360DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Elearn360DBContext"), b => b.MigrationsAssembly("eLearn360.API")), ServiceLifetime.Transient);
//builder.Services.AddDbContext<Elearn360DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Elearn360DBRelease"), b => b.MigrationsAssembly("eLearn360.API")), ServiceLifetime.Transient);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
#endregion
#region Identity Instance
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzçABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
});
builder.Services.AddDefaultIdentity<User>()
                    .AddRoles<Occupation>()
                    .AddEntityFrameworkStores<Elearn360DBContext>()
                    .AddDefaultTokenProviders()
                    .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");
#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtIssuer"],
                ValidAudience = builder.Configuration["JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]))
            };
        });
#endregion
#region EmailConfig
var emailConfig = builder.Configuration
   .GetSection("EmailConfiguration")
   .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
#endregion
#region Autorization Policies
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
    config.AddPolicy(Policies.IsProfessor, Policies.IsProfessorPolicy());
    config.AddPolicy(Policies.IsStudent, Policies.IsStudentPolicy());
    config.AddPolicy(Policies.IsSuperAdmin, Policies.IsSuperAdminfPolicy());
    config.AddPolicy(Policies.IsVisitor, Policies.IsVisitorPolicy());
    config.AddPolicy(Policies.IsTutor, Policies.IsTutorfPolicy());
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
