using DocumentFormat.OpenXml.Bibliography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.MVC.Models;

namespace TMS.MVC.Models
{
    public class Havaleh
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "شماره حواله")]
        public string HavalehNumber { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "شماره قرارداد")]
        public string? ContractNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "نوع حواله")]
        public string? HavalehType { get; set; }

        [Display(Name = "نیازمند اخذ مجوز ورود ناوگان")]
        public bool RequiresFleetEntryPermit { get; set; }

        [Display(Name = "پیمانکار حمل")]
        public long? TransportContractorLegalEntityId { get; set; }
        public LegalEntity? TransportContractorLegalEntity { get; set; }

        [Display(Name = "صاحب کالا")]
        public long? GoodsOwnerLegalEntityId { get; set; }
        public LegalEntity? GoodsOwnerLegalEntity { get; set; }

        [Display(Name = "مبدا")]
        public long? OriginPlaceId { get; set; }

        public Place? OriginPlace { get; set; }

        [Display(Name = "محصول")]
        public long? ProductId { get; set; }
        public Product? Product { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "مقدار محصول")]
        public decimal? ProductAmount { get; set; }

        [StringLength(20)]
        [Display(Name = "واحد")]
        public string? Unit { get; set; }

        [Display(Name = "تاریخ خرید")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "تاریخ مجاز بارگیری")]
        public DateTime? AllowedLoadingDate { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "فی کسری مقدار به ازای هر واحد")]
        public decimal? ShortagePenaltyPerUnit { get; set; }

        public ICollection<SubHavaleh> SubHavalehs { get; set; } = new List<SubHavaleh>();

    }
}