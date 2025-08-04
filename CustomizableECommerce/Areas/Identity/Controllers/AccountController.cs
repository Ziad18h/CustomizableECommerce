using CustomizableECommerce.Areas.Customer.Controllers;
using CustomizableECommerce.Models;
using CustomizableECommerce.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using System.Threading.Tasks;

namespace CustomizableECommerce.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registrVM)
        {
            if(!ModelState.IsValid)
            {
                return View(registrVM);
            }
           


            ApplicationUser applicationuser = new ApplicationUser()
            {
                FirstName = registrVM.FirstName,
                LastName = registrVM.LastName,
                UserName = registrVM.UserName,
                Email = registrVM.Email,
                Address = registrVM.Address,
             
            };

           var result =await _userManager.CreateAsync(applicationuser , registrVM.Password);


            if (result.Succeeded)
            {
                TempData["success-notification"] = "Registration successful! You can now log in.";
                return RedirectToAction(nameof(Index) , "Home" , new { area = "Customer"});
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registrVM);



            }   
        }
    }
}
