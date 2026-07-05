using GymManagement.BLL.ViewModels.AccountViewModels;
using GymManagement.Controllers;
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicaionUser> _userManager;
        private readonly SignInManager<ApplicaionUser> _signInManager;
      

        public AccountController(UserManager<ApplicaionUser> userManager,
                                 SignInManager<ApplicaionUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
       
        }

        //Get : Login > empty form
        public IActionResult Login()
        {
            return View();
        }
        //Post : SignIn
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,CancellationToken c)
        {

            if(!ModelState.IsValid) return View(model);
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user == null) 
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(model);
            }
            //else i will sign in                                                                        //Lockout rules
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
            if (result.Succeeded)
            {
              //  _logger.LogInformation($"User {user.UserName} Logged in");

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else if(result.IsLockedOut) 
            {
               // _logger.LogWarning($"User {user.UserName} Locked Out");
                ModelState.AddModelError("InvalidLogin", "This Account LockedOut");
                return View(model);
            }else
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(model);

            }
        }


        //log out
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        { 
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
                }
    }
}
