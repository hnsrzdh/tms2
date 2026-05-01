using System.ComponentModel.DataAnnotations;

public class TransportAgreement
{
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string CargoOwnerName { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string Origin { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string Destination { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = "";

    [Required]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Status { get; set; } = "";
}