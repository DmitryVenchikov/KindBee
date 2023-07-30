
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.Json.Serialization;
using KindBee.DB;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllersWithViews().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

//builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
builder.Configuration.AddJsonFile("mySettings.json");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<KindBeeDBContext>(options => options.UseSqlServer(builder.Configuration["DefaultConnection"], b => b.MigrationsAssembly("DOKINWebApplicationMVC")));
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


//builder.Services.AddDbContext<DokinDbContext>();
//builder.Services.AddTransient<IDataAccessCar, CarsDAL>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
//app.UseMvc();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


//// �������� ���� � ����� 
//var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
//// ���� � �������� �������
//var pathToContentRoot = Path.GetDirectoryName(pathToExe);
//// ������� ����
//var host = WebHost.CreateDefaultBuilder(args)
//        .UseContentRoot(pathToContentRoot)
//        //.UseStartup<Startup>()
//        .Build();
//// ��������� � ���� ������
//host.RunAsService();