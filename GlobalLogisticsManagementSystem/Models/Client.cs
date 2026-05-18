using System.ComponentModel.DataAnnotations;

namespace GlobalLogisticsManagementSystem.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ContactDetails { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Region { get; set; } = string.Empty;

        public virtual ICollection<Contract>? Contracts { get; set; }
    }
}