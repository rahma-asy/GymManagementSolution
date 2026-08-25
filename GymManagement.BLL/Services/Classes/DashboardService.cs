using AutoMapper.Execution;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.HomeViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.classes;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Member = GymManagement.DAL.Data.Models.Member;

namespace GymManagement.BLL.Services.Classes
{
    public class DashboardService : IDashboardService
    {
        public object  now = DateTime.Now;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DashboardViewModel> GetDashboardAsync(CancellationToken c=default)
        {
            var now = DateTime.Now;

            return new DashboardViewModel
            {
                TotalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct:c),

                ActiveMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(m => m.EndDate>now),
                TotalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: c),

                UpcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > now, c),

                OngoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate <= now && s.EndDate >= now, c),

                CompletedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate < now, c)
            };
        }
    }
}
