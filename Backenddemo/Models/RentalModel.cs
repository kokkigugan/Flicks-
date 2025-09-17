using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Rentals")]
    public class Rental
    {
        [Key]
        public int RentalID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int MovieID { get; set; }

        [ForeignKey("MovieID")]
        public Movie? Movie { get; set; }  // Navigation property to access BluRayCost

        [Required]
        public DateTime RentalDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Rented";
    }
}
