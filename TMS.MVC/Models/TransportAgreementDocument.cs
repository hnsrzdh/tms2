using System.ComponentModel.DataAnnotations;

public class TransportAgreementDocument
{
    public int Id { get; set; }

    [Required]
    public int TransportAgreementId { get; set; }

    public TransportAgreement TransportAgreement { get; set; } = null!;

    [Required(ErrorMessage = "نام مدرک الزامی است")]
    [MaxLength(300)]
    [Display(Name = "نام مدرک")]
    public string DocumentName { get; set; } = "";

    [Required]
    [MaxLength(260)]
    public string OriginalFileName { get; set; } = "";

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = "";

    [MaxLength(120)]
    public string? ContentType { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
