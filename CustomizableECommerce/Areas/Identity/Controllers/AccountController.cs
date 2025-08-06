using CustomizableECommerce.Areas.Customer.Controllers;
using CustomizableECommerce.Models;
using CustomizableECommerce.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.Win32;
using NuGet.Protocol;
using System.Threading.Tasks;

namespace CustomizableECommerce.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager ,IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationuser);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new {area ="Identity" , userId = applicationuser.Id, token = token },Request.Scheme);


             
                string html = System.IO.File.ReadAllText("EmailTemplates/ConfirmEmail.html");
                html = html.Replace("{{link}}", confirmationLink);

                await _emailSender.SendEmailAsync(registrVM.Email, "Confirm Your Account", html);

                TempData["success-notification"] = "Registration successful! Confirm your email.";
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
    

    public async Task<IActionResult> ConfirmEmail(string userId , string token)
        {
            var user =await _userManager.FindByIdAsync(userId);

            if (user is not null) 
            {
             var result =  await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                    return View();

                TempData["error-notification"] = String.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index), controllerName: "Home", new { area = "Customer" });
            }
            return NotFound();

        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginvm);
            }

            var user =await _userManager.FindByEmailAsync(loginvm.EmailOrUserName) ??
                      await _userManager.FindByNameAsync(loginvm.EmailOrUserName);
            if (user is not null)
            {

                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Please confirm your account via the email we sent you.");
                    return View(loginvm);
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginvm.Password, loginvm.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    TempData["success-notification"] = "Login successful! Welcome back.";
                    return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked due to too many failed attempts. Please try again later.");
                    return View(loginvm);
                }

                ModelState.AddModelError(string.Empty, "The password you entered is incorrect.");
                return View(loginvm);
            }

            
            ModelState.AddModelError(string.Empty, "Invalid username or email.");
            return View(loginvm);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success-notification"] = "Logout Successfully";
            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }



        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmationAsync(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resendEmailConfirmationVM);
            }
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.EmailOrUserName) ??
                        await _userManager.FindByNameAsync(resendEmailConfirmationVM.EmailOrUserName);
            if (user is not null)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { area = "Identity", userId = user.Id, token = token }, Request.Scheme);



                string html = System.IO.File.ReadAllText("EmailTemplates/ConfirmEmail.html");
                html = html.Replace("{{link}}", confirmationLink);

                await _emailSender.SendEmailAsync(user.Email!, "Confirm Your Account", html);

            }

            TempData["success-notification"] = "Confirm your email.";
            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }
    }

}
