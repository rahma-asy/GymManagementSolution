using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
     public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIDAsync(sessionId, ct);
            if (session is null) return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot cancel a booking for a session that has already started.");

            var booking = await _unitOfWork.BookingRepository.FirstOrDefultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, tracking: true, c: ct);
            if (booking is null) return Result.NotFound("Booking not found.");

            _unitOfWork.BookingRepository.Delete(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Booking Cancel Failed");
        }
        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.FirstOrDefultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, tracking: true, c: ct);
            if (booking is null) return Result.NotFound("Booking not found.");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;
            _unitOfWork.BookingRepository.Update(booking);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to Mark As Attended");
        }
        public async Task<Result> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIDAsync(model.SessionId, ct);
            if (session is null) return Result.NotFound("Session not found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot book a session that has already started.");

            var hasActiveMembership = await _unitOfWork.MembershipRepository
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (!hasActiveMembership)
                return Result.Fail("Member does not have an active membership.");

            // Prevent double-booking the same member into the same session.
            var alreadyBooked = await _unitOfWork.BookingRepository
                .AnyAsync(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId, ct);
            if (alreadyBooked)
                return Result.Fail("Member is already booked for this session.");

            var booked = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(model.SessionId, ct);
            if (booked >= session.Capacity)
                return Result.Fail("Session is full.");

            _unitOfWork.BookingRepository.Add(new Booking
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false,
                CreatedAt = DateTime.Now,
            });

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Book Session");
        }
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {

            var bookings = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(x => x.EndDate >= DateTime.Now);
            if (!bookings.Any()) return null!;
            var MappedSession = _mapper.Map<IEnumerable<SessionViewModel>>(bookings);
            foreach (var item in MappedSession)
            {
                item.AvailableSlots = item.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(item.Id);
            }
            return MappedSession;
        }
        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(
         int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionIdAsync(sessionId, ct);
            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = sessionId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            }).ToList();
        }
        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(
         int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionIdAsync(sessionId, ct);
            return bookings.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = sessionId,
                MemberName = b.Member.Name,
                BookingDate = b.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                IsAttended = b.IsAttended,
            }).ToList();
        }
        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository
                                             .GetAllAsync(x => x.SessionId == sessionId);

            var bookedMemberIds = booking.Select(x => x.MemberId);

            var availableMembers = await _unitOfWork.GetRepository<Member>()
                                              .GetAllAsync(x => !bookedMemberIds.Contains(x.Id));

            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembers);
        }
    }
}

