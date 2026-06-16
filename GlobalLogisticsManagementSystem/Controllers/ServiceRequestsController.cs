using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GlobalLogisticsManagementSystem.Models;
using GlobalLogisticsManagementSystem.Services;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly IApiService _apiService;

        public ServiceRequestsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var requests = await _apiService.GetServiceRequestsAsync();
            return View(requests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = await _apiService.GetServiceRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // GET: ServiceRequests/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allContracts = await _apiService.GetContractsAsync();
            var activeContracts = allContracts.Where(c => c.Status == "Active").ToList();
            ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            var canCreate = await _apiService.CanCreateServiceRequestAsync(serviceRequest.ContractId);

            if (!canCreate)
            {
                TempData["Error"] = "Cannot create service request. Contract is not Active. Only Active contracts are allowed.";
                var allContracts = await _apiService.GetContractsAsync();
                var activeContracts = allContracts.Where(c => c.Status == "Active").ToList();
                ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
                return View(serviceRequest);
            }

            serviceRequest.RequestDate = DateTime.Now;
            serviceRequest.Status = "Pending";
            serviceRequest.ExchangeRateUsed = 19.00m;
            serviceRequest.AmountZAR = serviceRequest.AmountUSD * 19.00m;

            if (ModelState.IsValid)
            {
                await _apiService.CreateServiceRequestAsync(serviceRequest);
                TempData["Success"] = "Service request created successfully!";
                return RedirectToAction(nameof(Index));
            }

            var contracts = await _apiService.GetContractsAsync();
            var activeContractsList = contracts.Where(c => c.Status == "Active").ToList();
            ViewBag.ContractId = new SelectList(activeContractsList, "ContractId", "Client.Name", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var request = await _apiService.GetServiceRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _apiService.UpdateServiceRequestStatusAsync(id, serviceRequest.Status);
                TempData["Success"] = "Service request updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _apiService.GetServiceRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteServiceRequestAsync(id);
            TempData["Success"] = "Service request deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}