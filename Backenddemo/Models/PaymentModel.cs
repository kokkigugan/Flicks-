using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        public int RentalID { get; set; }

        [ForeignKey("RentalID")]
        public Rental? Rental { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal BaseAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal GSTAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime PaidOn { get; set; } = DateTime.Now;
    }
}
