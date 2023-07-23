using Serilog;
using Serilog.Events;
using Serilog;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sec_Forum.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sec_Forum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
                          policy =>
                          {
                              policy.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                          });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/UserMasters/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/UserMasters", "/Login");
    });

 

builder.Services.AddScoped<CustomAuthorizeFilter>();


//var ConnectionString1 = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<EmployeeContext>(options =>
//options.UseSqlServer(ConnectionString1));

var ConnectionString1 =builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<SecForumContext>(options =>
options.UseMySQL(ConnectionString1));

// Cofe for Sessin Expire Time
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});


var app = builder.Build();
//adding log file

//builder.Logging.AddSerilog(new LoggerConfiguration()
//              .MinimumLevel.Information()
//              .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//        .WriteTo.File("logs/Sec_Forum-.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {RequestId} {SourceContext} - {Message}{NewLine}{Exception}")
//        .CreateLogger());

//builder.Services.AddDbContext<EmployeeContext>(options => options.UseSqlServer());
//builder.Services.AddSwaggerGen();
//var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapRazorPages();
//    // Add additional endpoint mappings here if needed
//});

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");
});


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=UserMasters}/{action=Login}");

app.Run();
