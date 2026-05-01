using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.ViewModels
{
    public class TractorIndexItemViewModel
    {
        public int Id { get; set; }
        public string PolicePlateNumber { get; set; } = "";
        public string? OwnerName { get; set; }
        public string? TractorSmartCardNumber { get; set; }
        public string? Status { get; set; }
        public decimal? MaxLoadCapacity { get; set; }
        public string? CapacityUnit { get; set; }
        public DateTime? TechnicalInspectionExpireDate { get; set; }
        public DateTime? ThirdPartyInsuranceExpireDate { get; set; }
    }
    public class TractorIndexViewModel
    {
        public List<TractorIndexItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class TractorUpsertViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "دو رقم اول پلاک")]
        public string PlatePartLeft2 { get; set; } = "";

        [Required]
        [Display(Name = "حرف پلاک")]
        public string PlateLetter { get; set; } = "";

        [Required]
        [Display(Name = "واحد ظرفیت")]
        public string? CapacityUnit { get; set; }

        [Required]
        [Display(Name = "سه رقم وسط پلاک")]
        public string PlatePartMiddle3 { get; set; } = "";

        [Required]
        [Display(Name = "دو رقم کد استان")]
        public string PlatePartRight2 { get; set; } = "";

        [Display(Name = "کاربر مالک")]
        public string? OwnerApplicationUserId { get; set; }

        [Display(Name = "نمایش مالک")]
        public string? OwnerUserDisplayName { get; set; }

        [Display(Name = "شناسه ملی")]
        public string? NationalId { get; set; }

        [Display(Name = "شماره هوشمند کشنده")]
        public string? TractorSmartCardNumber { get; set; }

        [Display(Name = "شماره پرونده")]
        public string? FileNumber { get; set; }

        [Display(Name = "وضعیت")]
        public string? Status { get; set; }

        [Display(Name = "شناسه کشنده")]
        public string? TractorIdentifier { get; set; }

        [Display(Name = "حداکثر ظرفیت قابل حمل")]
        public decimal? MaxLoadCapacity { get; set; }

        [Display(Name = "تاریخ اعتبار معاینه فنی")]
        [DataType(DataType.Date)]
        public DateTime? TechnicalInspectionExpireDate { get; set; }

        [Display(Name = "تاریخ اعتبار بیمه شخص ثالث")]
        [DataType(DataType.Date)]
        public DateTime? ThirdPartyInsuranceExpireDate { get; set; }

        [Display(Name = "سال تولید")]
        public int? ProductionYear { get; set; }

        [Display(Name = "تعداد محور")]
        public int? AxleCount { get; set; }

        [Display(Name = "سیستم")]
        public string? SystemName { get; set; }

        [Display(Name = "نوع کشنده")]
        public string? TractorType { get; set; }

        [Display(Name = "پلاک ترانزیت")]
        public string? TransitPlateNumber { get; set; }
    }

    public class TractorBankAccountUpsertViewModel
    {
        public int Id { get; set; }
        public int TractorId { get; set; }

        [Display(Name = "نام صاحب حساب")]
        public string? AccountOwnerName { get; set; }

        [Display(Name = "نام بانک")]
        public string? BankName { get; set; }

        [Display(Name = "شماره حساب")]
        public string? AccountNumber { get; set; }

        [Display(Name = "شماره کارت")]
        public string? CardNumber { get; set; }

        [Display(Name = "شماره شبا")]
        public string? ShebaNumber { get; set; }

        [Display(Name = "حساب پیش فرض")]
        public bool IsDefault { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }

    public class TractorContactUpsertViewModel
    {
        public int Id { get; set; }
        public int TractorId { get; set; }

        [Display(Name = "عنوان")]
        public string? Title { get; set; }

        [Display(Name = "راه ارتباطی")]
        public string? ContactValue { get; set; }

        [Display(Name = "پیامک")]
        public bool HasSms { get; set; }

        [Display(Name = "واتساپ")]
        public bool HasWhatsApp { get; set; }

        [Display(Name = "فکس")]
        public bool IsFax { get; set; }

        [Display(Name = "تلفن")]
        public bool IsPhone { get; set; }

        [Display(Name = "ایمیل")]
        public bool IsEmail { get; set; }
    }

    public class TractorAddressUpsertViewModel
    {
        public int Id { get; set; }
        public int TractorId { get; set; }
        public string? Title { get; set; }
        public string? AddressText { get; set; }
    }
}