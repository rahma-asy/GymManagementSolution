using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(t => {
                t.HasCheckConstraint("sessionCapacityCheck", "Capacity Between 1 and 25");
                t.HasCheckConstraint("sessionEndDateCheck", "EndDate > StartDate");
            } ); 
        }
    }
}
