using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TMS.MVC.Models.ViewModels
{
    public class CityFormVm
    {
        public long? Id { get; set; }

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
        public decimal? Latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public decimal? Longitude { get; set; }

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Provinces { get; set; } = new();

        public Dictionary<string, List<string>> CountryProvinceMap { get; set; } = new();
    }

    public class CityIndexVm
    {
        public List<City> Items { get; set; } = new();

        public string? Search { get; set; }
        public string? CountryFilter { get; set; }
        public string? ProvinceFilter { get; set; }

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Provinces { get; set; } = new();

        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

}