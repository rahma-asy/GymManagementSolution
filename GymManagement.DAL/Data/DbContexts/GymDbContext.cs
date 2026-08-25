
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace GymManagement.DAL.Data.DbContexts
{
    public class GymDbContext:IdentityDbContext<ApplicaionUser>
    {

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //identity configuration
            base.OnModelCreating(modelBuilder);
        }

       public DbSet <Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Membership> MemberShips { get; set; }

        //public DbSet<ApplicaionUser> Users { get; set; }// instead of <IdentityUsers>

        //public DbSet<IdentityRole> Roles { get; set; }
        ////we  have a m-m relationshio between Roles&Users
        //public DbSet <IdentityUserRole<string>> UserRoles { get; set; }
    }
}
