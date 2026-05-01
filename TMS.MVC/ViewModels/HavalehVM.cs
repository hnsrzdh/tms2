using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class HavalehIndexItemViewModel
    {
        public long Id { get; set; }
        public string HavalehNumber { get; set; } = "";
        public string? ContractNumber { get; set; }
        public string? HavalehType { get; set; }
        public bool RequiresFleetEntryPermit { get; set; }
        public string? TransportContractorName { get; set; }
        public string? GoodsOwnerName { get; set; }
        public string? OriginCityText { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductAmount { get; set; }
        public string? Unit { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? AllowedLoadingDate { get; set; }
        public decimal? ShortagePenaltyPerUnit { get; set; }
    }

    public class HavalehIndexViewModel
    {
        public List<HavalehIndexItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class HavalehUpsertViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "شماره حواله")]
        public string HavalehNumber { get; set; } = "";

        [Display(Name = "شماره قرارداد")]
        public string? ContractNumber { get; set; }

        [Display(Name = "نوع حواله")]
        public string? HavalehType { get; set; }

        [Display(Name = "نیازمند اخذ مجوز ورود ناوگان")]
        public bool RequiresFleetEntryPermit { get; set; }

        [Display(Name = "پیمانکار حمل")]
        public long? TransportContractorLegalEntityId { get; set; }

        [Display(Name = "نمایش پیمانکار حمل")]
        public string? TransportContractorDisplayName { get; set; }

        [Display(Name = "صاحب کالا")]
        public long? GoodsOwnerLegalEntityId { get; set; }

        [Display(Name = "نمایش صاحب کالا")]
        public string? GoodsOwnerDisplayName { get; set; }

        [Display(Name = "مبدا")]
        public long? OriginCityId { get; set; }

        [Display(Name = "نمایش مبدا")]
        public string? OriginCityDisplayName { get; set; }

        [Display(Name = "محصول")]
        public long? ProductId { get; set; }

        [Display(Name = "نمایش محصول")]
        public string? ProductDisplayName { get; set; }

        [Display(Name = "مقدار محصول")]
        public decimal? ProductAmount { get; set; }

        [Display(Name = "واحد")]
        public string? Unit { get; set; }

        [Display(Name = "تاریخ خرید")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "تاریخ مجاز بارگیری")]
        public DateTime? AllowedLoadingDate { get; set; }

        [Display(Name = "جریمه به ازای هر واحد کسری بار")]
        public decimal? ShortagePenaltyPerUnit { get; set; }
    }
    public class HavalehDetailsViewModel
    {
        public Havaleh Entity { get; set; } = null!;
        public List<SubHavalehIndexItemViewModel> SubItems { get; set; } = new();
        public int SubPage { get; set; }
        public int SubPageSize { get; set; }
        public int SubTotalItems { get; set; }
        public int SubTotalPages => (int)Math.Ceiling((double)SubTotalItems / SubPageSize);
        public int SubRowNumberStart => SubTotalItems == 0 ? 0 : ((SubPage - 1) * SubPageSize) + 1;
    }


}