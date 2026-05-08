using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Models;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public partial class TractorAssignmentController
    {
        // This file extends the existing TractorAssignmentController class.
        // It is not a separate controller; the action remains under /TractorAssignment/.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmLoadingWithDocuments(
            long assignmentId,
            decimal loadedAmount,
            decimal? loadingNetVolume,
            string? loadingBillOfLadingNumber,
            List<IFormFile>? loadingDocuments)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
                return NotFound();

            if (assignment.Status != AssignmentStatus.ArrivedAtOrigin)
            {
                TempData["Error"] = "ثبت بارگیری فقط بعد از ثبت رسیدن به مبدا امکان‌پذیر است.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            if (loadedAmount <= 0)
            {
                TempData["Error"] = "وزن خالص بارگیری ضروری است و باید بزرگتر از صفر باشد.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            if (loadingNetVolume.HasValue && loadingNetVolume.Value < 0)
            {
                TempData["Error"] = "حجم خالص بارگیری نمی‌تواند منفی باشد.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            var validDocuments = (loadingDocuments ?? new List<IFormFile>())
                .Where(x => x != null && x.Length > 0)
                .ToList();

            if (!validDocuments.Any())
            {
                TempData["Error"] = "آپلود حداقل یک مدرک بارگیری الزامی است.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            var assignedAmount = assignment.AssignedCargoAmount ?? 0;
            if (assignedAmount > 0 && loadedAmount > assignedAmount)
            {
                TempData["Error"] = $"وزن خالص بارگیری نمی‌تواند بیشتر از مقدار تخصیص باشد. مقدار تخصیص: {assignedAmount:N3}";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            var subAmount = assignment.SubHavaleh.RequestedCargoAmount ?? 0;
            var currentLoadedTotal = await _context.TractorAssignments
                .Where(x => x.SubHavalehId == assignment.SubHavalehId &&
                            x.Id != assignment.Id &&
                            x.Status != AssignmentStatus.Cancelled)
                .SumAsync(x => (decimal?)x.LoadedAmount) ?? 0;

            if (currentLoadedTotal + loadedAmount > subAmount)
            {
                var remaining = subAmount - currentLoadedTotal;
                TempData["Error"] = $"مجموع بارگیری‌ها نباید بیشتر از مقدار زیرحواله باشد. باقیمانده قابل بارگیری: {remaining:N3} {assignment.SubHavaleh.Havaleh.Unit}";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            assignment.LoadingStartDate = DateTime.Now;
            assignment.LoadingEndDate = DateTime.Now;
            assignment.LoadedAmount = loadedAmount;
            assignment.LoadingNetVolume = loadingNetVolume;
            assignment.LoadingBillOfLadingNumber = string.IsNullOrWhiteSpace(loadingBillOfLadingNumber)
                ? null
                : loadingBillOfLadingNumber.Trim();
            assignment.Status = AssignmentStatus.Loaded;
            assignment.IsLoadingConfirmed = true;
            assignment.LoadingConfirmedBy = User.Identity?.Name;

            foreach (var document in validDocuments)
            {
                var filePath = await SaveFileAsync(document, "Documents");

                var title = Path.GetFileNameWithoutExtension(document.FileName);
                if (string.IsNullOrWhiteSpace(title))
                    title = document.FileName;

                assignment.LoadingDocuments.Add(new LoadingDocument
                {
                    TractorAssignmentId = assignmentId,
                    Title = title,
                    FilePath = filePath,
                    DocumentType = "Loading",
                    UploadDate = DateTime.Now,
                    UploadedBy = User.Identity?.Name
                });
            }

            await _context.SaveChangesAsync();

            TempData["Ok"] = "بارگیری و مدارک بارگیری با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }
    }
}
