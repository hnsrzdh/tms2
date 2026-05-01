using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class LegalEntityIndexRowVm
    {
        public long Id { get; set; }
        public string CompanyName { get; set; } = "";
        public string? CompanyType { get; set; }
        public string? City { get; set; }
        public string? FirstAddress { get; set; }
    }

    public class LegalEntityIndexVm
    {
        public List<LegalEntityIndexRowVm> Items { get; set; } = new();

        public string? Search { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
    public class LegalEntityFormVm
    {
        public long? Id { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "نوع")]
        public string? CompanyType { get; set; }

        [StringLength(200)]
        [Display(Name = "شهر")]
        public string? City { get; set; }
    }

    public class LegalEntityDetailsVm
    {
        public LegalEntity Entity { get; set; } = new();
    }
}