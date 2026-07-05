using AutoMapper;
using AutoMapper.Execution;
using GymManagement.BLL;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanVewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Member = GymManagement.DAL.Data.Models.Member;
namespace GymManagement.BLL
{
    public class MappingProfile :Profile //function [createmap]  ضامن ان عندي    
    {
        public MappingProfile()
        {
            MapMember();
            MapSession();
            MapTrainer();
            MapPlan();
            MapBooking();
            MapMembership();
        }
        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>()//sourse and destination  // create new obj from sourse , destination and assign data and return it
                .ForMember(d => d.Address, o => o.MapFrom(s => $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateofBirth.ToShortDateString())).ReverseMap();//ركزي بتاخد ايه


            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
            CreateMap<Member, MemberToUpdateViewModel>()
             .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
             .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
             .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                 .ForMember(dest => dest.Name, opt => opt.Ignore())
                 .ForMember(dest => dest.Photo, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.Address.BuildingNumber = src.BuildingNumber;
                     dest.Address.Street = src.Street;
                     dest.Address.City = src.City;
                 });
            CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
            .ForMember(dest => dest.HealthRecord,
                opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }
        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer,TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, SessionViewModel>().
                ForMember(d => d.TrainerName, opt => opt.MapFrom(s=>s.Trainer.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.CategoryName));

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap(); 

        }
        private void MapTrainer()  
        {
            //CreateMap<Trainer, TrainerViewModel>();
            CreateMap<CreateTrainerViewModel, Trainer>().ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
            CreateMap<Trainer, TrainerViewModel>().ForMember(d => d.Address, o => o.MapFrom(s => $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"))
                .ForMember(d => d.DateofBirth, o => o.MapFrom(s => s.DateofBirth.ToShortDateString())).
                ForMember(d => d.Spectatty, o => o.MapFrom(s => s.Spectatty.ToString()))
                .ReverseMap();//ركزي بتاخد ايه

            CreateMap<CreateTrainerViewModel, Trainer>()
       .ForMember(d => d.DateofBirth,
           o => o.MapFrom(s => s.DateOfBirth.ToDateTime(TimeOnly.MinValue)))
       .ForMember(d => d.Spectatty,
           o => o.MapFrom(s => s.Spectatty))
       .ForMember(d => d.Address,
           o => o.MapFrom(s => new Address
           {
               BuildingNumber = s.BuildingNumber,
               City = s.City,
               Street = s.Street
           }));



            CreateMap<Trainer, TrainerToUpdateViewModel>()
         .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
         .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
         .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                 .ForMember(dest => dest.Name, opt => opt.Ignore())
             .AfterMap((src, dest) =>
                 {
                     dest.Address.BuildingNumber = src.BuildingNumber;
                     dest.Address.Street = src.Street;
                     dest.Address.City = src.City;
                 });


        }
        private void MapPlan()      
        {
            CreateMap<Plan, PlanViewModel>().ReverseMap();
            CreateMap<Plan,UpdatePlanViewModel>().ReverseMap();
        }
        private void MapBooking()     { }
        private void MapMembership() { }

    }
}


