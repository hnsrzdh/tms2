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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveFinancialCalculation(
            long assignmentId,
            string financialBaseCurrency,
            decimal? driverPriceExchangeRateToBase,
            decimal? driverTipExchangeRateToBase,
            decimal? driverStopFeeExchangeRateToBase,
            decimal? delayPenaltyExchangeRateToBase,
            decimal? shortagePenaltyExchangeRateToBase,
            bool useManualAmount,
            decimal? manualAmount,
            string? financialAdjustmentNote)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.SubHavaleh)
                    .ThenInclude(s => s.Havaleh)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
                return NotFound();

            if (!assignment.UnloadedAmount.HasValue)
            {
                TempData["Error"] = "تا قبل از ثبت تخلیه، تایید مالی امکان‌پذیر نیست.";
                return RedirectToAction(nameof(Details), new { id = assignmentId });
            }

            financialBaseCurrency = string.IsNullOrWhiteSpace(financialBaseCurrency)
                ? (assignment.SubHavaleh.DriverPriceCurrency ?? "ریال")
                : financialBaseCurrency.Trim();

            assignment.FinancialBaseCurrency = financialBaseCurrency;
            assignment.DriverPriceExchangeRateToBase = NormalizeRate(assignment.SubHavaleh.DriverPriceCurrency, financialBaseCurrency, driverPriceExchangeRateToBase);
            assignment.DriverTipExchangeRateToBase = NormalizeRate(assignment.SubHavaleh.DriverTipCurrency, financialBaseCurrency, driverTipExchangeRateToBase);
            assignment.DriverStopFeeExchangeRateToBase = NormalizeRate(assignment.SubHavaleh.DriverStopFeeCurrency, financialBaseCurrency, driverStopFeeExchangeRateToBase);
            assignment.DelayPenaltyExchangeRateToBase = NormalizeRate(assignment.SubHavaleh.LateDeliveryPenaltyCurrency, financialBaseCurrency, delayPenaltyExchangeRateToBase);
            assignment.ShortagePenaltyExchangeRateToBase = NormalizeRate("ریال", financialBaseCurrency, shortagePenaltyExchangeRateToBase);

            var result = CalculateAssignmentFinancials(assignment);

            assignment.ShortageAmount = result.RawShortageAmount;
            assignment.ChargeableShortageAmount = result.ChargeableShortageAmount;
            assignment.ShortagePenalty = result.ShortagePenalty;
            assignment.LoadingDelayDays = result.LoadingDelayDays;
            assignment.DeliveryDelayDays = result.DeliveryDelayDays;
            assignment.TotalDelayDays = result.TotalDelayDays;
            assignment.DelayPenalty = result.DelayPenalty;
            assignment.FinancialCalculatedAmount = result.TotalFare;
            assignment.FinalFare = result.TotalFare;
            assignment.PayableAmount = result.TotalFare;

            if (useManualAmount)
            {
                if (!manualAmount.HasValue || manualAmount.Value < 0)
                {
                    TempData["Error"] = "برای تایید دستی، مبلغ نهایی معتبر وارد کنید.";
                    return RedirectToAction(nameof(Details), new { id = assignmentId });
                }

                if (string.IsNullOrWhiteSpace(financialAdjustmentNote))
                {
                    TempData["Error"] = "برای تغییر دستی مبلغ نهایی، توضیح اپراتور الزامی است.";
                    return RedirectToAction(nameof(Details), new { id = assignmentId });
                }

                assignment.FinancialManualAmount = manualAmount.Value;
                assignment.FinancialApprovedAmount = manualAmount.Value;
                assignment.FinancialAdjustmentNote = financialAdjustmentNote.Trim();
            }
            else
            {
                assignment.FinancialManualAmount = null;
                assignment.FinancialApprovedAmount = result.TotalFare;
                assignment.FinancialAdjustmentNote = null;
            }

            assignment.IsFinancialApproved = true;
            assignment.FinancialApprovedBy = User.Identity?.Name;
            assignment.FinancialApprovedDate = DateTime.Now;
            assignment.PayableAmount = assignment.FinancialApprovedAmount;
            assignment.FinalFare = assignment.FinancialApprovedAmount;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "محاسبات مالی و مبلغ نهایی با موفقیت تایید شد.";
            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }

        private decimal NormalizeRate(string? itemCurrency, string baseCurrency, decimal? inputRate)
        {
            if (string.Equals(itemCurrency ?? baseCurrency, baseCurrency, StringComparison.OrdinalIgnoreCase))
                return 1m;

            return inputRate.HasValue && inputRate.Value > 0 ? inputRate.Value : 0m;
        }

        private FinancialCalculationResult CalculateAssignmentFinancials(TractorAssignment assignment)
        {
            var sub = assignment.SubHavaleh;
            var havaleh = sub.Havaleh;
            var baseCurrency = assignment.FinancialBaseCurrency ?? sub.DriverPriceCurrency ?? "ریال";

            decimal Rate(string? currency, decimal? savedRate)
            {
                if (string.Equals(currency ?? baseCurrency, baseCurrency, StringComparison.OrdinalIgnoreCase))
                    return 1m;

                return savedRate ?? 0m;
            }

            var loadedAmount = assignment.LoadedAmount ?? 0m;
            var unloadedAmount = assignment.UnloadedAmount ?? 0m;

            var rawShortageAmount = Math.Max(0m, loadedAmount - unloadedAmount);
            var allowedShortage = sub.AcceptableWeightLoss ?? 0m;
            var chargeableShortageAmount = Math.Max(0m, rawShortageAmount - allowedShortage);

            var loadingDelayDays = CalculateDelayDays(assignment.AssignmentDate, assignment.LoadingEndDate, sub.AllowedLoadingTime);
            var deliveryDelayDays = CalculateDelayDays(assignment.AssignmentDate, assignment.UnloadingEndDate, sub.AllowedDeliveryTime);
            var totalDelayDays = loadingDelayDays + deliveryDelayDays;

            var baseFare = (unloadedAmount / 1000m) * (sub.DriverPricePer1000Unit ?? 0m) * Rate(sub.DriverPriceCurrency, assignment.DriverPriceExchangeRateToBase);
            var driverTip = (sub.DriverTip ?? 0m) * Rate(sub.DriverTipCurrency, assignment.DriverTipExchangeRateToBase);
            var stopFee = CalculateDriverStopHours(assignment) * (sub.DriverStopFee ?? 0m) * Rate(sub.DriverStopFeeCurrency, assignment.DriverStopFeeExchangeRateToBase);
            var shortagePenalty = chargeableShortageAmount * (havaleh.ShortagePenaltyPerUnit ?? 0m) * Rate("ریال", assignment.ShortagePenaltyExchangeRateToBase);
            var delayPenalty = totalDelayDays * (sub.LateDeliveryPenalty ?? 0m) * Rate(sub.LateDeliveryPenaltyCurrency, assignment.DelayPenaltyExchangeRateToBase);

            var totalFare = baseFare + driverTip + stopFee - shortagePenalty - delayPenalty;
            if (totalFare < 0) totalFare = 0;

            return new FinancialCalculationResult
            {
                RawShortageAmount = rawShortageAmount,
                ChargeableShortageAmount = chargeableShortageAmount,
                LoadingDelayDays = loadingDelayDays,
                DeliveryDelayDays = deliveryDelayDays,
                TotalDelayDays = totalDelayDays,
                ShortagePenalty = shortagePenalty,
                DelayPenalty = delayPenalty,
                TotalFare = totalFare
            };
        }

        private int CalculateDelayDays(DateTime assignmentDate, DateTime? actualDate, int? allowedDays)
        {
            if (!actualDate.HasValue || !allowedDays.HasValue) return 0;

            var actualDays = (actualDate.Value.Date - assignmentDate.Date).Days;
            var delayDays = actualDays - allowedDays.Value;

            return delayDays > 0 ? delayDays : 0;
        }

        private sealed class FinancialCalculationResult
        {
            public decimal RawShortageAmount { get; set; }
            public decimal ChargeableShortageAmount { get; set; }
            public int LoadingDelayDays { get; set; }
            public int DeliveryDelayDays { get; set; }
            public int TotalDelayDays { get; set; }
            public decimal ShortagePenalty { get; set; }
            public decimal DelayPenalty { get; set; }
            public decimal TotalFare { get; set; }
        }
    }
}
