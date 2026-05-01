using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public class Tractor
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string PolicePlateNumber { get; set; } = "";

        [MaxLength(20)]
        public string? NationalId { get; set; }

        [MaxLength(50)]
        public string? TractorSmartCardNumber { get; set; }

        [MaxLength(50)]
        public string? FileNumber { get; set; }

        [MaxLength(100)]
        public string? Status { get; set; }

        [MaxLength(50)]
        public string? TractorIdentifier { get; set; }

        public decimal? MaxLoadCapacity { get; set; }
        [MaxLength(20)]
        public string? CapacityUnit { get; set; }
        public DateTime? TechnicalInspectionExpireDate { get; set; }
        public DateTime? ThirdPartyInsuranceExpireDate { get; set; }
       
        public decimal? WalletBalance { get; set; }

        public int? ProductionYear { get; set; }
        public int? AxleCount { get; set; }

        [MaxLength(100)]
        public string? SystemName { get; set; }

        [MaxLength(100)]
        public string? TractorType { get; set; }

        [MaxLength(50)]
        public string? TransitPlateNumber { get; set; }

        // مالک مستقیم
        public string? OwnerApplicationUserId { get; set; }
        public ApplicationUser? OwnerApplicationUser { get; set; }

        public ICollection<TractorBankAccount> BankAccounts { get; set; } = new List<TractorBankAccount>();
        public ICollection<TractorContact> Contacts { get; set; } = new List<TractorContact>();
        public ICollection<TractorAddress> Addresses { get; set; } = new List<TractorAddress>();
    }

    public class TractorBankAccount
    {
        public int Id { get; set; }

        public int TractorId { get; set; }
        public Tractor Tractor { get; set; } = null!;

        [MaxLength(200)]
        public string? AccountOwnerName { get; set; }

        [MaxLength(100)]
        public string? BankName { get; set; }

        [MaxLength(50)]
        public string? AccountNumber { get; set; }

        [MaxLength(50)]
        public string? CardNumber { get; set; }

        [MaxLength(50)]
        public string? ShebaNumber { get; set; }

        public bool IsDefault { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }

    public class TractorContact
    {
        public int Id { get; set; }

        public int TractorId { get; set; }
        public Tractor Tractor { get; set; } = null!;

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(200)]
        public string? ContactValue { get; set; }

        public bool HasSms { get; set; }
        public bool HasWhatsApp { get; set; }
        public bool IsFax { get; set; }
        public bool IsPhone { get; set; }
        public bool IsEmail { get; set; }
    }

    public class TractorAddress
    {
        public int Id { get; set; }

        public int TractorId { get; set; }
        public Tractor Tractor { get; set; } = null!;

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? AddressText { get; set; }
    }
}