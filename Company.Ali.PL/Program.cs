using AutoMapper;
using Company.Ali.BLL;
using Company.Ali.BLL.Interfaces;
using Company.Ali.BLL.Repositories;
using Company.Ali.DAL.Data.Contexts;
using Company.Ali.DAL.Models;
using Company.Ali.PL.Mapping;
using Company.Ali.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Ali.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Rigester Built-in MVC services 
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow DI For DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); // Allow DI For DepartmentRepository
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI For CompanyDbContext

            // Life Time 
            // builder.Services.AddScoped();    Create Object Life Time Per Request - UnReachable Object
            // builder.Services.AddTransient(); Create Object Life Time Per Operation
            // builder.Services.AddSingleton(); Create Object Life Time Per APP


            builder.Services.AddScoped<IScopedService, ScopedService>(); // Per Request
            builder.Services.AddTransient<ITransientService, TransientService>(); // Per Operation 
            builder.Services.AddSingleton<ISingltonServices, SingltonServices>(); // Per APP


            // Register the Service For Mapping 
            // The Best Life Time for the mapping is the Transient
            builder.Services.AddAutoMapper(typeof(EmployeeProfile));

            // Register the Service For the UnitOfWord Design pattern 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            // Register The Service of Dependency Injection for Identity 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>();



            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            // Use Authentication MiddleWare

            


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            // Use Authorization MiddleWare
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
