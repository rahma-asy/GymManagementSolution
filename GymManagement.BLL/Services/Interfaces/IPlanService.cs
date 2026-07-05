using GymManagement.BLL.ViewModels.PlanVewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken c = default);
        Task<PlanViewModel?> GetPlanByIdAsync(int planId ,CancellationToken c = default);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId ,CancellationToken c = default);
        Task<bool> UpdatePlanAsync(int planId,UpdatePlanViewModel model, CancellationToken c = default);

        Task<bool> ToggleActivationAsync(int planId ,CancellationToken c = default);

    }
}
