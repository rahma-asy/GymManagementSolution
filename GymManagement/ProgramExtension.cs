using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.DataSeeding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public static  class ProgramExtension
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            //to let FrameWork create dbcontext obj without constractor
            // by request feom FrameWork to enject container obj

            //scope that have container to talk with service provider insted of inject in constractor
            using var scope = app.Services.CreateScope();
            //search for GymDbContextand his configuration then get a obj
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();



            #region for seeding users and roles [last session]
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicaionUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); 
            #endregion


            var pendingMigration = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigration.Any())
            {
                logger.LogInformation($"Appling {pendingMigration.Count()} Pending Migrations");
                await dbContext.Database.MigrateAsync(); 
            }

      //  F:\route\08 MVC\session6\demo\GymManagementSolution\GymManagement\   wwwroot\
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath,"wwwroot","Files");

            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);

            //session 6
            await IdentityDataSeeding.SeedIdentityData(roleManager, userManager, logger);
        }
    }
}

