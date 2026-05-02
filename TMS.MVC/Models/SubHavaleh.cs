using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public long? DestinationPlaceId { get; set; }
        public Place? DestinationPlace { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی هر 1000 واحد - صاحب کالا")]
        public decimal? GoodsOwnerPricePer1000Unit { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ فی صاحب کالا")]
        public string? GoodsOwnerPriceCurrency { get; set; } = "ریال";

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "انعام - صاحب کالا")]
        public decimal? GoodsOwnerTip { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد انعام صاحب کالا")]
        public string? GoodsOwnerTipCurrency { get; set; } = "ریال";

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "حق توقف ساعتی - صاحب کالا")]
        public decimal? GoodsOwnerStopFee { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد حق توقف صاحب کالا")]
        public string? GoodsOwnerStopFeeCurrency { get; set; } = "ریال";

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "فی هر 1000 واحد - راننده")]
        public decimal? DriverPricePer1000Unit { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد مبلغ فی راننده")]
        public string? DriverPriceCurrency { get; set; } = "ریال";

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "انعام - راننده")]
        public decimal? DriverTip { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد انعام راننده")]
        public string? DriverTipCurrency { get; set; } = "ریال";

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "حق توقف ساعتی - راننده")]
        public decimal? DriverStopFee { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد حق توقف راننده")]
        public string? DriverStopFeeCurrency { get; set; } = "ریال";

        [Display(Name = "زمان مجاز بارگیری")]
        public int? AllowedLoadingTime { get; set; }

        [Display(Name = "زمان مجاز تحویل")]
        public int? AllowedDeliveryTime { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "جریمه روزانه تاخیر در تحویل")]
        public decimal? LateDeliveryPenalty { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد جریمه تاخیر")]
        public string? LateDeliveryPenaltyCurrency { get; set; } = "ریال";

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

        [Display(Name = "تحت سوپروایزر")]
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

        public ICollection<SubHavalehIntermediatePlace> IntermediatePlaces { get; set; } = new List<SubHavalehIntermediatePlace>();
        public virtual ICollection<TractorAssignment> TractorAssignments { get; set; } = new List<TractorAssignment>();

        [NotMapped]
        public int AssignedTractorsCount => TractorAssignments?.Count ?? 0;

        [NotMapped]
        public decimal TotalLoadedAmount => TractorAssignments?.Sum(x => x.LoadedAmount) ?? 0;

        [NotMapped]
        public decimal TotalUnloadedAmount => TractorAssignments?.Sum(x => x.UnloadedAmount) ?? 0;
    }

    public class SubHavalehIntermediatePlace
    {
        public long Id { get; set; }

        [Required]
        public long SubHavalehId { get; set; }
        public SubHavaleh SubHavaleh { get; set; } = null!;

        [Required]
        public long PlaceId { get; set; }
        public Place Place { get; set; } = null!;

        public int SortOrder { get; set; }
    }
}
