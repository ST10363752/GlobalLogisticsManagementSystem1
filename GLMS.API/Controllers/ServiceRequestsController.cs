using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GLMS.API.Data;
using GLMS.API.Models;

namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .OrderByDescending(s => s.RequestDate)
                .ToListAsync();
            return Ok(requests);
        }

        // GET: api/servicerequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (request == null)
                return NotFound(new { message = $"Service request with ID {id} not found" });

            return Ok(request);
        }

        // GET: api/servicerequests/can-create/5
        [HttpGet("can-create/{contractId}")]
        public async Task<IActionResult> CanCreate(int contractId)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract == null)
                return Ok(new { canCreate = false, reason = "Contract not found" });

            var canCreate = contract.Status == "Active";
            return Ok(new { canCreate = canCreate, contractStatus = contract.Status });
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            // Check if contract exists
            var contract = await _context.Contracts.FindAsync(request.ContractId);
            if (contract == null)
                return BadRequest(new { message = "Contract not found" });

            // Check if contract is Active
            if (contract.Status != "Active")
                return BadRequest(new { message = $"Cannot create service request. Contract is {contract.Status}. Only Active contracts are allowed." });

            // Set default values
            request.RequestDate = DateTime.Now;
            request.Status = "Pending";
            request.ExchangeRateUsed = 19.00m;
            request.AmountZAR = request.AmountUSD * 19.00m;

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = request.ServiceRequestId }, request);
        }

        // PATCH: api/servicerequests/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = $"Service request with ID {id} not found" });

            request.Status = status;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Status updated successfully", status = request.Status });
        }

        // DELETE: api/servicerequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}