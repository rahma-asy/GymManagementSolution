using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanVewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    
    public class PlanService:IPlanService
    {
    
     private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper) //TO inject repo
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Index
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken c = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: c);

            if (!plans.Any()) return [];
            var plansViewModel =_mapper.Map<IEnumerable<PlanViewModel>>(plans);
            return plansViewModel;
        }

        #endregion

        #region Details
        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken c = default)
        {

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIDAsync(planId, c);
            if (plan == null) return null;
            else 
              return  _mapper.Map<PlanViewModel>(plan);
             
        }
        #endregion

        #region update
        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken c = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIDAsync(planId, c);
            if (plan == null || !plan.IsActive) return null; //من الاول كدا
            var hasActiveMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(p => p.PlanId == planId && p.EndDate > DateTime.Now,c);
            if (hasActiveMembership) return null;
            else
                return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public async Task<bool> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken c = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIDAsync(planId, c);
            if (plan == null) return false;
            var hasActiveMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(p => p.PlanId == planId && p.EndDate > DateTime.Now, c);
            if (hasActiveMembership) return false;

           _mapper.Map(model,plan);
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(c);
            return result > 0;
        }


        #endregion
        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken c = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIDAsync(planId, c);
            var hasActiveMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(p => p.PlanId == planId && p.EndDate > DateTime.Now, c);
            if (plan == null) return false;
           if(plan.IsActive && hasActiveMembership) return false;
         
             plan.IsActive= !plan.IsActive;
            plan.UpdatedAt= DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(c);
                return result > 0;
            


        }
        //private async Task<bool> HasActiveMemberShipAsync(int planId, CancellationToken c)
        //{
        //    return await _unitOfWork.GetRepository<MemberShip>().AnyAsync(p => p.PlanId == planId && p.EndDate > DateTime.Now, c);
        //}

    }
}
