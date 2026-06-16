using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GLMS.API.Data;
using GLMS.API.Models;

namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/contracts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _context.Contracts
                .Include(c => c.Client)
                .ToListAsync();
            return Ok(contracts);
        }

        // GET: api/contracts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound(new { message = $"Contract with ID {id} not found" });

            return Ok(contract);
        }

        // GET: api/contracts/filter
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            var contracts = await query.ToListAsync();
            return Ok(contracts);
        }

        // POST: api/contracts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = contract.ContractId }, contract);
        }

        // PATCH: api/contracts/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
                return NotFound(new { message = $"Contract with ID {id} not found" });

            contract.Status = status;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Status updated successfully", status = contract.Status });
        }

        // DELETE: api/contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
                return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}