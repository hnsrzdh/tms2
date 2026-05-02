using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMS.MVC.Models;

namespace TMS.MVC.Models.ViewModels
{
    public class PlaceFormVm
    {
        public long? Id { get; set; }

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
        public decimal? Latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public decimal? Longitude { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Provinces { get; set; } = new();
        public List<SelectListItem> Cities { get; set; } = new();

        public Dictionary<string, List<string>> CountryProvinceMap { get; set; } = new();
        public Dictionary<string, List<string>> ProvinceCityMap { get; set; } = new();
    }

    public class PlaceIndexVm
    {
        public List<Place> Items { get; set; } = new();

        public string? Search { get; set; }
        public string? CountryFilter { get; set; }
        public string? ProvinceFilter { get; set; }
        public string? CityFilter { get; set; }

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Provinces { get; set; } = new();
        public List<SelectListItem> Cities { get; set; } = new();

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }
}