using Microsoft.EntityFrameworkCore;
using GLMS.API.Models;

namespace GLMS.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { ClientId = 1, Name = "TechMove Logistics", ContactDetails = "contact@techmove.com", Region = "North America" },
                new Client { ClientId = 2, Name = "Global Shipping Inc", ContactDetails = "info@globalshipping.com", Region = "Europe" },
                new Client { ClientId = 3, Name = "Asia Freight Solutions", ContactDetails = "sales@asiafreight.com", Region = "Asia Pacific" }
            );

            // Seed Contracts
            modelBuilder.Entity<Contract>().HasData(
                new Contract { ContractId = 1, ClientId = 1, StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now.AddMonths(6), Status = "Active", ServiceLevel = "Premium" },
                new Contract { ContractId = 2, ClientId = 1, StartDate = DateTime.Now.AddMonths(-12), EndDate = DateTime.Now.AddMonths(-1), Status = "Expired", ServiceLevel = "Standard" },
                new Contract { ContractId = 3, ClientId = 2, StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(3), Status = "Active", ServiceLevel = "Express" },
                new Contract { ContractId = 4, ClientId = 3, StartDate = DateTime.Now.AddMonths(-1), EndDate = DateTime.Now.AddMonths(5), Status = "Draft", ServiceLevel = "Standard" }
            );
        }
    }
}