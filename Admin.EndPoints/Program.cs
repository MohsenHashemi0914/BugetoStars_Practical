using Application.Visitors.GetDailyReport;
using Domain.Users;
using Infrastructure.IdentityConfigs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

var sqlConnectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(sqlConnectionString));

#endregion

builder.Services.AddTransient<IGetDailyReportService, GetDailyReportService>();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
