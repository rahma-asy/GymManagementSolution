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
    internal class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {

    
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(p => p.CreatedAt).HasColumnName("JoinedDate").HasDefaultValueSql("GETDATE()");
           base.Configure(builder);
        }
    }
}

