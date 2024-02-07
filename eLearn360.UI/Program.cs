global using Microsoft.AspNetCore.Components;
global using eLearn360.Service.Interface;
global using eLearn360.Service;
global using eLearn360.Data.Models;
global using Sotsera.Blazor.Toaster;
global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Sotsera.Blazor.Toaster.Core.Models;
using eLearn360.Data.VM.Policies;
using Microsoft.AspNetCore.Components.Authorization;
using eLearn360.Service.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<HttpClient>(s =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://localhost:7256/") };
    //var client = new HttpClient { BaseAddress = new Uri("https://elearn360api.azurewebsites.net") };
    return client;

});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>(); ;

builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<IOccupationService, OccupationService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IPathWayService, PathWayService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IStaffHasOccupationService, StaffHasOccupationService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<IQuizzService, QuizzService>();
// ===== TOASTER ===== 
builder.Services.AddToaster(config =>
{
    //Customizations
    config.PositionClass = Defaults.Classes.Position.BottomRight;
    config.PreventDuplicates = false;
    config.NewestOnTop = true;
});

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
    config.AddPolicy(Policies.IsProfessor, Policies.IsProfessorPolicy());
    config.AddPolicy(Policies.IsStudent, Policies.IsStudentPolicy());
    config.AddPolicy(Policies.IsSuperAdmin, Policies.IsSuperAdminfPolicy());
    config.AddPolicy(Policies.IsVisitor, Policies.IsVisitorPolicy());
    config.AddPolicy(Policies.IsTutor, Policies.IsTutorfPolicy());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
