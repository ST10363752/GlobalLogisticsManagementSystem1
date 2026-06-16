using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.API.Models
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Draft";

        [Required]
        public string ServiceLevel { get; set; } = string.Empty;

        public string? SignedAgreementPath { get; set; }

        public string? SignedAgreementFileName { get; set; }

        // Navigation property
        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        public virtual ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}