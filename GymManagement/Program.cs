using GymManagement.BLL;
using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.classes;
using GymManagement.DAL.Repositories.interfaces;
using GymManagement.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //to create obj of container , get actions and views
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //when he ask for IPlanRepository sent PlanRepository
            // builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddDbContext<GymDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            builder.Services.AddScoped(typeof(IGenaricReposatory<>), typeof(GenaricRepository<>));
            builder.Services.AddScoped<IMemberService,MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<ISessionReposatory, SessionReposatory>();
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));//MappingProfile عشان يعرف هيحول ازاي هيروح يدور في  


            #region Services of last session
            builder.Services.AddIdentity<ApplicaionUser, IdentityRole>(configuration =>
              {
                  // add any configuration
                  configuration.Password.RequireUppercase = true;//defult
                  configuration.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                  configuration.Lockout.MaxFailedAccessAttempts = 1;
              }).AddEntityFrameworkStores<GymDbContext>();  //must added it links IdentityDbContext and GymDbContext 


            //control Cookie
            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Acount/Login";//defult      
                option.AccessDeniedPath = "/Acount/AccessDenied ";//defult >انا مش محدد اصلا هيروح فين فالاكشنز دي لا دا الافتراضي
            }); 
            #endregion



            var app = builder.Build();
            //seeding After rejester Services and creating container and build app
            //seeding before congigure pipline to be ready for requests
            await app.MigrateAndSeedDataAsync();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
