using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityData(RoleManager<IdentityRole> roleManager,
                                    UserManager<ApplicaionUser> userManager,
                                    ILogger logger, CancellationToken c = default)
        {
            try
            {
                //i do seeding Just once
                //save when he runعشان لو في داتا موجوده مش كل مره هيعملها 
                bool HasUsers = await userManager.Users.AnyAsync(c);  //check if users are exist
                bool HasRoles = await roleManager.Roles.AnyAsync(c);  //check if roles are exist
                if (HasUsers && HasRoles) return;

                //create rules
                var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuberAdmin"),
                new IdentityRole("Admin")
            };
                //to add rules
                foreach (var role in roles)
                {
                    //ensure that rolename not exist
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var ruleResult = await roleManager.CreateAsync(role);
                        if (!ruleResult.Succeeded)
                        {
                            logger.LogError($"Failed to Add Role{role.Name}");
                        }
                    }
                }


                //users
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicaionUser()
                    {
                        FirstName = "Rahma",
                        LastName = "Hossam",
                        Email = "rahmahossam@gmail.com"
                        ,
                        UserName = "RahmaAsy",
                        PhoneNumber = "01113687658"
                    };
                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuberAdmin");


                    var Admin = new ApplicaionUser()
                    {
                        FirstName = "sara",
                        LastName = "Hossam",
                        Email = "sarahossam@gmail.com",
                        UserName = "saraAsy",
                        PhoneNumber = "01123687658"
                    };
                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin, "Admin");


                    logger.LogInformation("Identity Seeded Successfully");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }
        }
    }
}
