using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Models.DataModels;
using TradeBotPro.App.Models.FormModels;
using TradeBotPro.App.Models.ViewModels;
using TradeBotPro.App.Services.Interfaces;
using TradeBotPro.App.Utilities;

namespace TradeBotPro.App.Controllers
{
    [Authorize(Roles = $"{UserRoles.SystemAdmin},{UserRoles.ClientAdmin}")]
    public class UsersController : BaseController
    {
        private readonly DatabaseContext _dbContext;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(DatabaseContext dbContext, IAuthenticationService authenticationService)
        {
            _dbContext = dbContext;
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            // Get Client and Users
            var client = await _dbContext.Clients
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
                return View("Error", new ErrorViewModel().AddError("You're not authorized to view this page"));

            // Get Value Frorm Claims Based on Provided Claim Type
            if (User.IsInRole(UserRoles.ClientAdmin) && ClientId != id)
                return View("Error", new ErrorViewModel().AddError("You're not authorized to view this page"));

            // Create View Model
            var viewModel = new UserListViewModel
            {
                ClientId = client.Id,
                Users = ((ClientViewModel)client).Users
            };

            return View("Index", viewModel);
        }

        [Route("Client/{clientId}/Users/Add")]
        public IActionResult Add(Guid clientId)
        {
            // Validate Access
            if (User.IsInRole(UserRoles.ClientAdmin) && clientId != ClientId)
                return View("Error", new ErrorViewModel().AddError("You're not authorized to view this page"));

            return View("Edit", new UserEditFormModel
            {
                ClientId = clientId
            });
        }

        [HttpPost]
        [Route("Client/{clientId}/Users/Add")]
        public async Task<IActionResult> Add(UserEditFormModel userEditFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
                return View("Edit", userEditFormModel);

            // Get Client
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id == userEditFormModel.ClientId);
            if (client == null)
                return View("Error", new ErrorViewModel().AddError("Client not found"));

            // Get User by Email and Validate
            var userExists = await _dbContext.Users.AnyAsync(x => x.Email == userEditFormModel.Email);
            if (userExists)
                return View("Error", new ErrorViewModel().AddError("User already exists"));

            // Create User and Add to Database
            var user = new User
            {
                FirstName = userEditFormModel.FirstName,
                LastName = userEditFormModel.LastName,
                Email = userEditFormModel.Email,
                Status = UserStatusEnum.New,
                UserRole = string.IsNullOrWhiteSpace(userEditFormModel.UserRole) ? UserRoleEnum.ClientUser : Enum.Parse<UserRoleEnum>(userEditFormModel.UserRole),
                Password = _authenticationService.HashPassword(userEditFormModel.Password),
                Client = client,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { id = client.Id });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return View("Edit", (UserEditFormModel)user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditFormModel userEditFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
                return View("Edit", userEditFormModel);

            // Get User by Id and Validate
            var user = await _dbContext.Users
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == userEditFormModel.Id);

            if (user == null)
                return View("Error", new ErrorViewModel().AddError("User not found"));

            // Update User Properties
            user.FirstName = userEditFormModel.FirstName;
            user.LastName = userEditFormModel.LastName;
            user.Email = userEditFormModel.Email;
            user.Status = Enum.Parse<UserStatusEnum>(userEditFormModel.Status);
            user.UpdatedAt = DateTime.UtcNow;

            // Change Password if User Enters New One
            if (!string.IsNullOrWhiteSpace(userEditFormModel.Password))
            {
                user.Password = _authenticationService.HashPassword(userEditFormModel.Password);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { id = user.Client.Id });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid userId, string status)
        {
            // Get User by Id and Validate
            var user = await _dbContext.Users
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return View("Error", new ErrorViewModel().AddError("User not found"));

            // Validate Status
            if (user.Client.Status != ClientStatusEnum.Active && status == ClientStatusEnum.Active.ToString())
                return Json(new { success = false, error = "Client must be active to activate a user" });

            // Update User Status
            user.Status = Enum.Parse<UserStatusEnum>(status);
            await _dbContext.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}