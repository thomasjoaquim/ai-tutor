using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Services;
using MyProject.Extensions;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure configuration to use environment variables
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Database
var connectionString = Environment.GetEnvironmentVariable("DB_SERVER") != null 
    ? $"Server={Environment.GetEnvironmentVariable("DB_SERVER")};Database={Environment.GetEnvironmentVariable("DB_DATABASE")};Uid={Environment.GetEnvironmentVariable("DB_USER")};Pwd={Environment.GetEnvironmentVariable("DB_PASSWORD")};Port={Environment.GetEnvironmentVariable("DB_PORT")};" 
    : "Server=localhost;Database=my_database;Uid=admin;Pwd=P@zzword1;Port=3306;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Services
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMemorialService, MemorialService>();
builder.Services.AddScoped<TokenLimitService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<QRCodeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

// Initialize database
await app.Services.InitializeDatabaseAsync();

app.Run();
