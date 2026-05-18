using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlobalLogisticsManagementSystem.Data;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get statistics for dashboard
            ViewBag.TotalClients = await _context.Clients.CountAsync();
            ViewBag.TotalContracts = await _context.Contracts.CountAsync();
            ViewBag.ActiveContracts = await _context.Contracts.CountAsync(c => c.Status == "Active");
            ViewBag.TotalServiceRequests = await _context.ServiceRequests.CountAsync();
            ViewBag.PendingRequests = await _context.ServiceRequests.CountAsync(s => s.Status == "Pending");

            // Get recent service requests
            var recentRequests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c != null ? c.Client : null)
                .OrderByDescending(s => s.RequestDate)
                .Take(5)
                .ToListAsync();

            return View(recentRequests);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}