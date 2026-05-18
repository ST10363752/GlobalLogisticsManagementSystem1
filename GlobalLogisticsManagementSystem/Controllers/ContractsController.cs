using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalLogisticsManagementSystem.Data;
using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContractsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var contracts = _context.Contracts.Include(c => c.Client);
            return View(await contracts.ToListAsync());
        }

        // GET: Contracts/Search - LINQ Search/Filter
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate, string? status)
        {
            // Start with all contracts including client data
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            // Filter by Start Date range (LINQ)
            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate.Value);
            }

            // Filter by End Date range (LINQ)
            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate.Value);
            }

            // Filter by Status (Draft, Active, Expired, OnHold) (LINQ)
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            var contracts = await query.ToListAsync();

            // Store filter values for display in the view
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = status;
            ViewBag.IsSearchResult = true;

            return View("Index", contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.ContractId == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name");
            return View();
        }

        // POST: Contracts/Create (WITH PDF UPLOAD)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,ClientId,StartDate,EndDate,Status,ServiceLevel")] Contract contract, IFormFile? pdfFile)
        {
            if (ModelState.IsValid)
            {
                // Handle PDF Upload
                if (pdfFile != null && pdfFile.Length > 0)
                {
                    // Validate file extension (only PDF allowed)
                    var fileExtension = Path.GetExtension(pdfFile.FileName).ToLower();
                    if (fileExtension != ".pdf")
                    {
                        ModelState.AddModelError("pdfFile", "Only PDF files are allowed.");
                        ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
                        return View(contract);
                    }

                    // Validate file size (max 5MB)
                    if (pdfFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("pdfFile", "File size cannot exceed 5MB.");
                        ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
                        return View(contract);
                    }

                    // Create unique filename
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + pdfFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(fileStream);
                    }

                    contract.SignedAgreementPath = "/uploads/" + uniqueFileName;
                    contract.SignedAgreementFileName = pdfFile.FileName;
                }

                _context.Add(contract);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Contract created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // POST: Contracts/Edit/5 (WITH PDF UPLOAD)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractId,ClientId,StartDate,EndDate,Status,ServiceLevel,SignedAgreementPath,SignedAgreementFileName")] Contract contract, IFormFile? pdfFile)
        {
            if (id != contract.ContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle PDF Upload (if new file is provided)
                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        // Validate file extension (only PDF allowed)
                        var fileExtension = Path.GetExtension(pdfFile.FileName).ToLower();
                        if (fileExtension != ".pdf")
                        {
                            ModelState.AddModelError("pdfFile", "Only PDF files are allowed.");
                            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
                            return View(contract);
                        }

                        // Validate file size (max 5MB)
                        if (pdfFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("pdfFile", "File size cannot exceed 5MB.");
                            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
                            return View(contract);
                        }

                        // Delete old file if exists
                        if (!string.IsNullOrEmpty(contract.SignedAgreementPath))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, contract.SignedAgreementPath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Save new file
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + pdfFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await pdfFile.CopyToAsync(fileStream);
                        }

                        contract.SignedAgreementPath = "/uploads/" + uniqueFileName;
                        contract.SignedAgreementFileName = pdfFile.FileName;
                    }

                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Contract updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.ContractId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.ContractId == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5 (WITH FILE DELETION)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                // Delete the associated PDF file if it exists
                if (!string.IsNullOrEmpty(contract.SignedAgreementPath))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, contract.SignedAgreementPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Contract deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Download PDF file
        public async Task<IActionResult> DownloadFile(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
            {
                TempData["Error"] = "File not found.";
                return RedirectToAction(nameof(Index));
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, contract.SignedAgreementPath.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                TempData["Error"] = "File not found on server.";
                return RedirectToAction(nameof(Index));
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = contract.SignedAgreementFileName ?? "signed_agreement.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.ContractId == id);
        }
    }
}