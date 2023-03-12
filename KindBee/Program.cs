using KindBee.DB;
using KindBee.DB.DAL;
using KindBee.DB.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllersWithViews();


builder.Services.AddControllers();
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
builder.Services.AddHttpContextAccessor();
//builder.Services.AddDbContext<KindBeeDBContext>(options => options.UseSqlServer(builder.Configuration["DefaultConnection"]));

//builder.Services.AddTransient<IDataAccess, >();
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();   // добавление middleware авторизации 
//app.UseMiddleware<MiddleWare>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
