using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public class City
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "کشور")]
        public string CountryName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "استان")]
        public string ProvinceName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "شهر")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "عرض جغرافیایی")]
        [Column(TypeName = "decimal(10,7)")]
        public decimal? Latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        [Column(TypeName = "decimal(10,7)")]
        public decimal? Longitude { get; set; }
    }
}