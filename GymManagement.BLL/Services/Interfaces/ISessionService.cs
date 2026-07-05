using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken c = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken c = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken c = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken c = default);
        Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId, CancellationToken c = default);



        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken c = default);
        Task<Result> UpdateSessionDetailsAsync(int id, UpdateSessionViewModel model, CancellationToken c = default);
       
        
        Task<Result> RemoveSessionAsync(int id, CancellationToken c = default); 
    }
}
