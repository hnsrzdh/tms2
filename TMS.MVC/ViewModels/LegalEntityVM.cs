using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class LegalEntityContactChannelVm
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "انتخاب LegalEntityId الزامی است.")]
        public long LegalEntityId { get; set; }

        [Required(ErrorMessage = "عنوان الزامی است.")]
        [Display(Name = "عنوان")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "پیامک")]
        [MaxLength(200)]
        public string? SmsNumber { get; set; }

        [Display(Name = "واتساپ")]
        [MaxLength(200)]
        public string? WhatsAppNumber { get; set; }

        [Display(Name = "فکس")]
        [MaxLength(200)]
        public string? FaxNumber { get; set; }

        [Display(Name = "تلفن")]
        [MaxLength(200)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(300)]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
        public string? EmailAddress { get; set; }
    }
    public class LegalEntityBankAccountVm
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "انتخاب LegalEntityId الزامی است.")]
        public long LegalEntityId { get; set; }

        [Required(ErrorMessage = "نام صاحب حساب الزامی است.")]
        [Display(Name = "نام صاحب حساب")]
        [MaxLength(200)]
        public string? AccountOwnerName { get; set; }

        [Display(Name = "نام بانک")]
        [MaxLength(100)]
        public string? BankName { get; set; }

        [Display(Name = "شماره حساب")]
        [MaxLength(50)]
        public string? AccountNumber { get; set; }

        [Display(Name = "شماره کارت")]
        [MaxLength(50)]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "شماره شبا الزامی است.")]
        [Display(Name = "شماره شبا")]
        [MaxLength(50)]
        public string? ShebaNumber { get; set; }

        [Display(Name = "پیش فرض")]
        public bool IsDefault { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000)]
        public string? Description { get; set; }
    }
    public class LegalEntityIndexRowVm
    {
        public long Id { get; set; }
        public string CompanyName { get; set; } = "";
        public string? CompanyType { get; set; }
        public string? City { get; set; }
        public string? FirstAddress { get; set; }
    }

    public class LegalEntityIndexVm
    {
        public List<LegalEntityIndexRowVm> Items { get; set; } = new();

        public string? Search { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
    public class LegalEntityFormVm
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "وارد کردن نام شرکت الزامی است.")]
        [StringLength(300)]
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "نوع")]
        public string? CompanyType { get; set; }

        [StringLength(200)]
        [Display(Name = "شهر")]
        public string? City { get; set; }
    }

    public class LegalEntityDetailsVm
    {
        public LegalEntity Entity { get; set; } = new();
    }
}