using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken c)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(c);
            return View(sessions);
        }

        #region create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken c)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownListAsync();//عشان تفضل معايا لو رجعت من غير مدخل الداتا عشان فيه خطا معين
                return View(model);
            }
            var result = await _sessionService.CreateSessionAsync(model, c);
            if (result.success)
            {
                TempData["successMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
                TempData["ErrorMessage"] = result.error;
            await PopulateDropDownListAsync();
            return View(model);
        }

        #endregion


        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);

            if (result.success)
                return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        } 
        #endregion


        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);

            if (result.success)
            {
                await TrainerList();

                return View(result.value);
            }
            else

            {
                TempData["ErrorMessage"] = result.error; return RedirectToAction(nameof(Index));
            } 
        }
        [HttpPost]

        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {

            if (!ModelState.IsValid)
            {
                await TrainerList();
                return View(model);
            }

            var result = await _sessionService.UpdateSessionDetailsAsync(id, model, ct);

            if (result.success)

            {
                TempData["SuccessMessage"] = "Session Updated"; 
                return RedirectToAction(nameof(Index));
            }
            else

            {
               await TrainerList();
                TempData["ErrorMessage"] = result.error;
           
                return View(model);
            }
        }


        #endregion


        #region delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken c)
        {
            var result=await _sessionService.GetSessionByIdAsync(id, c);
            if (result.success)
            {
              return  View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken c) 
        {
            var result = await _sessionService.RemoveSessionAsync(id, c);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Deleted";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        

        #endregion



        //helpers
        private async Task PopulateDropDownListAsync() //كدا لما يرجع للصفح اللي هحددها هيبقي معاهدول مش لو باظ ف مكان خلاص كدا
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");//select listعشان هناك المفروض تبقي 
                                                                                                                 //ايه القيمه اللي معاه وايه اللي هيظهر فلليست 
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(), "Id", "CategoryName");
        }
        private async Task TrainerList()
        {

            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
        }


    }
}