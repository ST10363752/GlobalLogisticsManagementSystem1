using Microsoft.AspNetCore.Mvc;
using GlobalLogisticsManagementSystem.Services;

namespace GlobalLogisticsManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _apiService.GetClientsAsync();
            var contracts = await _apiService.GetContractsAsync();
            var requests = await _apiService.GetServiceRequestsAsync();

            ViewBag.TotalClients = clients.Count();
            ViewBag.TotalContracts = contracts.Count();
            ViewBag.ActiveContracts = contracts.Count(c => c.Status == "Active");
            ViewBag.TotalServiceRequests = requests.Count();
            ViewBag.PendingRequests = requests.Count(r => r.Status == "Pending");

            var recentRequests = requests.OrderByDescending(r => r.RequestDate).Take(5).ToList();
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