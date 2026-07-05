using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(GymDbContext dbContext, ISessionReposatory sessionReposatory)
        {
            _dbContext = dbContext;
            SessionReposatory = sessionReposatory;
        }

        public ISessionReposatory SessionReposatory {  get; }

        public IGenaricReposatory<TEntity> GetRepository<TEntity>()where TEntity : BaseEntity, new()
        {
            // Check TEntity == ??? // Plan , Trainer , Member
            var typeName = typeof(TEntity).Name;

            // If Exists -> Return
            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenaricReposatory<TEntity>)value;
            else
            {
                // If Not -> Create - Store - Return
                var repo = new GenaricRepository<TEntity>(_dbContext);
                _repositories[typeName] = repo;

                return repo;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken c = default)
        {
            //هيشوف اي العمليات اللي عملتها لوكال ويعملها سيف في الداتا بيز
            return await _dbContext.SaveChangesAsync(c);
        }

    }



}

