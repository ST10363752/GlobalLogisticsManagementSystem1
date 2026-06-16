using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var contracts = await _apiService.GetContractsAsync();
            return View(contracts);
        }

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

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        public IActionResult Create()
        {
            return View();
        }

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
            return View(contract);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

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
            return View(contract);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _apiService.GetContractByIdAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

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