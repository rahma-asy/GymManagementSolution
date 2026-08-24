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
    //LINQ queries > no Code Duplication
    //Repository Pattern
    public class GenaricRepository<TEntity> : IGenaricReposatory<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _set;
       public GenaricRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _set=dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _set.Add(entity);
      
        }

        public void Delete(TEntity entity)
        {
            _set.Remove(entity);
        
        }


        public async Task<TEntity?> GetByIDAsync(int id, CancellationToken c = default)
        {
            return await _set.FindAsync(id, c);
        }


        public void Update(TEntity entity)
        {
            _set.Update(entity);
        
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _set : _set.AsNoTracking();
            if (predicate is not null) query = query.Where(predicate);
            return await query.ToListAsync(ct);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken c=default)
        {
            return _set.AsNoTracking().AnyAsync(predicate, c);  
        }

        public async Task<TEntity?> FirstOrDefultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken c = default)
        {
            IQueryable<TEntity> query = tracking ? _set : _set.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, c);
            
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            //if(predicate == null)
            //return await _set.AsNoTracking().CountAsync(ct);
            //else return await _set.AsNoTracking().CountAsync(predicate, ct);
            return   predicate == null ? await _set.AsNoTracking().CountAsync(ct) : await _set.AsNoTracking().CountAsync(predicate, ct);
        }
     
    }
}
