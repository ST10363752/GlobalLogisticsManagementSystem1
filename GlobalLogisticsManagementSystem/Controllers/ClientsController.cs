using Microsoft.AspNetCore.Mvc;
using GlobalLogisticsManagementSystem.Models;
using GlobalLogisticsManagementSystem.Services;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IApiService _apiService;

        public ClientsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _apiService.GetClientsAsync();
            return View(clients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _apiService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateClientAsync(client);
                TempData["Success"] = "Client created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _apiService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientId) return NotFound();

            if (ModelState.IsValid)
            {
                await _apiService.UpdateClientAsync(id, client);
                TempData["Success"] = "Client updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _apiService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteClientAsync(id);
            TempData["Success"] = "Client deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}