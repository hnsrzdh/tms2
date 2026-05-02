using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class SubHavalehIntermediatePlaceUpsertItemViewModel
    {
        public long? Id { get; set; }

        [Display(Name = "مکان")]
        public long? PlaceId { get; set; }

        [Display(Name = "نمایش مکان")]
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
    }

    public class SubHavalehUpsertViewModel
    {
        public long Id { get; set; }

        [Required]
        public long HavalehId { get; set; }

        [Display(Name = "شماره حواله")]
        public string? HavalehNumber { get; set; }

        [Display(Name = "مبدا")]
        public string? OriginPlaceDisplayName { get; set; }

        [Display(Name = "عنوان")]
        public string? Title { get; set; }

        [Display(Name = "نوع قرارداد")]
        public string? ContractType { get; set; }

        [Display(Name = "مبنای تسویه")]
        public string? SettlementBase { get; set; }

        [Display(Name = "نوع حمل")]
        public string? TransportType { get; set; }

        [Display(Name = "مقصد نهایی")]
        public long? DestinationPlaceId { get; set; }

        [Display(Name = "نمایش مقصد نهایی")]
        public string? DestinationPlaceDisplayName { get; set; }

        [Display(Name = "نوع ارز راننده")]
        public string? DriverCurrencyType { get; set; }

        [Display(Name = "نرخ برابری ارز راننده")]
        public decimal? DriverCurrencyRate { get; set; }

        [Display(Name = "نوع ارز صاحب کالا")]
        public string? GoodsOwnerCurrencyType { get; set; }

        [Display(Name = "نرخ برابری ارز صاحب کالا")]
        public decimal? GoodsOwnerCurrencyRate { get; set; }

        [Display(Name = "فی هر تن - صاحب کالا")]
        public decimal? GoodsOwnerPricePerTon { get; set; }

        [Display(Name = "انعام - صاحب کالا")]
        public decimal? GoodsOwnerTip { get; set; }

        [Display(Name = "فی هر تن - راننده")]
        public decimal? DriverPricePerTon { get; set; }

        [Display(Name = "انعام - راننده")]
        public decimal? DriverTip { get; set; }

        [Display(Name = "فی حق توقف - صاحب کالا")]
        public decimal? GoodsOwnerStopFee { get; set; }

        [Display(Name = "فی حق توقف - راننده")]
        public decimal? DriverStopFee { get; set; }

        [Display(Name = "زمان مجاز بارگیری - روز")]
        public int? AllowedLoadingTime { get; set; }

        [Display(Name = "زمان مجاز تحویل - روز")]
        public int? AllowedDeliveryTime { get; set; }

        [Display(Name = "فی جریمه تاخیر در تحویل - روزانه")]
        public decimal? LateDeliveryPenalty { get; set; }

        [Display(Name = "نوع جریمه تاخیر در تحویل")]
        public string? LateDeliveryPenaltyType { get; set; }

        [Display(Name = "نوع جریمه کسری بار")]
        public string? ShortagePenaltyType { get; set; }

        [Display(Name = "نوع کسری")]
        public string? ShortageType { get; set; }

        [Display(Name = "فی جریمه کسری بار")]
        public decimal? FixedShortageAmount { get; set; }

        [Display(Name = "کسر بار قابل قبول")]
        public decimal? AcceptableWeightLoss { get; set; }

        [Display(Name = "فعال بودن حق سرویس")]
        public bool IsUnderSupervisor { get; set; }

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