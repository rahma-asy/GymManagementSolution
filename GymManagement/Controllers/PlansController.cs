using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanVewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {

        private readonly IPlanService _planService;
        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }
    
        public async Task<IActionResult> Index(CancellationToken c)
        {
            var plans = await _planService.GetAllPlansAsync(c);
            return View(plans);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken c)
        {
            var plan = await _planService.GetPlanByIdAsync(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";

                return RedirectToAction(nameof(Index)); }
            else
                return View(plan);
        }
       

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken c)
        {
            var plan = await _planService.GetPlanToUpdateAsync(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Cannot Be Edited";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
      
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatePlanViewModel model, CancellationToken c)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _planService.UpdatePlanAsync(id, model,c);
            if (result)

                TempData["successMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Updated Plan";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Activete(int id, CancellationToken c) 
        {
           
            var result = await _planService.ToggleActivationAsync(id, c);
            if (result)

                TempData["successMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed to Changed Status";

            return RedirectToAction(nameof(Index));
        }


    }
}
