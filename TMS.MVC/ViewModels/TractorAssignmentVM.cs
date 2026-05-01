using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    // ========== لیست کشنده‌ها ==========
    public class TractorAssignmentIndexViewModel
    {
        public long SubHavalehId { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? HavalehNumber { get; set; }
        public List<TractorAssignmentItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
        public string? StatusFilter { get; set; }
    }

    public class TractorAssignmentItemViewModel
    {
        public long Id { get; set; }
        public string PolicePlateNumber { get; set; } = "";
        public string? DriverName { get; set; }
        public AssignmentStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public DateTime AssignmentDate { get; set; }
        public decimal? LoadedAmount { get; set; }
        public decimal? UnloadedAmount { get; set; }
        public bool IsCompleted { get; set; }
    }

    // ========== ایجاد/ویرایش ==========
    public class TractorAssignmentUpsertViewModel
    {
        public long Id { get; set; }
        [Required] public long SubHavalehId { get; set; }
        [Display(Name = "عنوان زیرحواله")] public string? SubHavalehTitle { get; set; }
        [Display(Name = "شماره حواله")] public string? HavalehNumber { get; set; }
        [Required(ErrorMessage = "انتخاب کشنده الزامی است")] public int TractorId { get; set; }
        [Display(Name = "پلاک کشنده")] public string? TractorPlateNumber { get; set; }
        [Display(Name = "راننده")] public int? DriverProfileId { get; set; }
        [Display(Name = "نام راننده")] public string? DriverFullName { get; set; }
        [StringLength(500)] public string? Notes { get; set; }
    }

    // ========== جزئیات ==========
    public class TractorAssignmentDetailsViewModel
    {
        public TractorAssignment Assignment { get; set; } = null!;
        public long SubHavalehId { get; set; }
        public string SubHavalehTitle { get; set; } = "";
        public string HavalehNumber { get; set; } = "";
        public string TractorPlateNumber { get; set; } = "";
        public string? DriverFullName { get; set; }
        public string OriginCityDisplay { get; set; } = "";
        public string DestinationCityDisplay { get; set; } = "";

        public List<DocumentItemViewModel> LoadingDocuments { get; set; } = new();
        public List<DocumentItemViewModel> UnloadingDocuments { get; set; } = new();
        public List<LocationTrackingViewModel> LocationHistory { get; set; } = new();

        public decimal? ShortageAmount { get; set; }
        public decimal? ShortagePenalty { get; set; }
        public decimal? FinalFare { get; set; }

        public bool CanConfirmArrival { get; set; }
        public bool CanConfirmLoading { get; set; }
        public bool CanConfirmDestinationArrival { get; set; }
        public bool CanConfirmUnloading { get; set; }

        // پراپرتی‌های جدید برای لغو
        public bool CanCancelByAdmin { get; set; }
        public bool CanRequestCancellation { get; set; }
        public bool ShowCancellationActions { get; set; }
    }

    // ========== مدارک ==========
    public class DocumentItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string? DocumentType { get; set; }
        public string FilePath { get; set; } = "";
        public DateTime UploadDate { get; set; }
        public string? UploadedBy { get; set; }
        public bool IsApproved { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? RejectionNote { get; set; }
        public string? Notes { get; set; }
    }

    public class DocumentApprovalViewModel
    {
        public long DocumentId { get; set; }
        public bool IsApproved { get; set; }
        public string? Notes { get; set; }
        public string? RejectionNote { get; set; }
    }

     // ========== رهگیری موقعیت ==========
    public class LocationTrackingInputModel
    {
        public long TractorAssignmentId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public string? Notes { get; set; }
    }

    public class LocationTrackingViewModel
    {
        public long Id { get; set; }
        public long TractorAssignmentId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Notes { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public string FormattedLocation => $"{Latitude:F6}, {Longitude:F6}";
        public string FormattedSpeed => Speed.HasValue ? $"{Speed:F1} km/h" : "-";
        public string GoogleMapsUrl => $"https://www.google.com/maps?q={Latitude},{Longitude}";
        public string FormattedTimestamp => Timestamp.ToString("yyyy/MM/dd HH:mm");
    }

    public class LocationHistoryViewModel
    {
        public List<LocationTrackingViewModel> Locations { get; set; } = new();
        public long AssignmentId { get; set; }
        public string? AssignmentTitle { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int TotalLocations { get; set; }
    }

    // ========== جستجو ==========
    public class TractorSearchResultViewModel
    {
        public int Id { get; set; }
        public string PolicePlateNumber { get; set; } = "";
        public string? TractorType { get; set; }
        public decimal? Capacity { get; set; }
        public string? CapacityUnit { get; set; }
    }

    public class DriverSearchResultViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string? NationalId { get; set; }
        public string? PhoneNumber { get; set; }
    }

    // ========== چت ==========
    public class ChatViewModel
    {
        public long AssignmentId { get; set; }
        public string TractorPlateNumber { get; set; } = "";
        public string? DriverName { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; } = new();
    }

    public class ChatMessageViewModel
    {
        public long Id { get; set; }
        public string SenderName { get; set; } = "";
        public string SenderRole { get; set; } = "";
        public string? Message { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public string SentDateFormatted => SentDate.ToString("yyyy/MM/dd HH:mm");
        public bool IsImage =>
            FilePath != null && (
                FilePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                FilePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                FilePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                FilePath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
            );
    }

    public class SendMessageInputModel
    {
        public long AssignmentId { get; set; }
        public string? Message { get; set; }
        public IFormFile? File { get; set; }
    }
}