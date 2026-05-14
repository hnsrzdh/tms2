using System.ComponentModel.DataAnnotations;

public class TransportAgreement
{
    public int Id { get; set; }

    [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
    [MaxLength(300)]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "وارد کردن نام صاحب کالا الزامی است.")]
    [MaxLength(200)]
    public string CargoOwnerName { get; set; } = "";

    [Required(ErrorMessage = "وارد کردن مبدا الزامی است.")]
    [MaxLength(200)]
    public string Origin { get; set; } = "";

    [MaxLength(200)]
    public string? Destination { get; set; }

    [Required(ErrorMessage = "وارد کردن محصول الزامی است.")]
    [MaxLength(200)]
    public string ProductName { get; set; } = "";

    [Required(ErrorMessage = "وارد کردن مبلغ درخواست برداشت الزامی است.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "انتخاب واحد الزامی است.")]
    [MaxLength(20)]
    public string Unit { get; set; } = "";

    [MaxLength(100)]
    public string? Status { get; set; }

    public ICollection<TransportAgreementDocument> Documents { get; set; } = new List<TransportAgreementDocument>();
}
