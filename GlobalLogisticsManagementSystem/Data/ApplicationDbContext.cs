using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

            // Seed demo data for Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { ClientId = 1, Name = "TechMove Logistics", ContactDetails = "contact@techmove.com", Region = "North America" },
                new Client { ClientId = 2, Name = "Global Shipping Inc", ContactDetails = "info@globalshipping.com", Region = "Europe" },
                new Client { ClientId = 3, Name = "Asia Freight Solutions", ContactDetails = "sales@asiafreight.com", Region = "Asia Pacific" }
            );

            // Seed demo data for Contracts
            modelBuilder.Entity<Contract>().HasData(
                new Contract { ContractId = 1, ClientId = 1, StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now.AddMonths(6), Status = "Active", ServiceLevel = "Premium", SignedAgreementPath = null, SignedAgreementFileName = null },
                new Contract { ContractId = 2, ClientId = 1, StartDate = DateTime.Now.AddMonths(-12), EndDate = DateTime.Now.AddMonths(-1), Status = "Expired", ServiceLevel = "Standard", SignedAgreementPath = null, SignedAgreementFileName = null },
                new Contract { ContractId = 3, ClientId = 2, StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(3), Status = "Active", ServiceLevel = "Express", SignedAgreementPath = null, SignedAgreementFileName = null },
                new Contract { ContractId = 4, ClientId = 3, StartDate = DateTime.Now.AddMonths(-1), EndDate = DateTime.Now.AddMonths(5), Status = "Draft", ServiceLevel = "Standard", SignedAgreementPath = null, SignedAgreementFileName = null }
            );

            // Seed demo data for ServiceRequests
            modelBuilder.Entity<ServiceRequest>().HasData(
                new ServiceRequest { ServiceRequestId = 1, ContractId = 1, Description = "Urgent shipment from NY to London", AmountUSD = 5000, AmountZAR = 95000, ExchangeRateUsed = 19.00m, Status = "Completed", RequestDate = DateTime.Now.AddDays(-10) },
                new ServiceRequest { ServiceRequestId = 2, ContractId = 1, Description = "Regular container shipping", AmountUSD = 2500, AmountZAR = 47500, ExchangeRateUsed = 19.00m, Status = "Pending", RequestDate = DateTime.Now.AddDays(-3) },
                new ServiceRequest { ServiceRequestId = 3, ContractId = 3, Description = "Express delivery - Electronics", AmountUSD = 12000, AmountZAR = 228000, ExchangeRateUsed = 19.00m, Status = "Pending", RequestDate = DateTime.Now.AddDays(-1) }
            );
        }
    }
}