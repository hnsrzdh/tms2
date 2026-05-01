using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models
{
    public class LegalEntity
    {
        public long Id { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "نوع")]
        public string? CompanyType { get; set; }

        [StringLength(200)]
        [Display(Name = "شهر")]
        public string? City { get; set; }

        public List<LegalEntityContact> Contacts { get; set; } = new();
        public List<LegalEntityAddress> Addresses { get; set; } = new();
        public List<LegalEntityBankAccount> BankAccounts { get; set; } = new();
    }

    public class LegalEntityAddress
    {
        public long Id { get; set; }

        [Required]
        public long LegalEntityId { get; set; }
        public LegalEntity? LegalEntity { get; set; }

        [StringLength(200)]
        [Display(Name = "عنوان")]
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "آدرس")]
        public string AddressText { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "کدپستی")]
        public string? PostalCode { get; set; }
    }
    public class LegalEntityBankAccount
    {
        public long Id { get; set; }

        [Required]
        public long LegalEntityId { get; set; }
        public LegalEntity? LegalEntity { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "شماره شبا")]
        public string Iban { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "نام صاحب حساب")]
        public string? AccountHolderName { get; set; }

        [StringLength(300)]
        [Display(Name = "نام استعلام شده")]
        public string? VerifiedName { get; set; }
    }
    public class LegalEntityContact
    {
        public long Id { get; set; }

        [Required]
        public long LegalEntityId { get; set; }
        public LegalEntity? LegalEntity { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
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

}