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
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
        public decimal RemainingToAssign => (RequestedCargoAmount ?? 0) - AssignedAmount;
        public decimal RemainingToLoad => (RequestedCargoAmount ?? 0) - LoadedAmount;
        public decimal RemainingToUnload => (RequestedCargoAmount ?? 0) - UnloadedAmount;
    }

    public class SubHavalehUpsertViewModel
    {
        public long? Id { get; set; }

        [Required]
        public long HavalehId { get; set; }

        [Display(Name = "شماره حواله")]
        public string? HavalehNumber { get; set; }

        [Display(Name = "مبدا")]
        public string? OriginPlaceDisplayName { get; set; }

        [Display(Name = "واحد حواله")]
        public string? HavalehUnit { get; set; }

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

        [Display(Name = "مقصد نهایی")]
        public long? DestinationPlaceId { get; set; }

        [Display(Name = "مقصد نهایی")]
        public string? DestinationPlaceDisplayName { get; set; }

        [Display(Name = "فی هر 1000 واحد - صاحب کالا")]
        public decimal? GoodsOwnerPricePer1000Unit { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? GoodsOwnerPriceCurrency { get; set; } = "ریال";

        [Display(Name = "انعام - صاحب کالا")]
        public decimal? GoodsOwnerTip { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? GoodsOwnerTipCurrency { get; set; } = "ریال";

        [Display(Name = "حق توقف ساعتی - صاحب کالا")]
        public decimal? GoodsOwnerStopFee { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? GoodsOwnerStopFeeCurrency { get; set; } = "ریال";

        [Display(Name = "فی هر 1000 واحد - راننده")]
        public decimal? DriverPricePer1000Unit { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? DriverPriceCurrency { get; set; } = "ریال";

        [Display(Name = "انعام - راننده")]
        public decimal? DriverTip { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? DriverTipCurrency { get; set; } = "ریال";

        [Display(Name = "حق توقف ساعتی - راننده")]
        public decimal? DriverStopFee { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ")]
        public string? DriverStopFeeCurrency { get; set; } = "ریال";

        [Display(Name = "زمان مجاز بارگیری")]
        public int? AllowedLoadingTime { get; set; }

        [Display(Name = "زمان مجاز تحویل")]
        public int? AllowedDeliveryTime { get; set; }

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

        [Display(Name = "کسری بار ثابت")]
        public decimal? FixedShortageAmount { get; set; }

        [Display(Name = "کسر بار قابل قبول")]
        public decimal? AcceptableWeightLoss { get; set; }

        [Display(Name = "مشمول حق سرویس")]
        public bool IsUnderSupervisor { get; set; }

        [StringLength(200)]
        [Display(Name = "نحوه تعیین میزان محموله درخواستی")]
        public string? RequestedCargoAmountType { get; set; }

        [Display(Name = "میزان محموله درخواستی")]
        public decimal? RequestedCargoAmount { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }

        public List<SubHavalehIntermediatePlaceUpsertItemViewModel> IntermediatePlaces { get; set; } = new();
    }

}