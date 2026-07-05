using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Data.Configurations
{
    public class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
     public void  Configure(EntityTypeBuilder<Plan> builder)
        {
           builder.Property(b=>b.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(b => b.Description).HasMaxLength(200);
            builder.Property(b => b.Price).HasPrecision(10, 2);
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.ToTable(t => {
                t.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");
            });
            //builder.HasData(
            //new Plan
            //{
            //    Id = 1,
            //    Name = "Basic Plan",
            //    Price = 300,
            //    DurationDays = 30,
            //    Description = "Access to gym equipment during staffed hours",
            //    IsActive = true
            //},
            //new Plan
            //{
            //    Id = 2,
            //    Name = "Standard Plan",
            //    Price = 500,
            //    DurationDays = 60,
            //    Description = "Includes gym equipment and 2 group classes per week",
            //    IsActive = false
            //},
            //new Plan
            //{
            //    Id = 3,
            //    Name = "Premium Plan",
            //    Price = 900,
            //    DurationDays = 90,
            //    Description = "Unlimited access to equipment, classes, and sauna",
            //    IsActive = false
            //},
            //new Plan
            //{
            //    Id = 4,
            //    Name = "Annual Plan",
            //    Price = 3000,
            //    DurationDays = 365,
            //    Description = "Full year access with personal trainer sessions",
            //    IsActive = true
            //}
     //   );
        }
    }

}
