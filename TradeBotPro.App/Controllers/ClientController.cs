using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Models.DataModels;
using TradeBotPro.App.Models.FormModels;
using TradeBotPro.App.Models.ViewModels;

namespace TradeBotPro.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public ClientController(DatabaseContext dbContext)
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

            // Get Client From Name and Validate
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
                Status = ClientStatusEnum.Active
            };

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Client");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id == id);
            return View("Edit", (ClientEditFormModel) client);
        }
    }
}