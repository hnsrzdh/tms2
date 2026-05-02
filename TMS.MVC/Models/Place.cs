using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public class Place
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "نام کشور الزامی است")]
        [StringLength(200)]
        [Display(Name = "کشور")]
        public string CountryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام استان الزامی است")]
        [StringLength(200)]
        [Display(Name = "استان")]
        public string ProvinceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام شهر الزامی است")]
        [StringLength(200)]
        [Display(Name = "شهر")]
        public string CityName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام مکان الزامی است")]
        [StringLength(250)]
        [Display(Name = "نام مکان")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "آدرس / توضیحات")]
        public string? Address { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        [Column(TypeName = "decimal(10,7)")]
        public decimal? Latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        [Column(TypeName = "decimal(10,7)")]
        public decimal? Longitude { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;
    }
}