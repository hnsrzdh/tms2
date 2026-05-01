using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.MVC.Models;

namespace TMS.MVC.Models
{

    public class SubHavaleh
    {
        public long Id { get; set; }

        [Required]
        public long HavalehId { get; set; }
        public Havaleh Havaleh { get; set; } = null!;

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
        public long? DestinationCityId { get; set; }
        public City? DestinationCity { get; set; }

        [StringLength(100)]
        [Display(Name = "نوع ارز راننده")]
        public string? DriverCurrencyType { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "نرخ برابری ارز راننده")]
        public decimal? DriverCurrencyRate { get; set; }

        [StringLength(100)]
        [Display(Name = "نوع ارز صاحب کالا")]
        public string? GoodsOwnerCurrencyType { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "نرخ برابری ارز صاحب کالا")]
        public decimal? GoodsOwnerCurrencyRate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی هر تن - صاحب کالا")]
        public decimal? GoodsOwnerPricePerTon { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "انعام - صاحب کالا")]
        public decimal? GoodsOwnerTip { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی هر تن - راننده")]
        public decimal? DriverPricePerTon { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "انعام - راننده")]
        public decimal? DriverTip { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی حق توقف - صاحب کالا")]
        public decimal? GoodsOwnerStopFee { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی حق توقف - راننده")]
        public decimal? DriverStopFee { get; set; }

        [Display(Name = "زمان مجاز بارگیری")]
        public int? AllowedLoadingTime { get; set; }

        [Display(Name = "زمان مجاز تحویل")]
        public int? AllowedDeliveryTime { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی جریمه تاخیر در تحویل")]
        public decimal? LateDeliveryPenalty { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع جریمه تاخیر در تحویل")]
        public string? LateDeliveryPenaltyType { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع جریمه کسری بار")]
        public string? ShortagePenaltyType { get; set; }

        [StringLength(200)]
        [Display(Name = "نوع کسری")]
        public string? ShortageType { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "کسری بار ثابت")]
        public decimal? FixedShortageAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "کسر وزن قابل قبول")]
        public decimal? AcceptableWeightLoss { get; set; }

        [Display(Name = "فعال بودن تحت سوپرویزر")]
        public bool IsUnderSupervisor { get; set; }

        [StringLength(200)]
        [Display(Name = "نحوه تعیین میزان محموله درخواستی")]
        public string? RequestedCargoAmountType { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "میزان محموله درخواستی")]
        public decimal? RequestedCargoAmount { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }

        public ICollection<SubHavalehIntermediateCity> IntermediateCities { get; set; } = new List<SubHavalehIntermediateCity>();

        // اضافه کردن رابطه با TractorAssignment
        public virtual ICollection<TractorAssignment> TractorAssignments { get; set; } = new List<TractorAssignment>();

        // تعداد کشنده‌های تخصیص داده شده
        [NotMapped]
        public int AssignedTractorsCount => TractorAssignments?.Count ?? 0;

        // مجموع بارگیری شده
        [NotMapped]
        public decimal TotalLoadedAmount => TractorAssignments?.Sum(x => x.LoadedAmount) ?? 0;

        // مجموع تخلیه شده
        [NotMapped]
        public decimal TotalUnloadedAmount => TractorAssignments?.Sum(x => x.UnloadedAmount) ?? 0;
    }

    public class SubHavalehIntermediateCity
    {
        public long Id { get; set; }

        [Required]
        public long SubHavalehId { get; set; }
        public SubHavaleh SubHavaleh { get; set; } = null!;

        [Required]
        public long CityId { get; set; }
        public City City { get; set; } = null!;

        public int SortOrder { get; set; }
    }
}