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

        public int SubHavalehCount { get; set; }
        public int ActiveAssignmentCount { get; set; }
        public decimal DefinedSubHavalehAmount { get; set; }
        public decimal RemainingSubHavalehAmount { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
        public bool AllSubHavalehsUnloaded { get; set; }
        public bool IsCompleted { get; set; }
        public bool CanEdit => SubHavalehCount == 0;
        public bool CanDelete => SubHavalehCount == 0;
        public string? LockReason => SubHavalehCount > 0 ? "این حواله دارای ریزحواله است و امکان ویرایش یا حذف آن وجود ندارد." : null;

        public decimal CompletionPercent
        {
            get
            {
                var total = ProductAmount ?? 0;
                if (total <= 0) return 0;
                var percent = (UnloadedAmount / total) * 100;
                if (percent < 0) return 0;
                if (percent > 100) return 100;
                return percent;
            }
        }
    }

    public class HavalehIndexViewModel
    {
        public List<HavalehIndexItemViewModel> Items { get; set; } = new();
        public List<HavalehIndexItemViewModel> CompletedItems { get; set; } = new();
        public string? Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int CompletedTotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class HavalehUpsertViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "وارد کردن شماره حواله الزامی است.")]
        [Display(Name = "شماره حواله")]
        public string HavalehNumber { get; set; } = "";

        [Required(ErrorMessage = "شماره قرارداد الزامی است.")]
        [Display(Name = "شماره قرارداد")]
        public string? ContractNumber { get; set; }

        [Display(Name = "نوع حواله")]
        public string? HavalehType { get; set; }

        [Display(Name = "نیازمند اخذ مجوز ورود ناوگان")]
        public bool RequiresFleetEntryPermit { get; set; }

        [Required(ErrorMessage = "انتخاب پیمانکار حمل الزامی است.")]
        [Display(Name = "پیمانکار حمل")]
        public long? TransportContractorLegalEntityId { get; set; }

        [Display(Name = "نمایش پیمانکار حمل")]
        public string? TransportContractorDisplayName { get; set; }

        [Required(ErrorMessage = "انتخاب صاحب کالا الزامی است.")]
        [Display(Name = "صاحب کالا")]
        public long? GoodsOwnerLegalEntityId { get; set; }

        [Display(Name = "نمایش صاحب کالا")]
        public string? GoodsOwnerDisplayName { get; set; }

        [Required(ErrorMessage = "انتخاب مبدا الزامی است.")]
        [Display(Name = "مبدا")]
        public long? OriginPlaceId { get; set; }

        [Display(Name = "نمایش مبدا")]
        public string? OriginPlaceDisplayName { get; set; }

        [Required(ErrorMessage = "انتخاب محصول الزامی است.")]
        [Display(Name = "محصول")]
        public long? ProductId { get; set; }

        [Display(Name = "نمایش محصول")]
        public string? ProductDisplayName { get; set; }

        [Required(ErrorMessage = "مقدار محصول الزامی است.")]
        [Range(typeof(decimal), "0.000001", "9999999999999999999999999999", ErrorMessage = "مقدار محصول باید بزرگتر از صفر باشد.")]
        [Display(Name = "مقدار محصول")]
        public decimal? ProductAmount { get; set; }

        [Required(ErrorMessage = "انتخاب واحد الزامی است.")]
        [Display(Name = "واحد")]
        public string? Unit { get; set; }

        [Display(Name = "تاریخ خرید")]
        public DateTime? PurchaseDate { get; set; }

        [Required(ErrorMessage = "تاریخ مجاز بارگیری الزامی است.")]
        [Display(Name = "تاریخ مجاز بارگیری")]
        public DateTime? AllowedLoadingDate { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ جریمه باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "جریمه به ازای هر واحد کسری بار")]
        public decimal? ShortagePenaltyPerUnit { get; set; }
    }
    public class HavalehDetailsViewModel
    {
        public Havaleh Entity { get; set; } = null!;
        public List<SubHavalehIndexItemViewModel> SubItems { get; set; } = new();
        public List<SubHavalehIndexItemViewModel> CompletedSubItems { get; set; } = new();
        public int SubPage { get; set; }
        public int SubPageSize { get; set; }
        public int SubTotalItems { get; set; }
        public int CompletedSubTotalItems { get; set; }
        public int SubTotalPages => (int)Math.Ceiling((double)SubTotalItems / SubPageSize);
        public int SubRowNumberStart => SubTotalItems == 0 ? 0 : ((SubPage - 1) * SubPageSize) + 1;

        public decimal TotalHavalehAmount { get; set; }
        public decimal TotalSubHavalehAmount { get; set; }
        public decimal RemainingForSubHavaleh => TotalHavalehAmount - TotalSubHavalehAmount;
        public decimal TotalAssignedAmount { get; set; }
        public decimal TotalLoadedAmount { get; set; }
        public decimal TotalUnloadedAmount { get; set; }
        public decimal RemainingToAssign => TotalHavalehAmount - TotalAssignedAmount;
        public decimal RemainingToLoad => TotalHavalehAmount - TotalLoadedAmount;
        public decimal RemainingToUnload => TotalHavalehAmount - TotalUnloadedAmount;
    }


}