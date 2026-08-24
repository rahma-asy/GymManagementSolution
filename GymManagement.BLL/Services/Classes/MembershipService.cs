using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MembershipService(IUnitOfWork unitOfWork, IMapper mapper) : IMembershipService
    {
        public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            var memberExists = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
            if (!memberExists) return Result.NotFound("Member not found.");

            var plan = await unitOfWork.GetRepository<Plan>().GetByIDAsync(model.PlanId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (!plan.IsActive) return Result.Fail("Plan is not active.");


            var hasActive = await unitOfWork.MembershipRepository
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActive) return Result.Fail("Member already has an active membership.");

            var entity = new MemberShip
            {
                MemberId = model.MemberId,
                PlanId = plan.Id,
                CreatedAt = DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.DurationDays),
            };

            unitOfWork.MembershipRepository.Add(entity);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Create New Membership");
        }

        public async Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken ct = default)
        {
            var active = await unitOfWork.MembershipRepository.FirstOrDefultAsync(
                m => m.MemberId == memberId && m.EndDate > DateTime.Now, tracking: true, c: ct);

            if (active is null) return Result.NotFound("No active membership for this member.");

            unitOfWork.MembershipRepository.Delete(active);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Membership");
        }

        public async Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await unitOfWork.MembershipRepository
                .GetAllMembershipsWithMemberAndPlanAsync(m => m.EndDate > DateTime.Now, ct);
            return mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(p => p.IsActive, ct: ct);
            return mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            return mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }
    }
}
