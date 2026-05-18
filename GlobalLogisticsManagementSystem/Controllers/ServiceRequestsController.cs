using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalLogisticsManagementSystem.Data;
using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceRequestsController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var serviceRequests = _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .OrderByDescending(s => s.RequestDate);

            return View(await serviceRequests.ToListAsync());
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(m => m.ServiceRequestId == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            var activeContracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status == "Active")
                .ToListAsync();

            ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name");
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestId,ContractId,Description,AmountUSD,AmountZAR,ExchangeRateUsed,Status,RequestDate")] ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Contract not found.");
                var activeContracts = await _context.Contracts.Include(c => c.Client).Where(c => c.Status == "Active").ToListAsync();
                ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name", serviceRequest.ContractId);
                return View(serviceRequest);
            }

            if (contract.Status == "Expired" || contract.Status == "OnHold")
            {
                ModelState.AddModelError("ContractId", $"Cannot create service request. Contract is {contract.Status}. Only Active contracts are allowed.");
                var activeContracts = await _context.Contracts.Include(c => c.Client).Where(c => c.Status == "Active").ToListAsync();
                ViewBag.ContractId = new SelectList(activeContracts, "ContractId", "Client.Name", serviceRequest.ContractId);
                return View(serviceRequest);
            }

            serviceRequest.RequestDate = DateTime.Now;
            serviceRequest.Status = "Pending";
            serviceRequest.ExchangeRateUsed = 19.00m;
            serviceRequest.AmountZAR = serviceRequest.AmountUSD * serviceRequest.ExchangeRateUsed;

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Service request created successfully!";
                return RedirectToAction(nameof(Index));
            }

            var contracts = await _context.Contracts.Include(c => c.Client).Where(c => c.Status == "Active").ToListAsync();
            ViewBag.ContractId = new SelectList(contracts, "ContractId", "Client.Name", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            var contracts = await _context.Contracts.Include(c => c.Client).ToListAsync();
            ViewBag.ContractId = new SelectList(contracts, "ContractId", "Client.Name", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestId,ContractId,Description,AmountUSD,AmountZAR,ExchangeRateUsed,Status,RequestDate")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Service request updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.ServiceRequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var contracts = await _context.Contracts.Include(c => c.Client).ToListAsync();
            ViewBag.ContractId = new SelectList(contracts, "ContractId", "Client.Name", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(m => m.ServiceRequestId == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Service request deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.ServiceRequestId == id);
        }
    }
}