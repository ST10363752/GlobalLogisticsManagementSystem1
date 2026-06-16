using GLMS.API.Data;
using GLMS.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }

        // GET: api/clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound(new { message = $"Client with ID {id} not found" });
            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, client);
        }

        // PUT: api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            if (id != client.ClientId)
                return BadRequest();

            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        // DELETE: api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}