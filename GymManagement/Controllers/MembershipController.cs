using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService; 

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _membershipService.GetAllMembershipsAsync(ct));

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownsAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(ct);
                return View(model);
            }

            var result = await _membershipService.CreateMembershipAsync(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;
            await PopulateDropdownsAsync(ct);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await _membershipService.DeleteActiveMembershipAsync(id, ct);
            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success ? "Membership cancelled." : result.error;
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await _membershipService.GetPlansForDropDownAsync(ct), "Id", "Name");
            ViewBag.Members = new SelectList(await _membershipService.GetMembersForDropDownAsync(ct), "Id", "Name");
        }
    }
}