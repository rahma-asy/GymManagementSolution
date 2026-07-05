using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{ 

    // helper class to add seeding methods and call it before any app request[in program]
    public static class GymDataSeeding
    {                                                             // هيقرا منين[loction]
        public static async Task SeedAsync(GymDbContext dbContext, string seedFolderPath, ILogger logger,CancellationToken c=default) 
        {
            try
            {
                   //plan must be empty
                if (!await dbContext.Plans.AnyAsync())
                { 

                    var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");
                    // لو في داتا ممكن ادخلها
                    if (plans.Any())
                    {
                        //add locally
                        dbContext.Plans.AddRange(plans);
                        logger.LogInformation($"Plans Seeded With Count = {plans.Count}");
                    }

                    // SaveChangesAsync
                    if (dbContext.ChangeTracker.HasChanges())
                        await dbContext.SaveChangesAsync();
                    else
                        logger.LogInformation("Plan Already Seeded");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Seeding Failed");
                throw;
            }
        }

        public static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
        {
           
            var filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Seed Data File Not Found !");
            // read from path and return as string

            var data = File.ReadAllText(filePath);

            //to be collection of plans
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
