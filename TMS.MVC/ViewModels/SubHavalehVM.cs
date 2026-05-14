using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class SubHavalehIntermediatePlaceUpsertItemViewModel
    {
        public long? Id { get; set; }
        public long? PlaceId { get; set; }
        public string? PlaceDisplayName { get; set; }
        public int SortOrder { get; set; }
    }

    public class SubHavalehIndexItemViewModel
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? ContractType { get; set; }
        public string? TransportType { get; set; }
        public string? DestinationCityDisplayName { get; set; }
        public string? RouteSummary { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? RequestedCargoAmount { get; set; }
        public string? RequestedCargoAmountType { get; set; }
        public int ActiveAssignmentCount { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
        public decimal EffectiveUsedAmount { get; set; }
        public bool HasOpenAssignment { get; set; }
        public bool IsCompleted { get; set; }
        public bool CanEdit => ActiveAssignmentCount == 0;
        public bool CanDelete => ActiveAssignmentCount == 0;
        public string? LockReason => ActiveAssignmentCount > 0 ? "این ریزحواله دارای تخصیص لغونشده است و امکان ویرایش یا حذف آن وجود ندارد." : null;
        public decimal RemainingToAssign => (RequestedCargoAmount ?? 0) - EffectiveUsedAmount;
        public bool CanReleaseRemainingToHavaleh => RemainingToAssign > 0 && EffectiveUsedAmount > 0;
        public decimal RemainingToLoad => (RequestedCargoAmount ?? 0) - LoadedAmount;
        public decimal RemainingToUnload => (RequestedCargoAmount ?? 0) - UnloadedAmount;
        public decimal CompletionPercent
        {
            get
            {
                var requested = RequestedCargoAmount ?? 0;
                if (requested <= 0) return 0;
                var percent = (UnloadedAmount / requested) * 100;
                if (percent < 0) return 0;
                if (percent > 100) return 100;
                return percent;
            }
        }
    }

    public class SubHavalehUpsertViewModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "انتخاب حواله الزامی است.")]
        public long HavalehId { get; set; }

        [Display(Name = "شماره حواله")]
        public string? HavalehNumber { get; set; }

        [Display(Name = "مبدا")]
        public string? OriginPlaceDisplayName { get; set; }

        [Display(Name = "واحد حواله")]
        public string? HavalehUnit { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
        [StringLength(300)]
        [Display(Name = "عنوان")]
        public string? Title { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع قرارداد")]
        public string? ContractType { get; set; }

        [StringLength(200)]
        [Display(Name = "مبنای تسویه")]
        public string? SettlementBase { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع حمل")]
        public string? TransportType { get; set; }

        [Required(ErrorMessage = "انتخاب مقصد نهایی الزامی است.")]
        [Display(Name = "مقصد نهایی")]
        public long? DestinationPlaceId { get; set; }

        [Display(Name = "مقصد نهایی")]
        public string? DestinationPlaceDisplayName { get; set; }

        [Required(ErrorMessage = "فی صاحب کالا الزامی است.")]
        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ فی صاحب کالا باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "فی هر 1000 واحد - صاحب کالا")]
        public decimal? GoodsOwnerPricePer1000Unit { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? GoodsOwnerPriceCurrency { get; set; } = "ریال";

        [Required(ErrorMessage = "انعام صاحب کالا الزامی است.")]
        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ انعام صاحب کالا باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "انعام - صاحب کالا")]
        public decimal? GoodsOwnerTip { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? GoodsOwnerTipCurrency { get; set; } = "ریال";

        [Required(ErrorMessage = "حق توقف صاحب کالا الزامی است.")]
        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ حق توقف صاحب کالا باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "حق توقف ساعتی - صاحب کالا")]
        public decimal? GoodsOwnerStopFee { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? GoodsOwnerStopFeeCurrency { get; set; } = "ریال";

        [Required(ErrorMessage = "فی راننده الزامی است.")]
        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ فی راننده باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "فی هر 1000 واحد - راننده")]
        public decimal? DriverPricePer1000Unit { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? DriverPriceCurrency { get; set; } = "ریال";

        [Required(ErrorMessage = "انعام راننده الزامی است.")]
        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ انعام راننده باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "انعام - راننده")]
        public decimal? DriverTip { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? DriverTipCurrency { get; set; } = "ریال";

        [Required(ErrorMessage = "حق توقف راننده الزامی است.")]
        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ حق توقف راننده باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "حق توقف ساعتی - راننده")]
        public decimal? DriverStopFee { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? DriverStopFeeCurrency { get; set; } = "ریال";

        [Required(ErrorMessage = "زمان مجاز بارگیری الزامی است.")]
        [Display(Name = "زمان مجاز بارگیری")]
        public int? AllowedLoadingTime { get; set; }

        [Required(ErrorMessage = "زمان مجاز تخلیه الزامی است.")]
        [Display(Name = "زمان مجاز تحویل")]
        public int? AllowedDeliveryTime { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مبلغ جریمه تاخیر باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "جریمه روزانه تاخیر در تحویل")]
        public decimal? LateDeliveryPenalty { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? LateDeliveryPenaltyCurrency { get; set; } = "ریال";

        [StringLength(200)]
        [Display(Name = "نوع جریمه تاخیر")]
        public string? LateDeliveryPenaltyType { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع جریمه کسری بار")]
        public string? ShortagePenaltyType { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع کسری")]
        public string? ShortageType { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "مقدار کسری بار ثابت باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "کسری بار ثابت")]
        public decimal? FixedShortageAmount { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999999999999999", ErrorMessage = "کسر بار قابل قبول باید بزرگتر یا مساوی صفر باشد.")]
        [Display(Name = "کسر بار قابل قبول")]
        public decimal? AcceptableWeightLoss { get; set; }

        [Display(Name = "مشمول حق سرویس")]
        public bool IsUnderSupervisor { get; set; }

        [StringLength(200)]
        [Display(Name = "نحوه تعیین میزان محموله درخواستی")]
        public string? RequestedCargoAmountType { get; set; }

        [Required(ErrorMessage = "میزان محموله درخواستی الزامی است.")]
        [Range(typeof(decimal), "0.000001", "9999999999999999999999999999", ErrorMessage = "میزان محموله درخواستی باید بزرگتر از صفر باشد.")]
        [Display(Name = "میزان محموله درخواستی")]
        public decimal? RequestedCargoAmount { get; set; }

        [Display(Name = "مقدار باقیمانده حواله")]
        public decimal? RemainingHavalehAmountForSubHavaleh { get; set; }

        [Required(ErrorMessage = "تاریخ شروع الزامی است.")]
        [Display(Name = "تاریخ شروع")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "تاریخ پایان الزامی است.")]
        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }

        public List<SubHavalehIntermediatePlaceUpsertItemViewModel> IntermediatePlaces { get; set; } = new();
    }

}