using Application.Contexts.Mongo;
using Application.Visitors.OnlineVisitorService;
using Application.Visitors.SaveVisitorInfo;
using Domain.Users;
using Infrastructure.IdentityConfigs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Contexts.Mongo;
using WebSite.EndPoints.Hubs;
using WebSite.EndPoints.Utilities.Filters;
using WebSite.EndPoints.Utilities.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

#region Identity

builder.Services.AddIdentityService(builder.Configuration);

builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<CustomIdentityError>();

#endregion

builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    option.LoginPath = "/Account/Login";
    option.AccessDeniedPath = "/Account/AccessDenied";
    option.SlidingExpiration = true;
});

#region ConnectionString

string sqlConnectionString = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(sqlConnectionString));

#endregion

builder.Services.AddScoped<SaveVisitorInfoActionFilter>();
builder.Services.AddTransient<IOnlineVisitorService, OnlineVisitorService>();
builder.Services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseOnlineVisitorId();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OnlineVisitorHub>("/onlineVisitorHub");

app.Run();
