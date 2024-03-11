using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Models.DataModels;
using TradeBotPro.App.Models.FormModels;
using TradeBotPro.App.Models.ViewModels;
using TradeBotPro.App.Utilities;

namespace TradeBotPro.App.Controllers
{
    [Authorize(Roles = UserRoles.SystemAdmin)]
    public class ClientsController : BaseController
    {
        private readonly DatabaseContext _dbContext;

        public ClientsController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            // Get Clients From Database
            var clients = await _dbContext.Clients
                .Include(x => x.Users)
                .ToListAsync();

            // Loop Through Clients and Convert to View Model
            var viewModel = new ClientListViewModel();

            foreach (var client in clients)
            {
                viewModel.Clients.Add((ClientViewModel)client);
            }

            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View("Edit", new ClientEditFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClientEditFormModel clientEditFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
            {
                return View("Edit", clientEditFormModel);
            }

            // Get Client by Name and Validate
            var clientExists = await _dbContext.Clients.AnyAsync(x => x.Name == clientEditFormModel.Name);
            if (clientExists)
            {
                ModelState.AddModelError(nameof(ClientEditFormModel.Name), "Client already exists");
                return View("Edit", clientEditFormModel);
            }

            // Create Client and Add to Database
            var client = new Client
            {
                Name = clientEditFormModel.Name,
                Registrationkey = Guid.NewGuid().ToString("N").ToUpper(),
                Status = ClientStatusEnum.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Clients");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id == id);
            return View("Edit", (ClientEditFormModel) client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientEditFormModel clientEditFormModel)
        {
            // Validate Model State
            if (!ModelState.IsValid)
            {
                return View("Edit", clientEditFormModel);
            }

            // Get Client by Id and Validate
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id == clientEditFormModel.Id);
            if (client == null)
            {
                ModelState.AddModelError(nameof(ClientEditFormModel.Name), "Client not found");
                return View("Edit", clientEditFormModel);
            }

            // Update Client Properties
            client.Name = clientEditFormModel.Name;
            client.Status = Enum.Parse<ClientStatusEnum>(clientEditFormModel.Status);
            client.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Clients");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid clientId, string status)
        {
            // Get Client by Id and Validate
            var client = await _dbContext.Clients
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == clientId);

            if (client == null)
            {
                ModelState.AddModelError(nameof(ClientEditFormModel.Name), "Client not found");
                return RedirectToAction("Index", "Clients");
            }

            // Update Client Status
            client.Status = Enum.Parse<ClientStatusEnum>(status);

            // Suspend Users if Client Status is Suspended
            if (status == ClientStatusEnum.Suspended.ToString())
            {
                foreach (var user in client.Users)
                {
                    user.Status = UserStatusEnum.Suspended;
                }
            }

            await _dbContext.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}