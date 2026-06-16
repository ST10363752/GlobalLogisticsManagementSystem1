using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.API.Models
{
    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestId { get; set; }

        [Required]
        public int ContractId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountUSD { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountZAR { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExchangeRateUsed { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public DateTime RequestDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("ContractId")]
        public virtual Contract? Contract { get; set; }
    }
}