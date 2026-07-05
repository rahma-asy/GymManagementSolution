using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.Enums;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken c = default)
        {
            //var sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync();//category مش هتنفعني عشان انا عايزاجيب المدربين وال
            var sessions = await _unitOfWork.SessionReposatory.GetALLSessionsWithTrainerAndCategory(c);
            if (!sessions.Any() || sessions == null) return null;
            var mapedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach(var session in mapedSessions) 
            { 
                session.AvailableSlots=await _unitOfWork.SessionReposatory.GetCountOfBookedSlotsAsync(session.Id,c);
            }
            return mapedSessions;
        }


        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken c = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End Date Must Be Greater Than Start Date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must be in the Future");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity Must Be Between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIDAsync(model.TrainerId);

            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIDAsync(model.CategoryId);

            if (category is null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Specialties>(category.CategoryName,true,out var categorySpecialty);

            if (!isValid || trainer.Spectatty != categorySpecialty)
                return Result.Validation("Trainer and Category Must be the Same !");

            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);

            var result = await _unitOfWork.SaveChangesAsync(c);

            return result > 0? Result.OK():Result.Fail("Failed to Create Session");
        }

      

        public async Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId, CancellationToken c = default)
        {
            var session= await _unitOfWork.SessionReposatory.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, c);
            if (session is null) return Result<SessionViewModel>.NotFound("Session Not Found");
            else
            {
                var mappedsession= _mapper.Map<Session,SessionViewModel>(session);
                mappedsession.AvailableSlots=mappedsession.Capacity-await _unitOfWork.SessionReposatory.GetCountOfBookedSlotsAsync(sessionId);
                return Result<SessionViewModel>.OK(mappedsession);
            }
        
        }

      

        public async Task<Result> RemoveSessionAsync(int id, CancellationToken c = default)
        {
           var session = await _unitOfWork.SessionReposatory.GetByIDAsync(id,c);
            if (session is null) return Result.NotFound("Session Not Found");
            if (session.EndDate >= DateTime.Now) return Result.Fail("Cannot Delete Session that has not Ended yet");
            var booking =await _unitOfWork.SessionReposatory.GetCountOfBookedSlotsAsync(id, c);
            
    if(booking>0) return Result.Fail("Cannot Delete Session that has Booking");
            _unitOfWork.SessionReposatory.Delete(session);
          var result=await  _unitOfWork.SaveChangesAsync(c);
            return result > 0 ? Result.OK():Result.Fail("Failed to Delete Session");
          
            
        }
        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken c = default)
        {
            var session = await _unitOfWork.SessionReposatory.GetByIDAsync(sessionId, c);

            if (session is null)return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)return Result<UpdateSessionViewModel>.Fail("Cannot Update Ongoing Sessions !");

            var bookingCount = await _unitOfWork.SessionReposatory.GetCountOfBookedSlotsAsync(sessionId, c);

            if (bookingCount > 0)return Result<UpdateSessionViewModel>.Fail("Cannot Update Session Already Booked");

            var mappedSession =_mapper.Map<Session, UpdateSessionViewModel>(session);

            return Result<UpdateSessionViewModel>.OK(mappedSession);
        }

        public async Task<Result> UpdateSessionDetailsAsync(int id, UpdateSessionViewModel model, CancellationToken c = default)
        {
            var session = await _unitOfWork.SessionReposatory.GetByIDAsync(id, c);

            if (session is null)return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)return Result.Fail("Cannot Edit Session That Already Started!");
            // لو هو مدخل داتا غلط

            if (model.EndDate <= model.StartDate)return Result.Validation("End Date Must Be After Start Date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must be in the Future");

            var bookedCount = await _unitOfWork.SessionReposatory.GetCountOfBookedSlotsAsync(id);

            if (bookedCount > 0)return Result.Fail("Cannot Update Session Booked With Members");


            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIDAsync(model.TrainerId);

            if (trainer is null)return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIDAsync(session.CategoryId);

            var isValid = Enum.TryParse<Specialties>(category?.CategoryName,true,out var CategorySpecialty);

            if (!isValid || trainer.Spectatty != CategorySpecialty) return Result.Validation("Trainer And Category Not Matched !");
            _mapper.Map(model, session);

            //_mapper.Map<UpdateSessionViewModel, Session>(model);
            //    _mapper.Map(model,session);//with reverse
            session.UpdatedAt = DateTime.Now;


            _unitOfWork.SessionReposatory.Update(session);
            var result=await _unitOfWork.SaveChangesAsync(c);

            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Session");



        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken c = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(c:c);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken c = default)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(c: c);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }
    }
}
