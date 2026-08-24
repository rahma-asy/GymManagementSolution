using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IBookingService
    {
     Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
    Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(int sessionId, CancellationToken ct = default);
    Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(int sessionId, CancellationToken ct = default);
    Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken ct = default);

    Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
    Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
    Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
}
}

