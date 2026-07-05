using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken c = default);
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken c = default);
        Task<TrainerViewModel?> GetTrainerDetailsByIdAsync(int TrainerId, CancellationToken c = default);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int TrainerId, CancellationToken c = default);
        Task<bool> UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel model, CancellationToken c = default);
        Task<bool> RemoveTrainerAsync(int id, CancellationToken c = default);
    }
}
