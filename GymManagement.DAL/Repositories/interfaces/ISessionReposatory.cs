using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.interfaces
{
    public interface ISessionReposatory:IGenaricReposatory<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(
             Expression<Func<Session, bool>>? predicate = null,
             CancellationToken ct = default);//هترجع كله
        Task<int>GetCountOfBookedSlotsAsync(int sessionId, CancellationToken c=default);
        //هترجع حاجه معينه بتحقق الشرط
   
    Task<Session?> GetSessionByIdWithTrainerAndCategoryAsync( int id, CancellationToken c=default);
    }
}
