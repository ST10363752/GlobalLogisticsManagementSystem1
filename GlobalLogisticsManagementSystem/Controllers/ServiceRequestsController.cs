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
            try
            {
                var requests = await _apiService.GetServiceRequestsAsync();
                return View(requests);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading service requests: " + ex.Message;
                return View(new List<ServiceRequest>());
            }
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var request = await _apiService.GetServiceRequestByIdAsync(id);
                if (request == null)
                {
                    return NotFound();
                }
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading details: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ServiceRequests/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var allContracts = await _apiService.GetContractsAsync();
                var activeContracts = allContracts.Where(c => c.Status == "Active").ToList();

                if (!activeContracts.Any())
                {
                    TempData["Error"] = "No Active contracts available. Please create an Active contract first.";
                    return RedirectToAction("Index", "Contracts");
                }

                ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,Description,AmountUSD")] ServiceRequest serviceRequest)
        {
            try
            {
                // Check if ContractId is valid
                if (serviceRequest.ContractId <= 0)
                {
                    ModelState.AddModelError("ContractId", "Please select a valid contract.");
                }

                // Check ModelState
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    TempData["Error"] = "Validation failed: " + errors;

                    var allContracts = await _apiService.GetContractsAsync();
                    var activeContracts = allContracts.Where(c => c.Status == "Active").ToList();
                    ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
                    return View(serviceRequest);
                }

                // Check if contract is Active
                var canCreate = await _apiService.CanCreateServiceRequestAsync(serviceRequest.ContractId);

                if (!canCreate)
                {
                    TempData["Error"] = "Cannot create service request. Contract is not Active. Only Active contracts are allowed.";
                    var allContracts = await _apiService.GetContractsAsync();
                    var activeContracts = allContracts.Where(c => c.Status == "Active").ToList();
                    ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
                    return View(serviceRequest);
                }

                // Set default values
                serviceRequest.RequestDate = DateTime.Now;
                serviceRequest.Status = "Pending";
                serviceRequest.ExchangeRateUsed = 19.00m;
                serviceRequest.AmountZAR = serviceRequest.AmountUSD * 19.00m;

                var result = await _apiService.CreateServiceRequestAsync(serviceRequest);
                TempData["Success"] = "Service request created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating service request: " + ex.Message;
                var allContracts = await _apiService.GetContractsAsync();
                var activeContracts = allContracts.Where(c => c.Status == "Active").ToList();
                ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
                return View(serviceRequest);
            }
        }

        // GET: ServiceRequests/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var request = await _apiService.GetServiceRequestByIdAsync(id);
                if (request == null)
                {
                    return NotFound();
                }
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading edit form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestId,Description,Status")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _apiService.UpdateServiceRequestStatusAsync(id, serviceRequest.Status);
                    TempData["Success"] = "Service request updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                return View(serviceRequest);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating service request: " + ex.Message;
                return View(serviceRequest);
            }
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var request = await _apiService.GetServiceRequestByIdAsync(id);
                if (request == null)
                {
                    return NotFound();
                }
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading delete form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiService.DeleteServiceRequestAsync(id);
                TempData["Success"] = "Service request deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting service request: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}