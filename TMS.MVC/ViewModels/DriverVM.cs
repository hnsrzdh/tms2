using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.ViewModels
{
    public class DriverIndexItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string? NationalId { get; set; }
        public string? SmartCardNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class DriverIndexViewModel
    {
        public List<DriverIndexItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class DriverUpsertViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "کاربر راننده")]
        public string ApplicationUserId { get; set; } = "";

        [Display(Name = "نام راننده")]
        public string? UserDisplayName { get; set; }

        [Required(ErrorMessage = "کد ملی الزامی است")]
        [Display(Name = "کد ملی")]
        public string? NationalId { get; set; }

        [Display(Name = "شماره کارت هوشمند")]
        public string? SmartCardNumber { get; set; }

        [Display(Name = "شماره گواهینامه")]
        public string? DrivingLicenseNumber { get; set; }

        [Display(Name = "اعمال محدودیت فعالیت")]
        public bool IsBlocked { get; set; }

        [Display(Name = "توضیحات محدودیت")]
        public string? BlockDescription { get; set; }
    }

    public class DriverBankAccountUpsertViewModel
    {
        public int Id { get; set; }

        [Display(Name = "راننده")]
        public int DriverProfileId { get; set; }

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

        [Display(Name = "حساب پیش‌فرض")]
        public bool IsDefault { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }

    public class DriverContactUpsertViewModel
    {
        public int Id { get; set; }

        [Display(Name = "راننده")]
        public int DriverProfileId { get; set; }

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

    public class DriverAddressUpsertViewModel
    {
        public int Id { get; set; }

        [Display(Name = "راننده")]
        public int DriverProfileId { get; set; }

        [Display(Name = "عنوان")]
        public string? Title { get; set; }

        [Display(Name = "آدرس کامل")]
        public string? AddressText { get; set; }
    }
}