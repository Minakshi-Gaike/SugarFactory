
using System.ComponentModel.DataAnnotations;

namespace VeariumTraders.Models
{
    public class ContactEnquiry
    {
        [Key]
        public int ContactEnquiryId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Mobile { get; set; } = string.Empty;

        [StringLength(150)]
        public string? CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceInquiry { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public DateTime EnquiryDate { get; set; } = DateTime.Now;

        [StringLength(30)]
        public string Status { get; set; } = "Pending";
    }
}
