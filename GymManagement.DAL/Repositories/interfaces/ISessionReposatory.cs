using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.interfaces
{
    public interface ISessionReposatory:IGenaricReposatory<Session>
    {
        Task<IEnumerable<Session>> GetALLSessionsWithTrainerAndCategory(CancellationToken c=default);//هترجع كله
        Task<int>GetCountOfBookedSlotsAsync(int sessionId, CancellationToken c=default);
        //هترجع حاجه معينه بتحقق الشرط
   
    Task<Session?> GetSessionByIdWithTrainerAndCategoryAsync( int id, CancellationToken c=default);
    }
}
