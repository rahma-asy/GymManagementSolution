using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.classes
{
    public class MembershipRepository : GenaricRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null,
           CancellationToken ct = default)
        {
             IQueryable<MemberShip> query = _dbContext.MemberShips.AsNoTracking().Include(m => m.Plan).Include(m => m.Member);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }

    }
}
