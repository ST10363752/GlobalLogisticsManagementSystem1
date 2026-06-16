using System.ComponentModel.DataAnnotations;

namespace GLMS.API.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ContactDetails { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Contract>? Contracts { get; set; }
    }
}