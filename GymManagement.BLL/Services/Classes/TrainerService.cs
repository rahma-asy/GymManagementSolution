using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken c = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync();

            if (!trainers.Any()) return [];
            var trainersViewModel =_mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return trainersViewModel;
        }
       
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken c = default)
        {
            var EmailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, c);
            var PhoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, c);
            if (PhoneExist || EmailExist) return false;
            var trainer = _mapper.Map<Trainer>(model);
            _unitOfWork.GetRepository<Trainer>().Add(trainer);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }


        public async Task<TrainerViewModel?> GetTrainerDetailsByIdAsync(int TrainerId, CancellationToken c = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIDAsync(TrainerId);
            if (trainer == null) return null;
            else
            return _mapper.Map<TrainerViewModel>(trainer);
             
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int TrainerId, CancellationToken c = default)
        {
         var model= await _unitOfWork.GetRepository<Trainer>().GetByIDAsync(TrainerId);
            if (model == null) return null;
            else
                return _mapper.Map<TrainerToUpdateViewModel>(model);
            }

        //public async Task<bool> UpdateTrainerDetailsAsync(int id,TrainerToUpdateViewModel model,CancellationToken c = default)
        //{
        //    var trainer = await _unitOfWork
        //        .GetRepository<Trainer>()
        //        .GetByIDAsync(id, c);

        //    if (trainer == null)
        //        return false;

        //    var emailExists = await _unitOfWork
        //        .GetRepository<Trainer>()
        //        .AnyAsync(m => m.Email == model.Email && m.Id != id, c);

        //    var phoneExists = await _unitOfWork
        //        .GetRepository<Trainer>()
        //        .AnyAsync(m => m.Phone == model.Phone && m.Id != id, c);

        //    if (emailExists || phoneExists)
        //        return false;

        //    // Update the existing entity
        //    _mapper.Map(model, trainer);

        //    trainer.UpdatedAt = DateTime.Now;

        //    _unitOfWork.GetRepository<Trainer>().Update(trainer);

        //    var result = await _unitOfWork.SaveChangesAsync(c);

        //    return result > 0;
        //}
        public async Task<bool> UpdateTrainerDetailsAsync(
    int id,
    TrainerToUpdateViewModel model,
    CancellationToken c = default)
        {
            var trainer = await _unitOfWork
                .GetRepository<Trainer>()
                .GetByIDAsync(id, c);

            if (trainer == null)
                return false;

            var emailExists = await _unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(m => m.Email == model.Email && m.Id != id, c);

            var phoneExists = await _unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(m => m.Phone == model.Phone && m.Id != id, c);

            if (emailExists || phoneExists)
                return false;

            // قبل الـ Map
            Console.WriteLine($"OLD EMAIL: {trainer.Email}");
            Console.WriteLine($"NEW EMAIL: {model.Email}");

            _mapper.Map(model, trainer);

            // بعد الـ Map
            Console.WriteLine($"AFTER MAP EMAIL: {trainer.Email}");

            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);

            var result = await _unitOfWork.SaveChangesAsync(c);

            Console.WriteLine($"SAVE RESULT: {result}");

            return result > 0;
        }

        public async Task<bool> RemoveTrainerAsync(int id, CancellationToken c = default)
        {
            {
                var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIDAsync(id, c);
                if (trainer == null) return false;

                var hasFuturesessions = await _unitOfWork.GetRepository<Session>().AnyAsync(x => x.TrainerId == id && x.StartDate >DateTime.Now,c);
                if (hasFuturesessions) return false;

                _unitOfWork.GetRepository<Trainer>().Delete(trainer);
                var result = await _unitOfWork.SaveChangesAsync(c);
                return result > 0;

            }
        }

     
    }
}
