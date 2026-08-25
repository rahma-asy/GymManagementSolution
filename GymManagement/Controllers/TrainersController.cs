using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuberAdmin")]
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async  Task<IActionResult> Index(CancellationToken c) 
        {
            var trainers = await _trainerService.GetAllTrainersAsync(c);
            return View(trainers);
        }
        [HttpGet]
        public async Task<IActionResult> TrainerDetails(int id, CancellationToken c)
        {
            var trainer = await _trainerService.GetTrainerDetailsByIdAsync(id, c);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
    
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken c)
        {
            if (!ModelState.IsValid) return View( model); 
            var result = await _trainerService.CreateTrainerAsync(model, c);
            if (result)
              { 
                TempData["successMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
           
                TempData["ErrorMessage"] = "Failed to Create Trainer";
            return View(model);
        }

      
        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id, CancellationToken c)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, c);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditTrainer([FromRoute] int id, TrainerToUpdateViewModel model, CancellationToken c)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _trainerService.UpdateTrainerDetailsAsync(id, model, c);
            if (result)
{                TempData["successMessage"] = "Trainer Updated Successfully";
             
            }
            else
                TempData["ErrorMessage"] = "Failed to Updated Trainer";
            return RedirectToAction(nameof(Index));
            //  return View(model);
        }







        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken c)
        {
            var member = await _trainerService.GetTrainerDetailsByIdAsync(id, c);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
      
        [HttpPost]                                      
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken c)
        {
            var result = await _trainerService.RemoveTrainerAsync(id, c);
            if (result) 

                TempData["successMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Delete Trainer";

            return RedirectToAction(nameof(Index));
        }
      

    }
}
