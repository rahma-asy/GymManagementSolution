using AutoMapper;
using AutoMapper.Execution;
using GymManagement.BLL;
using GymManagement.BLL.ViewModels.MembershipViewModels;
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
            MapMemberships();
        }
        private void MapMember()
        {
            CreateMap<Member, MemberSelectListViewModel>();
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
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
            CreateMap<Trainer, TrainerViewModel>()
                            .ForMember(dest => dest.Address,
                            opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
          .AfterMap((src, dest) =>
          {
              if (dest.Address == null)
              {
                  dest.Address = new Address();
              }

              dest.Address.BuildingNumber = src.BuildingNumber;
              dest.Address.City = src.City;
              dest.Address.Street = src.Street;
              dest.UpdatedAt = DateTime.Now;
          });
        }
        private void MapPlan()      
        {
            CreateMap<Plan, PlanViewModel>().ReverseMap();
            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();
                CreateMap<Plan, PlanSelectListViewModel>(); 
        }
        private void MapBooking()     { }
        private void MapMemberships()
        {
            CreateMap<Membership, MemberShipForMemberViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                     .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<Membership, MemberShipViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                                          .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<CreateMemberShipViewModel, Membership>();
            CreateMap<Membership, MemberSelectListViewModel>();
         
        }
    }
}



