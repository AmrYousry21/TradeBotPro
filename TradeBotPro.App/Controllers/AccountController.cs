using TradeBotPro.App.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Models.DataModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace TradeBotPro.App.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.Interfaces.IAuthenticationService _authenticationService;

        public AccountController(DatabaseContext dbContext, Services.Interfaces.IAuthenticationService authenticationService)
        {
            _dbContext = dbContext;
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Index()
        {
            // Get User
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId);

            return View((AccountFormModel)user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(AccountFormModel accountFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
                return View("Index", accountFormModel);

            // Get User From Database
            var user = await _dbContext.Users
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == UserId);

            // Update User Properties
            user.FirstName = accountFormModel.FirstName;
            user.LastName = accountFormModel.LastName;
            user.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordFormModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordFormModel changePasswordFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
                return View(changePasswordFormModel);

            // Get User From Database
            var user = await _dbContext.Users
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == UserId);

            // Compare Password to Existing One
            var samePassword = _authenticationService.IsPasswordVerified(user.Password, changePasswordFormModel.Password);
            if (samePassword)
            {
                ModelState.AddModelError(nameof(changePasswordFormModel.Password), "Same password cannot be used");
                return View(changePasswordFormModel);
            }

            // Update User Password
            user.Password = _authenticationService.HashPassword(changePasswordFormModel.Password);
            user.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Register()
        {
            return View(new RegisterFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel registerFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
                return View(registerFormModel);

            // Get Client From Registration Key and Validate
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Registrationkey == registerFormModel.RegistrationKey);
            if (client == null)
            {
                ModelState.AddModelError(nameof(RegisterFormModel.RegistrationKey), "Registration Key is invalid");
                return View(registerFormModel);
            }

            // Check if Email Already Exists
            var emailExists = await _dbContext.Users.AnyAsync(x => x.Email == registerFormModel.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(RegisterFormModel.Email), "Email is already used");
                return View(registerFormModel);
            }

            // Create User and Add to Database
            var user = (User)registerFormModel;
            user.Password = _authenticationService.HashPassword(registerFormModel.Password);
            user.Client = client;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View(new LoginFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel loginFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
                return View(loginFormModel);

            // Get User From Database
            var user = await _dbContext.Users
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Email == loginFormModel.Email);

            if (user == null)
            {
                ModelState.AddModelError(nameof(RegisterFormModel.Email), "Email not found");
                return View(loginFormModel);
            }

            // Verify Password
            var isVerfiedPassword = _authenticationService.IsPasswordVerified(user.Password, loginFormModel.Password);
            if (!isVerfiedPassword)
            {
                ModelState.AddModelError(nameof(RegisterFormModel.Password), "Invalid credentials");
                return View(loginFormModel);
            }

            // Validate User Status and Client Status
            if (user.Status != UserStatusEnum.Active || (user.Client != null && user.Client.Status != ClientStatusEnum.Active))
            {
                ModelState.AddModelError(nameof(RegisterFormModel.Email), $"Account is {(user.Status == UserStatusEnum.Suspended ? "suspended" : "inactive")}");
                return View(loginFormModel);
            }

            // Create User Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            };

            if (user.Client != null)
            {
                claims.Add(new Claim(ClaimTypes.GroupSid, user.Client?.Id.ToString()));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = "/Dashboard"
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect("/Dashboard");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}