using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public partial class TractorAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TractorAssignmentController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(long subHavalehId, string? search, string? statusFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var subHavaleh = await _context.SubHavalehs
                .Include(s => s.Havaleh)
                .FirstOrDefaultAsync(s => s.Id == subHavalehId);

            if (subHavaleh == null)
                return NotFound();

            var query = _context.TractorAssignments
                .Where(a => a.SubHavalehId == subHavalehId)
                .Include(a => a.Tractor)
                .Include(a => a.DriverProfile)
                    .ThenInclude(d => d.ApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<AssignmentStatus>(statusFilter, out var status))
            {
                query = query.Where(a => a.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(a =>
                    a.Tractor.PolicePlateNumber.Contains(search) ||
                    (
                        a.DriverProfile != null &&
                        (
                            a.DriverProfile.ApplicationUser.FirstName.Contains(search) ||
                            a.DriverProfile.ApplicationUser.LastName.Contains(search) ||
                            (a.DriverProfile.ApplicationUser.FirstName + " " + a.DriverProfile.ApplicationUser.LastName).Contains(search)
                        )
                    )
                );
            }

            var totalItems = await query.CountAsync();

            var assignments = await query
                .OrderByDescending(a => a.AssignmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = assignments.Select(a => new TractorAssignmentItemViewModel
            {
                Id = a.Id,
                PolicePlateNumber = a.Tractor.PolicePlateNumber,
                DriverName = a.DriverProfile != null
                    ? $"{a.DriverProfile.ApplicationUser.FirstName} {a.DriverProfile.ApplicationUser.LastName}"
                    : "-",
                Status = a.Status,
                StatusDisplay = GetStatusDisplay(a.Status),
                StatusBadgeClass = GetStatusBadgeClass(a.Status),
                AssignmentDate = a.AssignmentDate,
                LoadedAmount = a.LoadedAmount,
                UnloadedAmount = a.UnloadedAmount,
                IsCompleted = a.Status == AssignmentStatus.Completed || a.Status == AssignmentStatus.Unloaded
            }).ToList();

            var model = new TractorAssignmentIndexViewModel
            {
                SubHavalehId = subHavalehId,
                SubHavalehTitle = subHavaleh.Title,
                HavalehNumber = subHavaleh.Havaleh.HavalehNumber,
                Items = items,
                Search = search,
                StatusFilter = statusFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Assign(long subHavalehId)
        {
            var subHavaleh = await _context.SubHavalehs
                .Include(s => s.Havaleh)
                .FirstOrDefaultAsync(s => s.Id == subHavalehId);

            if (subHavaleh == null)
                return NotFound();

            var model = new TractorAssignmentUpsertViewModel
            {
                SubHavalehId = subHavalehId,
                SubHavalehTitle = subHavaleh.Title,
                HavalehNumber = subHavaleh.Havaleh.HavalehNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(TractorAssignmentUpsertViewModel model)
        {
            await FillAssignmentHeaderAsync(model);

            if (!ModelState.IsValid)
                return View(model);

            var tractor = await _context.Tractors.FindAsync(model.TractorId);
            if (tractor == null)
            {
                ModelState.AddModelError(nameof(model.TractorId), "کشنده انتخاب شده معتبر نیست.");
                return View(model);
            }

            var tractorInActiveAssignment = await _context.TractorAssignments
                .AnyAsync(a =>
                    a.TractorId == model.TractorId &&
                    a.Status != AssignmentStatus.Completed &&
                    a.Status != AssignmentStatus.Cancelled &&
                    a.Status != AssignmentStatus.Unloaded);

            if (tractorInActiveAssignment)
            {
                ModelState.AddModelError(nameof(model.TractorId), "این کشنده در حال حاضر در یک حمل فعال دیگر تخصیص داده شده است.");
                return View(model);
            }

            if (model.DriverProfileId.HasValue)
            {
                var driver = await _context.DriverProfiles.FindAsync(model.DriverProfileId);
                if (driver == null)
                {
                    ModelState.AddModelError(nameof(model.DriverProfileId), "راننده انتخاب شده معتبر نیست.");
                    return View(model);
                }

                var driverInActiveAssignment = await _context.TractorAssignments
                    .AnyAsync(a =>
                        a.DriverProfileId == model.DriverProfileId &&
                        a.Status != AssignmentStatus.Completed &&
                        a.Status != AssignmentStatus.Cancelled &&
                        a.Status != AssignmentStatus.Unloaded);

                if (driverInActiveAssignment)
                {
                    ModelState.AddModelError(nameof(model.DriverProfileId), "این راننده در حال حاضر در یک حمل فعال دیگر تخصیص داده شده است.");
                    return View(model);
                }
            }

            var assignment = new TractorAssignment
            {
                SubHavalehId = model.SubHavalehId,
                TractorId = model.TractorId,
                DriverProfileId = model.DriverProfileId,
                AssignmentDate = DateTime.Now,
                Notes = model.Notes,
                Status = AssignmentStatus.Assigned
            };

            _context.TractorAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "کشنده با موفقیت به زیرحواله تخصیص داده شد.";
            return RedirectToAction(nameof(Details), new { id = assignment.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                        .ThenInclude(h => h.OriginPlace)
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.DestinationPlace)
                .Include(a => a.Tractor)
                .Include(a => a.DriverProfile)
                    .ThenInclude(d => d.ApplicationUser)
                .Include(a => a.LoadingDocuments)
                .Include(a => a.UnloadingDocuments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
                return NotFound();

            var locationHistory = await _context.LocationTrackings
                .Where(l => l.TractorAssignmentId == id)
                .OrderByDescending(l => l.Timestamp)
                .Take(50)
                .ToListAsync();

            var model = new TractorAssignmentDetailsViewModel
            {
                Assignment = assignment,
                SubHavalehId = assignment.SubHavalehId,
                SubHavalehTitle = assignment.SubHavaleh.Title,
                HavalehNumber = assignment.SubHavaleh.Havaleh.HavalehNumber,
                TractorPlateNumber = assignment.Tractor.PolicePlateNumber,
                DriverFullName = assignment.DriverProfile?.ApplicationUser.FullName ?? "تعیین نشده",
                OriginCityDisplay = assignment.SubHavaleh.Havaleh.OriginPlace != null
                    ? assignment.SubHavaleh.Havaleh.OriginPlace.Name
                    : "نامشخص",
                DestinationCityDisplay = assignment.SubHavaleh.DestinationPlace != null
                    ? assignment.SubHavaleh.DestinationPlace.Name
                    : "نامشخص",

                LoadingDocuments = assignment.LoadingDocuments.Select(d => new DocumentItemViewModel
                {
                    Id = d.Id,
                    Title = d.Title,
                    DocumentType = d.DocumentType,
                    FilePath = d.FilePath,
                    UploadDate = d.UploadDate,
                    UploadedBy = d.UploadedBy,
                    IsApproved = d.IsApproved,
                    ApprovedBy = d.ApprovedBy,
                    ApprovalDate = d.ApprovalDate,
                    RejectionNote = d.RejectionNote
                }).ToList(),

                UnloadingDocuments = assignment.UnloadingDocuments.Select(d => new DocumentItemViewModel
                {
                    Id = d.Id,
                    Title = d.Title,
                    DocumentType = d.DocumentType,
                    FilePath = d.FilePath,
                    UploadDate = d.UploadDate,
                    UploadedBy = d.UploadedBy,
                    IsApproved = d.IsApproved,
                    ApprovedBy = d.ApprovedBy,
                    ApprovalDate = d.ApprovalDate,
                    RejectionNote = d.RejectionNote
                }).ToList(),

                LocationHistory = locationHistory.Select(l => new LocationTrackingViewModel
                {
                    Id = l.Id,
                    TractorAssignmentId = l.TractorAssignmentId,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Timestamp = l.Timestamp,
                    Notes = l.Notes,
                    Speed = l.Speed,
                    Heading = l.Heading
                }).ToList(),

                CanConfirmArrival = assignment.Status == AssignmentStatus.Assigned,
                CanConfirmLoading = assignment.Status == AssignmentStatus.ArrivedAtOrigin,
                CanConfirmDestinationArrival = assignment.Status == AssignmentStatus.Loaded,
                CanConfirmUnloading = assignment.Status == AssignmentStatus.ArrivedAtDestination,

                CanCancelByAdmin =
                    assignment.Status == AssignmentStatus.Assigned ||
                    assignment.Status == AssignmentStatus.ArrivedAtOrigin ||
                    assignment.Status == AssignmentStatus.CancellationRequested,

                CanRequestCancellation =
                    assignment.Status == AssignmentStatus.Assigned ||
                    assignment.Status == AssignmentStatus.ArrivedAtOrigin,

                ShowCancellationActions = assignment.Status == AssignmentStatus.CancellationRequested
            };

            ApplyFinancialCalculationToDetailsModel(assignment, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmArrivalAtOrigin(long assignmentId)
        {
            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            assignment.ArrivalAtOriginDate = DateTime.Now;
            assignment.Status = AssignmentStatus.ArrivedAtOrigin;
            assignment.IsArrivalAtOriginConfirmed = true;
            assignment.ArrivalAtOriginConfirmedBy = User.Identity?.Name;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "رسیدن به مبدا ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmLoading(long assignmentId, decimal loadedAmount)
        {
            var assignment = await _context.TractorAssignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return NotFound();

            assignment.LoadingStartDate = DateTime.Now;
            assignment.LoadingEndDate = DateTime.Now;
            assignment.LoadedAmount = loadedAmount;
            assignment.Status = AssignmentStatus.Loaded;
            assignment.IsLoadingConfirmed = true;
            assignment.LoadingConfirmedBy = User.Identity?.Name;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "بارگیری با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmArrivalAtDestination(long assignmentId)
        {
            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            assignment.ArrivalAtDestinationDate = DateTime.Now;
            assignment.Status = AssignmentStatus.ArrivedAtDestination;
            assignment.IsArrivalAtDestinationConfirmed = true;
            assignment.ArrivalAtDestinationConfirmedBy = User.Identity?.Name;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "رسیدن به مقصد ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUnloading(long assignmentId, decimal unloadedAmount)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return NotFound();

            assignment.UnloadingStartDate = DateTime.Now;
            assignment.UnloadingEndDate = DateTime.Now;
            assignment.UnloadedAmount = unloadedAmount;
            assignment.Status = AssignmentStatus.Unloaded;
            assignment.IsUnloadingConfirmed = true;
            assignment.UnloadingConfirmedBy = User.Identity?.Name;

            if (assignment.LoadedAmount.HasValue)
                assignment.ShortageAmount = assignment.LoadedAmount.Value - unloadedAmount;

            ApplyFinancialCalculationToAssignment(assignment);

            await _context.SaveChangesAsync();

            TempData["Ok"] = "تخلیه با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestCancellation(long assignmentId, string reason)
        {
            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            if (assignment.Status != AssignmentStatus.Assigned &&
                assignment.Status != AssignmentStatus.ArrivedAtOrigin)
            {
                TempData["Error"] = "فقط قبل از بارگیری امکان درخواست لغو وجود دارد.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            assignment.Status = AssignmentStatus.CancellationRequested;
            assignment.IsCancellationRequestedByDriver = true;
            assignment.CancellationRequestDate = DateTime.Now;
            assignment.CancellationReason = reason;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست لغو با موفقیت ثبت شد. منتظر تایید ادمین باشید.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAssignment(long assignmentId, string reason)
        {
            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            if (assignment.Status != AssignmentStatus.Assigned &&
                assignment.Status != AssignmentStatus.ArrivedAtOrigin &&
                assignment.Status != AssignmentStatus.CancellationRequested)
            {
                TempData["Error"] = "فقط قبل از بارگیری امکان لغو وجود دارد.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            assignment.Status = AssignmentStatus.Cancelled;
            assignment.CancellationDate = DateTime.Now;
            assignment.CancelledBy = User.Identity?.Name;
            assignment.CancellationReason = reason;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "حمل با موفقیت لغو شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveCancellation(long assignmentId)
        {
            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            if (assignment.Status != AssignmentStatus.CancellationRequested)
            {
                TempData["Error"] = "این حمل در وضعیت درخواست لغو نیست.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            assignment.Status = AssignmentStatus.Cancelled;
            assignment.CancellationDate = DateTime.Now;
            assignment.CancelledBy = User.Identity?.Name;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست لغو تایید و سفر کنسل شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCancellation(long assignmentId, string reason)
        {
            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment == null) return NotFound();

            if (assignment.Status != AssignmentStatus.CancellationRequested)
            {
                TempData["Error"] = "این حمل در وضعیت درخواست لغو نیست.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            assignment.Status = assignment.ArrivalAtOriginDate.HasValue
                ? AssignmentStatus.ArrivedAtOrigin
                : AssignmentStatus.Assigned;

            assignment.IsCancellationRequestedByDriver = false;
            assignment.CancellationRequestDate = null;
            assignment.CancellationReason = null;

            if (!string.IsNullOrWhiteSpace(reason))
                assignment.CancellationReason = $"رد شد - {reason}";

            await _context.SaveChangesAsync();

            TempData["Warning"] = "درخواست لغو رد شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveDocument(long documentId, string documentType)
        {
            long? assignmentId = null;

            if (documentType == "Loading")
            {
                var doc = await _context.LoadingDocuments.FindAsync(documentId);
                if (doc != null)
                {
                    doc.IsApproved = true;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                    assignmentId = doc.TractorAssignmentId;
                }
            }
            else if (documentType == "Unloading")
            {
                var doc = await _context.UnloadingDocuments.FindAsync(documentId);
                if (doc != null)
                {
                    doc.IsApproved = true;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                    assignmentId = doc.TractorAssignmentId;
                }
            }

            await _context.SaveChangesAsync();

            if (assignmentId.HasValue)
                await CheckAndCompleteAssignment(assignmentId.Value);

            TempData["Ok"] = "مدرک تایید شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectDocument(long documentId, string documentType, string rejectionNote)
        {
            long? assignmentId = null;

            if (documentType == "Loading")
            {
                var doc = await _context.LoadingDocuments.FindAsync(documentId);
                if (doc != null)
                {
                    doc.IsApproved = false;
                    doc.RejectionNote = rejectionNote;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                    assignmentId = doc.TractorAssignmentId;
                }
            }
            else if (documentType == "Unloading")
            {
                var doc = await _context.UnloadingDocuments.FindAsync(documentId);
                if (doc != null)
                {
                    doc.IsApproved = false;
                    doc.RejectionNote = rejectionNote;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                    assignmentId = doc.TractorAssignmentId;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Warning"] = "مدرک رد شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDocument(long documentId, string documentType)
        {
            long? assignmentId = null;

            if (documentType == "Loading")
            {
                var doc = await _context.LoadingDocuments.FindAsync(documentId);
                if (doc != null)
                {
                    assignmentId = doc.TractorAssignmentId;
                    _context.LoadingDocuments.Remove(doc);
                }
            }
            else if (documentType == "Unloading")
            {
                var doc = await _context.UnloadingDocuments.FindAsync(documentId);
                if (doc != null)
                {
                    assignmentId = doc.TractorAssignmentId;
                    _context.UnloadingDocuments.Remove(doc);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Ok"] = "مدرک حذف شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAllDocuments(long assignmentId, string documentType)
        {
            if (documentType == "Loading")
            {
                var docs = await _context.LoadingDocuments
                    .Where(d => d.TractorAssignmentId == assignmentId)
                    .ToListAsync();

                foreach (var doc in docs)
                {
                    doc.IsApproved = true;
                    doc.RejectionNote = null;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                }
            }
            else if (documentType == "Unloading")
            {
                var docs = await _context.UnloadingDocuments
                    .Where(d => d.TractorAssignmentId == assignmentId)
                    .ToListAsync();

                foreach (var doc in docs)
                {
                    doc.IsApproved = true;
                    doc.RejectionNote = null;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
            await CheckAndCompleteAssignment(assignmentId);

            TempData["Ok"] = "همه مدارک تایید شدند.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectAllDocuments(long assignmentId, string documentType, string rejectionNote)
        {
            if (documentType == "Loading")
            {
                var docs = await _context.LoadingDocuments
                    .Where(d => d.TractorAssignmentId == assignmentId)
                    .ToListAsync();

                foreach (var doc in docs)
                {
                    doc.IsApproved = false;
                    doc.RejectionNote = rejectionNote;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                }
            }
            else if (documentType == "Unloading")
            {
                var docs = await _context.UnloadingDocuments
                    .Where(d => d.TractorAssignmentId == assignmentId)
                    .ToListAsync();

                foreach (var doc in docs)
                {
                    doc.IsApproved = false;
                    doc.RejectionNote = rejectionNote;
                    doc.ApprovedBy = User.Identity?.Name;
                    doc.ApprovalDate = DateTime.Now;
                }
            }

            var assignment = await _context.TractorAssignments.FindAsync(assignmentId);
            if (assignment != null && assignment.Status == AssignmentStatus.Completed)
                assignment.Status = AssignmentStatus.Unloaded;

            await _context.SaveChangesAsync();

            TempData["Warning"] = "همه مدارک رد شدند.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(long assignmentId, string documentType, IFormFile document, string? title)
        {
            if (document == null || document.Length == 0)
            {
                TempData["Error"] = "فایلی انتخاب نشده است.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            var filePath = await SaveFileAsync(document, "Documents");

            if (documentType == "Loading")
            {
                var loadingDoc = new LoadingDocument
                {
                    TractorAssignmentId = assignmentId,
                    Title = string.IsNullOrWhiteSpace(title) ? document.FileName : title.Trim(),
                    FilePath = filePath,
                    DocumentType = documentType,
                    UploadDate = DateTime.Now,
                    UploadedBy = User.Identity?.Name
                };

                _context.LoadingDocuments.Add(loadingDoc);
            }
            else if (documentType == "Unloading")
            {
                var unloadingDoc = new UnloadingDocument
                {
                    TractorAssignmentId = assignmentId,
                    Title = string.IsNullOrWhiteSpace(title) ? document.FileName : title.Trim(),
                    FilePath = filePath,
                    DocumentType = documentType,
                    UploadDate = DateTime.Now,
                    UploadedBy = User.Identity?.Name
                };

                _context.UnloadingDocuments.Add(unloadingDoc);
            }

            await _context.SaveChangesAsync();

            TempData["Ok"] = "مدرک با موفقیت آپلود شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SettleToDriver(long assignmentId)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.DriverProfile)
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return NotFound();

            if (assignment.IsSettled)
                return RedirectToAction(nameof(Details), new { id = assignmentId });

            if (!assignment.IsFinancialApproved || !assignment.FinancialApprovedAmount.HasValue)
            {
                TempData["Error"] = "قبل از واریز، مبلغ نهایی باید توسط اپراتور تایید شود.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            var totalFare = assignment.FinancialApprovedAmount.Value;

            if (assignment.DriverProfile != null)
                assignment.DriverProfile.WalletBalance = (assignment.DriverProfile.WalletBalance ?? 0) + totalFare;

            assignment.FinalFare = totalFare;
            assignment.PayableAmount = totalFare;
            assignment.IsSettled = true;
            assignment.SettledTo = "Driver";
            assignment.SettledBy = User.Identity?.Name;
            assignment.SettledDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Ok"] = $"مبلغ {totalFare:N0} به کیف پول راننده واریز شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SettleToTractor(long assignmentId)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.Tractor)
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return NotFound();

            if (assignment.IsSettled)
                return RedirectToAction(nameof(Details), new { id = assignmentId });

            if (!assignment.IsFinancialApproved || !assignment.FinancialApprovedAmount.HasValue)
            {
                TempData["Error"] = "قبل از واریز، مبلغ نهایی باید توسط اپراتور تایید شود.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            var totalFare = assignment.FinancialApprovedAmount.Value;

            if (assignment.Tractor != null)
                assignment.Tractor.WalletBalance = (assignment.Tractor.WalletBalance ?? 0) + totalFare;

            assignment.FinalFare = totalFare;
            assignment.PayableAmount = totalFare;
            assignment.IsSettled = true;
            assignment.SettledTo = "Tractor";
            assignment.SettledBy = User.Identity?.Name;
            assignment.SettledDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Ok"] = $"مبلغ {totalFare:N0} به کیف پول کشنده واریز شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationTrackingInputModel model)
        {
            var location = new LocationTracking
            {
                TractorAssignmentId = model.TractorAssignmentId,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Timestamp = DateTime.Now,
                Speed = model.Speed,
                Heading = model.Heading,
                Notes = model.Notes
            };

            _context.LocationTrackings.Add(location);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> SearchAvailableTractors(string? term)
        {
            var busyTractorIds = await _context.TractorAssignments
                .Where(a =>
                    a.Status != AssignmentStatus.Completed &&
                    a.Status != AssignmentStatus.Cancelled &&
                    a.Status != AssignmentStatus.Unloaded)
                .Select(a => a.TractorId)
                .Distinct()
                .ToListAsync();

            var query = _context.Tractors
                .Where(t => t.Status == "فعال" && !busyTractorIds.Contains(t.Id))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term) && term.Trim().Length >= 2)
            {
                term = term.Trim();
                query = query.Where(t => t.PolicePlateNumber.Contains(term));
            }

            var tractors = await query
                .OrderBy(t => t.PolicePlateNumber)
                .Select(t => new TractorSearchResultViewModel
                {
                    Id = t.Id,
                    PolicePlateNumber = t.PolicePlateNumber,
                    TractorType = t.TractorType,
                    Capacity = t.MaxLoadCapacity,
                    CapacityUnit = t.CapacityUnit
                })
                .Take(20)
                .ToListAsync();

            return Json(tractors);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAvailableDrivers(string? term)
        {
            var busyDriverIds = await _context.TractorAssignments
                .Where(a =>
                    a.Status != AssignmentStatus.Completed &&
                    a.Status != AssignmentStatus.Cancelled &&
                    a.Status != AssignmentStatus.Unloaded &&
                    a.DriverProfileId != null)
                .Select(a => a.DriverProfileId!.Value)
                .Distinct()
                .ToListAsync();

            var query = _context.DriverProfiles
                .Include(d => d.ApplicationUser)
                .Where(d => !d.IsBlocked && !busyDriverIds.Contains(d.Id))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term) && term.Trim().Length >= 2)
            {
                term = term.Trim();

                query = query.Where(d =>
                    d.ApplicationUser.FirstName.Contains(term) ||
                    d.ApplicationUser.LastName.Contains(term) ||
                    (d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName).Contains(term));
            }

            var drivers = await query
                .OrderBy(d => d.ApplicationUser.LastName)
                .ThenBy(d => d.ApplicationUser.FirstName)
                .Select(d => new DriverSearchResultViewModel
                {
                    Id = d.Id,
                    FullName = d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName,
                    NationalId = d.ApplicationUser.NationalId,
                    PhoneNumber = d.ApplicationUser.PhoneNumber
                })
                .Take(20)
                .ToListAsync();

            return Json(drivers);
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationHistory(long assignmentId, int count = 50)
        {
            var locations = await _context.LocationTrackings
                .Where(l => l.TractorAssignmentId == assignmentId)
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .Select(l => new LocationTrackingViewModel
                {
                    Id = l.Id,
                    TractorAssignmentId = l.TractorAssignmentId,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Timestamp = l.Timestamp,
                    Speed = l.Speed,
                    Heading = l.Heading,
                    Notes = l.Notes
                })
                .ToListAsync();

            return Json(locations);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignmentsList(long subHavalehId, string? search, string? statusFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var subHavaleh = await _context.SubHavalehs
                .Include(s => s.Havaleh)
                .FirstOrDefaultAsync(s => s.Id == subHavalehId);

            if (subHavaleh == null)
                return NotFound();

            var query = _context.TractorAssignments
                .Where(a => a.SubHavalehId == subHavalehId)
                .Include(a => a.Tractor)
                .Include(a => a.DriverProfile)
                    .ThenInclude(d => d.ApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(a =>
                    a.Tractor.PolicePlateNumber.Contains(search) ||
                    (
                        a.DriverProfile != null &&
                        (
                            a.DriverProfile.ApplicationUser.FirstName.Contains(search) ||
                            a.DriverProfile.ApplicationUser.LastName.Contains(search) ||
                            (a.DriverProfile.ApplicationUser.FirstName + " " + a.DriverProfile.ApplicationUser.LastName).Contains(search)
                        )
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<AssignmentStatus>(statusFilter, out var status))
            {
                query = query.Where(a => a.Status == status);
            }

            var totalItems = await query.CountAsync();

            var assignments = await query
                .OrderByDescending(a => a.AssignmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = assignments.Select(a => new TractorAssignmentItemViewModel
            {
                Id = a.Id,
                PolicePlateNumber = a.Tractor.PolicePlateNumber,
                DriverName = a.DriverProfile != null
                    ? $"{a.DriverProfile.ApplicationUser.FirstName} {a.DriverProfile.ApplicationUser.LastName}"
                    : "-",
                Status = a.Status,
                StatusDisplay = GetStatusDisplay(a.Status),
                StatusBadgeClass = GetStatusBadgeClass(a.Status),
                AssignmentDate = a.AssignmentDate,
                LoadedAmount = a.LoadedAmount,
                UnloadedAmount = a.UnloadedAmount,
                IsCompleted = a.Status == AssignmentStatus.Completed || a.Status == AssignmentStatus.Unloaded
            }).ToList();

            var model = new TractorAssignmentIndexViewModel
            {
                SubHavalehId = subHavalehId,
                SubHavalehTitle = subHavaleh.Title,
                HavalehNumber = subHavaleh.Havaleh.HavalehNumber,
                Items = items,
                Search = search,
                StatusFilter = statusFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return PartialView("_TractorAssignmentsList", model);
        }

        private async Task FillAssignmentHeaderAsync(TractorAssignmentUpsertViewModel model)
        {
            var subHavaleh = await _context.SubHavalehs
                .Include(s => s.Havaleh)
                .FirstOrDefaultAsync(s => s.Id == model.SubHavalehId);

            if (subHavaleh == null) return;

            model.SubHavalehTitle = subHavaleh.Title;
            model.HavalehNumber = subHavaleh.Havaleh.HavalehNumber;
        }

        private void ApplyFinancialCalculationToDetailsModel(TractorAssignment assignment, TractorAssignmentDetailsViewModel model)
        {
            if (assignment.LoadedAmount.HasValue && assignment.UnloadedAmount.HasValue)
            {
                model.ShortageAmount = assignment.LoadedAmount.Value - assignment.UnloadedAmount.Value;

                if (model.ShortageAmount > 0)
                {
                    var penaltyPerUnit = assignment.SubHavaleh.Havaleh.ShortagePenaltyPerUnit ?? 0;
                    model.ShortagePenalty = model.ShortageAmount * penaltyPerUnit;
                }
            }

            model.FinalFare = CalculateDriverPayableAmount(assignment);
        }

        private void ApplyFinancialCalculationToAssignment(TractorAssignment assignment)
        {
            if (assignment.LoadedAmount.HasValue && assignment.UnloadedAmount.HasValue)
            {
                var shortageAmount = assignment.LoadedAmount.Value - assignment.UnloadedAmount.Value;
                assignment.ShortageAmount = shortageAmount;

                if (shortageAmount > 0)
                {
                    var penaltyPerUnit = assignment.SubHavaleh.Havaleh.ShortagePenaltyPerUnit ?? 0;
                    assignment.ShortagePenalty = shortageAmount * penaltyPerUnit;
                }
                else
                {
                    assignment.ShortagePenalty = 0;
                }
            }

            assignment.FinalFare = CalculateDriverPayableAmount(assignment);
            assignment.PayableAmount = assignment.FinalFare;
        }

        private decimal CalculateDriverPayableAmount(TractorAssignment assignment)
        {
            var sub = assignment.SubHavaleh;
            if (sub == null) return 0;

            var unloadedAmount = assignment.UnloadedAmount ?? 0;
            var pricePer1000Unit = sub.DriverPricePer1000Unit ?? 0;
            var baseFare = (unloadedAmount / 1000m) * pricePer1000Unit;

            var driverTip = sub.DriverTip ?? 0;

            var stopHours = CalculateDriverStopHours(assignment);
            var stopFeeRatePerHour = sub.DriverStopFee ?? 0;
            var stopFee = stopHours * stopFeeRatePerHour;

            var shortagePenalty = assignment.ShortagePenalty ?? 0;
            var delayPenalty = assignment.DelayPenalty ?? 0;

            var total = baseFare + driverTip + stopFee - shortagePenalty - delayPenalty;

            return total < 0 ? 0 : total;
        }

        private decimal CalculateDriverStopHours(TractorAssignment assignment)
        {
            var sub = assignment.SubHavaleh;
            if (sub == null) return 0;

            decimal totalStopHours = 0;

            if (assignment.ArrivalAtOriginDate.HasValue && assignment.LoadingEndDate.HasValue && sub.AllowedLoadingTime.HasValue)
            {
                var loadingHours = (decimal)(assignment.LoadingEndDate.Value - assignment.ArrivalAtOriginDate.Value).TotalHours;
                var allowedLoadingHours = sub.AllowedLoadingTime.Value;

                if (loadingHours > allowedLoadingHours)
                    totalStopHours += Math.Ceiling(loadingHours - allowedLoadingHours);
            }

            if (assignment.ArrivalAtDestinationDate.HasValue && assignment.UnloadingEndDate.HasValue && sub.AllowedDeliveryTime.HasValue)
            {
                var unloadingHours = (decimal)(assignment.UnloadingEndDate.Value - assignment.ArrivalAtDestinationDate.Value).TotalHours;
                var allowedDeliveryHours = sub.AllowedDeliveryTime.Value;

                if (unloadingHours > allowedDeliveryHours)
                    totalStopHours += Math.Ceiling(unloadingHours - allowedDeliveryHours);
            }

            return totalStopHours;
        }

        private async Task CheckAndCompleteAssignment(long assignmentId)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.LoadingDocuments)
                .Include(a => a.UnloadingDocuments)
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return;

            if (assignment.Status != AssignmentStatus.Unloaded) return;

            var allLoadingApproved = assignment.LoadingDocuments.Any() &&
                                     assignment.LoadingDocuments.All(d => d.IsApproved);

            var allUnloadingApproved = assignment.UnloadingDocuments.Any() &&
                                       assignment.UnloadingDocuments.All(d => d.IsApproved);

            if (allLoadingApproved && allUnloadingApproved)
            {
                ApplyFinancialCalculationToAssignment(assignment);
                assignment.Status = AssignmentStatus.Completed;
                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var safeFileName = Path.GetFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid():N}_{safeFileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{uniqueFileName}";
        }

        private string GetStatusDisplay(AssignmentStatus status) => status switch
        {
            AssignmentStatus.Assigned => "تخصیص داده شده",
            AssignmentStatus.ArrivedAtOrigin => "رسیده به مبدا",
            AssignmentStatus.Loading => "در حال بارگیری",
            AssignmentStatus.Loaded => "بارگیری شده",
            AssignmentStatus.InTransit => "در راه",
            AssignmentStatus.ArrivedAtDestination => "رسیده به مقصد",
            AssignmentStatus.Unloading => "در حال تخلیه",
            AssignmentStatus.Unloaded => "تخلیه شده",
            AssignmentStatus.Completed => "تکمیل شده",
            AssignmentStatus.CancellationRequested => "درخواست لغو",
            AssignmentStatus.Cancelled => "لغو شده",
            _ => "نامشخص"
        };

        private string GetStatusBadgeClass(AssignmentStatus status) => status switch
        {
            AssignmentStatus.Assigned => "bg-secondary",
            AssignmentStatus.ArrivedAtOrigin => "bg-info text-dark",
            AssignmentStatus.Loading => "bg-info text-dark",
            AssignmentStatus.Loaded => "bg-primary",
            AssignmentStatus.InTransit => "bg-primary",
            AssignmentStatus.ArrivedAtDestination => "bg-success",
            AssignmentStatus.Unloading => "bg-warning text-dark",
            AssignmentStatus.Unloaded => "bg-warning text-dark",
            AssignmentStatus.Completed => "bg-success",
            AssignmentStatus.CancellationRequested => "bg-warning text-dark",
            AssignmentStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }
}