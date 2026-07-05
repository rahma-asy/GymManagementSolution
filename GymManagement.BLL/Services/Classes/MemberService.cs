using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.Enums;
using GymManagement.DAL.Repositories.classes;
using GymManagement.DAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymManagement.BLL.Services.Classes
{
    //talk with repo to get data from database
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork ,IMapper mapper ,IAttachmentService attachmentService) //TO inject repo
        {
            _unitOfWork = unitOfWork;
         _mapper = mapper;
           _attachmentService = attachmentService;
        }


        #region index view
        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken c = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(c: c);

            if (!members.Any()) return [];
            var membersViewModel = _mapper.Map< IEnumerable<Member>,IEnumerable<MemberViewModel>>(members);
            return membersViewModel;
        }
        #endregion

        #region create member
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken c = default)
        {
            var EmailExist =await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, c);
            var PhoneExist =await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, c);
            if (PhoneExist || EmailExist) return false;

          //  upload photo
            var storedphotoname = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "MembersPhoto", c);
            if (string.IsNullOrEmpty(storedphotoname)) return false;



            var member = _mapper.Map<Member>(model);  
            member.Photo= storedphotoname;
            _unitOfWork.GetRepository<Member>().Add(member);
            var result = await _unitOfWork.SaveChangesAsync(c);
            if (result > 0) return true;
            else 
            {
                //عشان لو الالشخص منضقش ميرفش الصوره علي الفاضي وتفضل موجوده
              _attachmentService.Delete(storedphotoname, "MembersPhoto");
            return false;
            }
        }


        #endregion

        #region  Get Member Details
        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int id, CancellationToken c = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIDAsync(id, c);
            if (member == null) return null; //من الاول كدا
            var model =_mapper.Map<Member,MemberViewModel>(member);
            var activeMembership = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefultAsync(x => x.Id == id && x.EndDate > DateTime.Now);
            if (activeMembership is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIDAsync(activeMembership.PlanId, c); //relationshiopهجيب البلان اللي فال
                model.PlanName = activePlan.Name;
               model.MemberShipStartDate= activeMembership.CreatedAt.ToString();
                model.MemberShipEndDate = activeMembership.EndDate.ToString();
            }
            return model;   
                }



        #endregion

        #region Health Record Details
        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken c = default)
        {
          var record=await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefultAsync(x=>x.Id == memberId,c:c);
            if (record is null) return null;
            else
            {
                return _mapper.Map<HealthRecord, HealthRecordViewModel>(record);
            }
        }


        #endregion

        #region Update
        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken c = default) 
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIDAsync(memberId, c);
            if (member == null) return null; //من الاول كدا
           else
                return _mapper.Map<MemberToUpdateViewModel>(member);
        }
        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken c = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIDAsync(id, c);

            if (member == null) return false;

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id,c);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id,c);

            if (emailExists || phoneExists) return false;

          _mapper.Map(model,member);
            member.UpdatedAt = DateTime.Now;//عشان تحصل فالاخر لو عندي عمليات كتير
             _unitOfWork.GetRepository<Member>().Update(member);
            var result =await _unitOfWork.SaveChangesAsync(c);
            return result > 0;
        }

        #endregion

        #region Delete
        public async Task<bool> RemoveMemberAsync(int id, CancellationToken c = default) 
        {
        var member=await _unitOfWork.GetRepository<Member>().GetByIDAsync(id,c);
            if (member == null) return false;

            var hasFutureBooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(x => x.MemberId == id && x.Session.StartDate > DateTime.Now,c);
      if (hasFutureBooking) return false;

   _unitOfWork.GetRepository<Member>().Delete(member);
            var result =await _unitOfWork.SaveChangesAsync(c);
            return result > 0;
        
        }
        #endregion

    }
}
