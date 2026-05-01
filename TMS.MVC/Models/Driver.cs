using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models
{
    public class DriverProfile
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [MaxLength(50)]
        public string? SmartCardNumber { get; set; }

        [MaxLength(50)]
        public string? DrivingLicenseNumber { get; set; }

        public bool IsBlocked { get; set; }

        [MaxLength(1000)]
        public string? BlockDescription { get; set; }
        public decimal? WalletBalance { get; set; }

        public ICollection<DriverBankAccount> BankAccounts { get; set; } = new List<DriverBankAccount>();
        public ICollection<DriverContact> Contacts { get; set; } = new List<DriverContact>();
        public ICollection<DriverAddress> Addresses { get; set; } = new List<DriverAddress>();
    }

    public class DriverBankAccount
    {
        public int Id { get; set; }

        public int DriverProfileId { get; set; }
        public DriverProfile DriverProfile { get; set; } = null!;

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
    public class DriverContact
    {
        public int Id { get; set; }

        public int DriverProfileId { get; set; }
        public DriverProfile DriverProfile { get; set; } = null!;

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
    public class DriverAddress
    {
        public int Id { get; set; }

        public int DriverProfileId { get; set; }
        public DriverProfile DriverProfile { get; set; } = null!;

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? AddressText { get; set; }
    }
}