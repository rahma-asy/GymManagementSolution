using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.interfaces
{
    public interface IUnitOfWork
    {
        IGenaricReposatory<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync(CancellationToken c=default);
        public ISessionReposatory SessionRepository { get; } //محدش يغير فيها
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }


    }
}
