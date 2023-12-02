using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestoMenus.Entities.Identity;
using RestoMenus.Models;
using System.Security.Claims;

namespace RestoMenus.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl = null)
        {
            //if (!ModelState.IsValid)
            //{ 
            //    return View(loginModel); 
            //}


            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user != null)
            {
                // try to sign in the user with the credentials they provided
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var firstNameClaim = new Claim("FirstName", user.FirstName);
                    await _userManager.AddClaimAsync(user, firstNameClaim);

                    var lastNameClaim = new Claim("LastName", user.LastName);
                    await _userManager.AddClaimAsync(user, lastNameClaim);


                    await _signInManager.RefreshSignInAsync(user);

                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Any(x => x == "Admin"))
                    {
                        return Redirect(Url.Action("Index", "Admin"));
                    }

                    return Redirect(GetRedirectUrl(returnUrl));
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid userName or password.");
                    return View(loginModel);

                }
            }


            return View(loginModel);

        }

        [NonAction]
        public string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }

        private void AddModelErrors(IdentityResult? result)
        {
            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Login), "Admin");
            }
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
