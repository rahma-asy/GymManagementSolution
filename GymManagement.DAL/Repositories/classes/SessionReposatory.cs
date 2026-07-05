using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.classes
{
    public class SessionReposatory:GenaricRepository<Session> ,ISessionReposatory
    {
        private readonly GymDbContext _dbContext;

        public SessionReposatory(GymDbContext dbContext) : base(dbContext)
            {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetALLSessionsWithTrainerAndCategory(CancellationToken c = default)
        {
            var query= _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);
            return await query.ToListAsync(c);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken c = default)
        {
            return await _dbContext.Bookings.AsNoTracking().CountAsync(x=>x.SessionId== sessionId);
        }

        public async Task<Session?> GetSessionByIdWithTrainerAndCategoryAsync(int id, CancellationToken c = default)
        {
            return await _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s=>s.Id==id);
        }
    }
}
