﻿using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.ViewModels
{
    public class DriverContactChannelVm
    {
        public int Id { get; set; }

        [Required]
        public int DriverProfileId { get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(100)]
        public string? Title { get; set; }

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

    public class TractorContactChannelVm
    {
        public int Id { get; set; }

        [Required]
        public int TractorId { get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(100)]
        public string? Title { get; set; }

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

    public class LegalEntityContactChannelVm
    {
        public long Id { get; set; }

        [Required]
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
}
