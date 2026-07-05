using GymManagement.DAL.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.interfaces
{
    public interface IGenaricReposatory<TEntity> where TEntity : BaseEntity , new()
    {
        Task<TEntity?> GetByIDAsync(int id, CancellationToken c = default);
       void Add(TEntity entity);
       void Update(TEntity entity);
    
       void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken c = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate ,CancellationToken c);
       Task<TEntity?> FirstOrDefultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken c=default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate=null,CancellationToken ct = default);
    }

}
