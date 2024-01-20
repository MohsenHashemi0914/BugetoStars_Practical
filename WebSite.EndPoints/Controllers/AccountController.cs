using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoints.Models.ViewModels.Account;
using WebSite.EndPoints.Utilities.Filters;

namespace WebSite.EndPoint.Controllers
{
    [ServiceFilter(typeof(SaveVisitorInfoActionFilter))]
    public class AccountController : Controller
    {
        #region Constructor

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            if(User.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/");
            }
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if(user is null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsPersistent, true);
            
            if(loginResult is { Succeeded : true })
            {
                return Redirect(model.ReturnUrl ?? "/");
            }
            else if (loginResult.RequiresTwoFactor)
            {
                //
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CustomLogin()
        {
            var user = await _userManager.FindByIdAsync("48b5162f-1185-4ebb-93e0-821c15fc8f35");
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(Profile), "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };

            var registerResult = await _userManager.CreateAsync(user, model.Password);
            if (registerResult.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }

            WriteErrorsOnModelState(registerResult.Errors);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return View();
        }

        #region Utilities

        private void WriteErrorsOnModelState(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        #endregion
    }
}