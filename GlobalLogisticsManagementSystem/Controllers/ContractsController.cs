using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GlobalLogisticsManagementSystem.Models;
using GlobalLogisticsManagementSystem.Services;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IApiService _apiService;

        public ContractsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var contracts = await _apiService.GetContractsAsync();
            return View(contracts);
        }

        // GET: Contracts/Search
        [HttpGet]
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate, string? status)
        {
            var contracts = await _apiService.FilterContractsAsync(startDate, endDate, status);
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = status;
            ViewBag.IsSearchResult = true;
            return View("Index", contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        // GET: Contracts/Create
        public async Task<IActionResult> Create()
        {
            var clients = await _apiService.GetClientsAsync();
            ViewBag.ClientId = new SelectList(clients, "ClientId", "Name");
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateContractAsync(contract);
                TempData["Success"] = "Contract created successfully!";
                return RedirectToAction(nameof(Index));
            }

            var clients = await _apiService.GetClientsAsync();
            ViewBag.ClientId = new SelectList(clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();

            var clients = await _apiService.GetClientsAsync();
            ViewBag.ClientId = new SelectList(clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.ContractId) return NotFound();

            if (ModelState.IsValid)
            {
                await _apiService.UpdateContractStatusAsync(id, contract.Status);
                TempData["Success"] = "Contract updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            var clients = await _apiService.GetClientsAsync();
            ViewBag.ClientId = new SelectList(clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteContractAsync(id);
            TempData["Success"] = "Contract deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}