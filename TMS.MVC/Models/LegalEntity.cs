﻿﻿using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models
{
    public class LegalEntity
    {
        public long Id { get; set; }

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

        public List<LegalEntityContact> Contacts { get; set; } = new();
        public List<LegalEntityAddress> Addresses { get; set; } = new();
        public List<LegalEntityBankAccount> BankAccounts { get; set; } = new();
    }

    public class LegalEntityAddress
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "انتخاب LegalEntityId الزامی است.")]
        public long LegalEntityId { get; set; }
        public LegalEntity? LegalEntity { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
        [StringLength(200)]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "وارد کردن آدرس الزامی است.")]
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

        [Required(ErrorMessage = "انتخاب LegalEntityId الزامی است.")]
        public long LegalEntityId { get; set; }
        public LegalEntity? LegalEntity { get; set; }

        // فیلد قدیمی برای سازگاری با کدهای قبلی نگه داشته شده است.
        [StringLength(100)]
        [Display(Name = "شماره شبا")]
        public string Iban { get; set; } = string.Empty;

        // فیلد قدیمی برای سازگاری با کدهای قبلی نگه داشته شده است.
        [StringLength(300)]
        [Display(Name = "نام صاحب حساب")]
        public string? AccountHolderName { get; set; }

        // فیلد قدیمی برای سازگاری با کدهای قبلی نگه داشته شده است.
        [StringLength(300)]
        [Display(Name = "نام استعلام شده")]
        public string? VerifiedName { get; set; }

        [Required(ErrorMessage = "نام صاحب حساب الزامی است.")]
        [StringLength(200)]
        [Display(Name = "نام صاحب حساب")]
        public string? AccountOwnerName { get; set; }

        [StringLength(100)]
        [Display(Name = "نام بانک")]
        public string? BankName { get; set; }

        [StringLength(50)]
        [Display(Name = "شماره حساب")]
        public string? AccountNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "شماره کارت")]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "شماره شبا الزامی است.")]
        [StringLength(50)]
        [Display(Name = "شماره شبا")]
        public string? ShebaNumber { get; set; }

        [Display(Name = "پیش فرض")]
        public bool IsDefault { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }

    public class LegalEntityContact
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "انتخاب LegalEntityId الزامی است.")]
        public long LegalEntityId { get; set; }
        public LegalEntity? LegalEntity { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
        [StringLength(200)]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = string.Empty;

        // فیلدهای قدیمی برای سازگاری با کدهای قبلی نگه داشته شده‌اند.
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

        [StringLength(200)]
        [Display(Name = "پیامک")]
        public string? SmsNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "واتساپ")]
        public string? WhatsAppNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "فکس")]
        public string? FaxNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "تلفن")]
        public string? PhoneNumber { get; set; }

        [StringLength(300)]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
        [Display(Name = "ایمیل")]
        public string? EmailAddress { get; set; }
    }
}
