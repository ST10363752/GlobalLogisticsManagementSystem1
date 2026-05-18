using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalLogisticsManagementSystem.Models
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Draft";

        [Required]
        [StringLength(100)]
        [Display(Name = "Service Level")]
        public string ServiceLevel { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Signed Agreement")]
        public string? SignedAgreementPath { get; set; }

        [Display(Name = "File Name")]
        public string? SignedAgreementFileName { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        public virtual ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}
