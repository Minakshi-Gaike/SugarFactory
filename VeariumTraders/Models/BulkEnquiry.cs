
using System.ComponentModel.DataAnnotations;

namespace VeariumTraders.Models
{
    public class BulkEnquiry
    {
        [Key]
        public int EnquiryId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? CompanyName { get; set; }

        [Required]
        [StringLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string? EmailAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0.01, 9999999999999999)]
        public decimal RequiredQuantity { get; set; }

        [Required]
        [StringLength(30)]
        public string QuantityUnit { get; set; } = string.Empty;

        [StringLength(200)]
        public string? DeliveryLocation { get; set; }

        public string? RequirementDetails { get; set; }

        public DateTime EnquiryDate { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
