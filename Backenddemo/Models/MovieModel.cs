using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BluRayCost { get; set; }

        public int Stock { get; set; }

        public int RentCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
