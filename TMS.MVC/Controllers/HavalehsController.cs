using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    public class HavalehsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HavalehsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            search = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

            var query = _context.Havalehs
                .Include(x => x.TransportContractorLegalEntity)
                .Include(x => x.GoodsOwnerLegalEntity)
                .Include(x => x.OriginPlace)
                .Include(x => x.Product)
                .Include(x => x.SubHavalehs)
                    .ThenInclude(x => x.TractorAssignments)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.HavalehNumber.Contains(search) ||
                    (x.ContractNumber ?? "").Contains(search) ||
                    (x.HavalehType ?? "").Contains(search) ||
                    (x.TransportContractorLegalEntity != null ? x.TransportContractorLegalEntity.CompanyName : "").Contains(search) ||
                    (x.GoodsOwnerLegalEntity != null ? x.GoodsOwnerLegalEntity.CompanyName : "").Contains(search) ||
                    (x.OriginPlace != null ? x.OriginPlace.Name : "").Contains(search) ||
                    (x.Product != null ? x.Product.Name : "").Contains(search));
            }

            var entities = await query
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var allItems = entities
                .Select(BuildHavalehIndexItem)
                .ToList();

            var activeItems = allItems
                .Where(x => !x.IsCompleted)
                .ToList();

            var completedItems = allItems
                .Where(x => x.IsCompleted)
                .ToList();

            var totalItems = activeItems.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages > 0 && page > totalPages) page = totalPages;

            var items = activeItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(new HavalehIndexViewModel
            {
                Items = items,
                CompletedItems = completedItems,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                CompletedTotalItems = completedItems.Count
            });
        }

        private static HavalehIndexItemViewModel BuildHavalehIndexItem(Havaleh x)
        {
            var subHavalehs = x.SubHavalehs?.ToList() ?? new List<SubHavaleh>();
            var activeAssignments = subHavalehs
                .SelectMany(s => s.TractorAssignments ?? new List<TractorAssignment>())
                .Where(a => a.Status != AssignmentStatus.Cancelled)
                .ToList();

            var totalAmount = x.ProductAmount ?? 0;
            var definedSubAmount = subHavalehs.Sum(s => s.RequestedCargoAmount ?? 0);
            var assignedAmount = activeAssignments.Sum(a => a.AssignedCargoAmount ?? 0);
            var loadedAmount = activeAssignments.Sum(a => a.LoadedAmount ?? 0);
            var unloadedAmount = activeAssignments.Sum(a => a.UnloadedAmount ?? 0);
            var remainingSubAmount = totalAmount - definedSubAmount;

            var allSubHavalehsUnloaded = subHavalehs.Any() && subHavalehs.All(sub =>
            {
                var requested = sub.RequestedCargoAmount ?? 0;
                var unloaded = (sub.TractorAssignments ?? new List<TractorAssignment>())
                    .Where(a => a.Status != AssignmentStatus.Cancelled)
                    .Sum(a => a.UnloadedAmount ?? 0);

                var hasOpenAssignment = (sub.TractorAssignments ?? new List<TractorAssignment>())
                    .Any(a => a.Status != AssignmentStatus.Cancelled &&
                              a.Status != AssignmentStatus.Unloaded &&
                              a.Status != AssignmentStatus.Completed);

                return requested > 0 && unloaded >= requested && !hasOpenAssignment;
            });

            var isFullyDefined = totalAmount > 0 && definedSubAmount >= totalAmount;
            var isCompleted = isFullyDefined && allSubHavalehsUnloaded && unloadedAmount >= totalAmount;

            return new HavalehIndexItemViewModel
            {
                Id = x.Id,
                HavalehNumber = x.HavalehNumber,
                ContractNumber = x.ContractNumber,
                HavalehType = x.HavalehType,
                RequiresFleetEntryPermit = x.RequiresFleetEntryPermit,
                TransportContractorName = x.TransportContractorLegalEntity?.CompanyName,
                GoodsOwnerName = x.GoodsOwnerLegalEntity?.CompanyName,
                OriginCityText = x.OriginPlace?.Name,
                ProductName = x.Product?.Name,
                ProductAmount = x.ProductAmount,
                Unit = x.Unit,
                PurchaseDate = x.PurchaseDate,
                AllowedLoadingDate = x.AllowedLoadingDate,
                ShortagePenaltyPerUnit = x.ShortagePenaltyPerUnit,
                SubHavalehCount = subHavalehs.Count,
                DefinedSubHavalehAmount = definedSubAmount,
                RemainingSubHavalehAmount = remainingSubAmount,
                AssignedAmount = assignedAmount,
                LoadedAmount = loadedAmount,
                UnloadedAmount = unloadedAmount,
                ActiveAssignmentCount = activeAssignments.Count,
                AllSubHavalehsUnloaded = allSubHavalehsUnloaded,
                IsCompleted = isCompleted
            };
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new HavalehUpsertViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HavalehUpsertViewModel vm)
        {
            NormalizeHavalehVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = new Havaleh
            {
                HavalehNumber = vm.HavalehNumber,
                ContractNumber = vm.ContractNumber,
                HavalehType = vm.HavalehType,
                RequiresFleetEntryPermit = vm.RequiresFleetEntryPermit,
                TransportContractorLegalEntityId = vm.TransportContractorLegalEntityId,
                GoodsOwnerLegalEntityId = vm.GoodsOwnerLegalEntityId,
                OriginPlaceId = vm.OriginPlaceId,
                ProductId = vm.ProductId,
                ProductAmount = vm.ProductAmount,
                Unit = vm.Unit,
                PurchaseDate = vm.PurchaseDate,
                AllowedLoadingDate = vm.AllowedLoadingDate,
                ShortagePenaltyPerUnit = vm.ShortagePenaltyPerUnit
            };

            _context.Havalehs.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "حواله با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _context.Havalehs
                .Include(x => x.TransportContractorLegalEntity)
                .Include(x => x.GoodsOwnerLegalEntity)
                .Include(x => x.OriginPlace)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            if (await HasAnySubHavalehAsync(entity.Id))
            {
                TempData["Warning"] = "این حواله دارای ریزحواله است و امکان ویرایش آن وجود ندارد.";
                return RedirectToAction(nameof(Details), new { id = entity.Id });
            }

            return View(new HavalehUpsertViewModel
            {
                Id = entity.Id,
                HavalehNumber = entity.HavalehNumber,
                ContractNumber = entity.ContractNumber,
                HavalehType = entity.HavalehType,
                RequiresFleetEntryPermit = entity.RequiresFleetEntryPermit,
                TransportContractorLegalEntityId = entity.TransportContractorLegalEntityId,
                TransportContractorDisplayName = entity.TransportContractorLegalEntity?.CompanyName,
                GoodsOwnerLegalEntityId = entity.GoodsOwnerLegalEntityId,
                GoodsOwnerDisplayName = entity.GoodsOwnerLegalEntity?.CompanyName,
                OriginPlaceId = entity.OriginPlaceId,
                OriginPlaceDisplayName = entity.OriginPlace?.Name,
                ProductId = entity.ProductId,
                ProductDisplayName = entity.Product == null ? null : entity.Product.Name + (string.IsNullOrWhiteSpace(entity.Product.Type) ? "" : $" - {entity.Product.Type}"),
                ProductAmount = entity.ProductAmount,
                Unit = entity.Unit,
                PurchaseDate = entity.PurchaseDate,
                AllowedLoadingDate = entity.AllowedLoadingDate,
                ShortagePenaltyPerUnit = entity.ShortagePenaltyPerUnit
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HavalehUpsertViewModel vm)
        {
            NormalizeHavalehVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = await _context.Havalehs.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (entity == null)
                return NotFound();

            if (await HasAnySubHavalehAsync(entity.Id))
            {
                TempData["Warning"] = "این حواله دارای ریزحواله است و امکان ویرایش آن وجود ندارد.";
                return RedirectToAction(nameof(Details), new { id = entity.Id });
            }

            entity.HavalehNumber = vm.HavalehNumber;
            entity.ContractNumber = vm.ContractNumber;
            entity.HavalehType = vm.HavalehType;
            entity.RequiresFleetEntryPermit = vm.RequiresFleetEntryPermit;
            entity.TransportContractorLegalEntityId = vm.TransportContractorLegalEntityId;
            entity.GoodsOwnerLegalEntityId = vm.GoodsOwnerLegalEntityId;
            entity.OriginPlaceId = vm.OriginPlaceId;
            entity.ProductId = vm.ProductId;
            entity.ProductAmount = vm.ProductAmount;
            entity.Unit = vm.Unit;
            entity.PurchaseDate = vm.PurchaseDate;
            entity.AllowedLoadingDate = vm.AllowedLoadingDate;
            entity.ShortagePenaltyPerUnit = vm.ShortagePenaltyPerUnit;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "حواله با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id, int subPage = 1, int subPageSize = 10)
        {
            if (subPage < 1) subPage = 1;
            if (subPageSize <= 0) subPageSize = 10;

            var entity = await _context.Havalehs
                .Include(x => x.TransportContractorLegalEntity)
                .Include(x => x.GoodsOwnerLegalEntity)
                .Include(x => x.OriginPlace)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            var subEntities = await _context.SubHavalehs
                .Where(x => x.HavalehId == id)
                .Include(x => x.DestinationPlace)
                .Include(x => x.IntermediatePlaces)
                    .ThenInclude(x => x.Place)
                .Include(x => x.TractorAssignments)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var allSubItems = subEntities
                .Select(BuildSubHavalehIndexItem)
                .ToList();

            var activeSubItems = allSubItems
                .Where(x => !x.IsCompleted)
                .ToList();

            var completedSubItems = allSubItems
                .Where(x => x.IsCompleted)
                .ToList();

            var subTotalItems = activeSubItems.Count;
            var subTotalPages = (int)Math.Ceiling((double)subTotalItems / subPageSize);
            if (subTotalPages > 0 && subPage > subTotalPages) subPage = subTotalPages;

            var subItems = activeSubItems
                .Skip((subPage - 1) * subPageSize)
                .Take(subPageSize)
                .ToList();

            var totalHavalehAmount = entity.ProductAmount ?? 0;
            var totalSubAmount = allSubItems.Sum(x => x.RequestedCargoAmount ?? 0);
            var activeAssignments = subEntities
                .SelectMany(x => x.TractorAssignments ?? new List<TractorAssignment>())
                .Where(x => x.Status != AssignmentStatus.Cancelled)
                .ToList();

            return View(new HavalehDetailsViewModel
            {
                Entity = entity,
                SubItems = subItems,
                CompletedSubItems = completedSubItems,
                SubPage = subPage,
                SubPageSize = subPageSize,
                SubTotalItems = subTotalItems,
                CompletedSubTotalItems = completedSubItems.Count,
                TotalHavalehAmount = totalHavalehAmount,
                TotalSubHavalehAmount = totalSubAmount,
                TotalAssignedAmount = activeAssignments.Sum(x => x.AssignedCargoAmount ?? 0),
                TotalLoadedAmount = activeAssignments.Sum(x => x.LoadedAmount ?? 0),
                TotalUnloadedAmount = activeAssignments.Sum(x => x.UnloadedAmount ?? 0)
            });
        }

        private static SubHavalehIndexItemViewModel BuildSubHavalehIndexItem(SubHavaleh x)
        {
            var activeAssignments = (x.TractorAssignments ?? new List<TractorAssignment>())
                .Where(a => a.Status != AssignmentStatus.Cancelled)
                .ToList();

            var requestedAmount = x.RequestedCargoAmount ?? 0;
            var assignedAmount = activeAssignments.Sum(a => a.AssignedCargoAmount ?? 0);
            var loadedAmount = activeAssignments.Sum(a => a.LoadedAmount ?? 0);
            var unloadedAmount = activeAssignments.Sum(a => a.UnloadedAmount ?? 0);
            var effectiveUsedAmount = activeAssignments.Sum(GetEffectiveUsedAmount);
            var hasOpenAssignment = activeAssignments.Any(a =>
                a.Status != AssignmentStatus.Unloaded &&
                a.Status != AssignmentStatus.Completed);
            var isCompleted = requestedAmount > 0 && unloadedAmount >= requestedAmount && !hasOpenAssignment;

            return new SubHavalehIndexItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ContractType = x.ContractType,
                TransportType = x.TransportType,
                DestinationCityDisplayName = x.DestinationPlace?.Name,
                RouteSummary = x.IntermediatePlaces != null && x.IntermediatePlaces.Any()
                    ? string.Join(" ، ", x.IntermediatePlaces.OrderBy(c => c.SortOrder).Select(c => c.Place.Name))
                    : "بدون شهر میانی",
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                RequestedCargoAmount = x.RequestedCargoAmount,
                RequestedCargoAmountType = x.RequestedCargoAmountType,
                ActiveAssignmentCount = activeAssignments.Count,
                AssignedAmount = assignedAmount,
                LoadedAmount = loadedAmount,
                UnloadedAmount = unloadedAmount,
                EffectiveUsedAmount = effectiveUsedAmount,
                HasOpenAssignment = hasOpenAssignment,
                IsCompleted = isCompleted
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.Havalehs
                .Include(x => x.SubHavalehs)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            if (entity.SubHavalehs != null && entity.SubHavalehs.Any())
            {
                TempData["Warning"] = "این حواله دارای ریزحواله است و امکان حذف آن وجود ندارد.";
                return RedirectToAction(nameof(Index));
            }

            _context.Havalehs.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "حواله با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddSubHavaleh(long havalehId)
        {
            var havaleh = await _context.Havalehs
                .Include(x => x.OriginPlace)
                .FirstOrDefaultAsync(x => x.Id == havalehId);

            if (havaleh == null)
                return NotFound();

            var vm = new SubHavalehUpsertViewModel
            {
                HavalehId = havaleh.Id,
                HavalehNumber = havaleh.HavalehNumber,
                OriginPlaceDisplayName = havaleh.OriginPlace == null ? "-" : havaleh.OriginPlace.Name,
                HavalehUnit = havaleh.Unit,
                RequestedCargoAmountType = "به میزان ظرفیت",
                GoodsOwnerPriceCurrency = "ریال",
                GoodsOwnerTipCurrency = "ریال",
                GoodsOwnerStopFeeCurrency = "ریال",
                DriverPriceCurrency = "ریال",
                DriverTipCurrency = "ریال",
                DriverStopFeeCurrency = "ریال",
                LateDeliveryPenaltyCurrency = "ریال"
            };

            await SetRemainingSubHavalehAmountAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubHavaleh(SubHavalehUpsertViewModel vm)
        {
            NormalizeSubVm(vm);

            var havaleh = await _context.Havalehs
                .Include(x => x.OriginPlace)
                .FirstOrDefaultAsync(x => x.Id == vm.HavalehId);

            if (havaleh == null)
                return NotFound();

            vm.HavalehNumber = havaleh.HavalehNumber;
            vm.OriginPlaceDisplayName = havaleh.OriginPlace == null ? "-" : havaleh.OriginPlace.Name;
            vm.HavalehUnit = havaleh.Unit;
            await SetRemainingSubHavalehAmountAsync(vm);

            if (!ModelState.IsValid)
                return View(vm);
            var havalehAmount = havaleh.ProductAmount ?? 0;
            var newSubAmount = vm.RequestedCargoAmount ?? 0;

            if (newSubAmount <= 0)
            {
                ModelState.AddModelError(nameof(vm.RequestedCargoAmount), "مقدار محموله زیرحواله باید بزرگتر از صفر باشد.");
                return View(vm);
            }

            var currentSubTotal = await _context.SubHavalehs
                .Where(x => x.HavalehId == vm.HavalehId)
                .SumAsync(x => (decimal?)x.RequestedCargoAmount) ?? 0;

            if (currentSubTotal + newSubAmount > havalehAmount)
            {
                var remaining = havalehAmount - currentSubTotal;
                ModelState.AddModelError(nameof(vm.RequestedCargoAmount),
                    $"مجموع مقدار زیرحواله‌ها نباید بیشتر از مقدار حواله باشد. مقدار باقیمانده قابل تعریف: {remaining:N3} {havaleh.Unit}");
                return View(vm);
            }

            var entity = new SubHavaleh
            {
                HavalehId = vm.HavalehId,
                Title = vm.Title,
                ContractType = vm.ContractType,
                SettlementBase = vm.SettlementBase,
                TransportType = vm.TransportType,
                DestinationPlaceId = vm.DestinationPlaceId,
                GoodsOwnerPricePer1000Unit = vm.GoodsOwnerPricePer1000Unit,
                GoodsOwnerPriceCurrency = vm.GoodsOwnerPriceCurrency,
                GoodsOwnerTip = vm.GoodsOwnerTip,
                GoodsOwnerTipCurrency = vm.GoodsOwnerTipCurrency,
                GoodsOwnerStopFee = vm.GoodsOwnerStopFee,
                GoodsOwnerStopFeeCurrency = vm.GoodsOwnerStopFeeCurrency,
                DriverPricePer1000Unit = vm.DriverPricePer1000Unit,
                DriverPriceCurrency = vm.DriverPriceCurrency,
                DriverTip = vm.DriverTip,
                DriverTipCurrency = vm.DriverTipCurrency,
                DriverStopFee = vm.DriverStopFee,
                DriverStopFeeCurrency = vm.DriverStopFeeCurrency,
                AllowedLoadingTime = vm.AllowedLoadingTime,
                AllowedDeliveryTime = vm.AllowedDeliveryTime,
                LateDeliveryPenalty = vm.LateDeliveryPenalty,
                LateDeliveryPenaltyCurrency = vm.LateDeliveryPenaltyCurrency,
                LateDeliveryPenaltyType = vm.LateDeliveryPenaltyType,
                ShortagePenaltyType = vm.ShortagePenaltyType,
                ShortageType = vm.ShortageType,
                FixedShortageAmount = vm.FixedShortageAmount,
                AcceptableWeightLoss = vm.AcceptableWeightLoss,
                IsUnderSupervisor = vm.IsUnderSupervisor,
                RequestedCargoAmountType = vm.RequestedCargoAmountType,
                RequestedCargoAmount = vm.RequestedCargoAmount,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate
            };

            foreach (var place in vm.IntermediatePlaces.Where(x => x.PlaceId.HasValue))
            {
                entity.IntermediatePlaces.Add(new SubHavalehIntermediatePlace
                {
                    PlaceId = place.PlaceId!.Value,
                    SortOrder = place.SortOrder
                });
            }

            _context.SubHavalehs.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "زیرحواله با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = vm.HavalehId });
        }

        [HttpGet]
        public async Task<IActionResult> EditSubHavaleh(long id)
        {
            var entity = await _context.SubHavalehs
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.OriginPlace)
                .Include(x => x.DestinationPlace)
                .Include(x => x.IntermediatePlaces)
                    .ThenInclude(x => x.Place)
                .Include(x => x.TractorAssignments)
                    .ThenInclude(x => x.Tractor)
                .Include(x => x.TractorAssignments)
                    .ThenInclude(x => x.DriverProfile)
                        .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            if (HasActiveAssignment(entity))
            {
                TempData["Warning"] = "این ریزحواله دارای تخصیص لغونشده است و امکان ویرایش آن وجود ندارد.";
                return RedirectToAction(nameof(SubHavalehDetails), new { id = entity.Id });
            }

            var vm = new SubHavalehUpsertViewModel
            {
                Id = entity.Id,
                HavalehId = entity.HavalehId,
                HavalehNumber = entity.Havaleh.HavalehNumber,
                OriginPlaceDisplayName = entity.Havaleh.OriginPlace == null ? "-" : entity.Havaleh.OriginPlace.Name,
                HavalehUnit = entity.Havaleh.Unit,
                Title = entity.Title,
                ContractType = entity.ContractType,
                SettlementBase = entity.SettlementBase,
                TransportType = entity.TransportType,
                DestinationPlaceId = entity.DestinationPlaceId,
                DestinationPlaceDisplayName = entity.DestinationPlace?.Name,
                GoodsOwnerPricePer1000Unit = entity.GoodsOwnerPricePer1000Unit,
                GoodsOwnerPriceCurrency = entity.GoodsOwnerPriceCurrency,
                GoodsOwnerTip = entity.GoodsOwnerTip,
                GoodsOwnerTipCurrency = entity.GoodsOwnerTipCurrency,
                GoodsOwnerStopFee = entity.GoodsOwnerStopFee,
                GoodsOwnerStopFeeCurrency = entity.GoodsOwnerStopFeeCurrency,
                DriverPricePer1000Unit = entity.DriverPricePer1000Unit,
                DriverPriceCurrency = entity.DriverPriceCurrency,
                DriverTip = entity.DriverTip,
                DriverTipCurrency = entity.DriverTipCurrency,
                DriverStopFee = entity.DriverStopFee,
                DriverStopFeeCurrency = entity.DriverStopFeeCurrency,
                AllowedLoadingTime = entity.AllowedLoadingTime,
                AllowedDeliveryTime = entity.AllowedDeliveryTime,
                LateDeliveryPenalty = entity.LateDeliveryPenalty,
                LateDeliveryPenaltyCurrency = entity.LateDeliveryPenaltyCurrency,
                LateDeliveryPenaltyType = entity.LateDeliveryPenaltyType,
                ShortagePenaltyType = entity.ShortagePenaltyType,
                ShortageType = entity.ShortageType,
                FixedShortageAmount = entity.FixedShortageAmount,
                AcceptableWeightLoss = entity.AcceptableWeightLoss,
                IsUnderSupervisor = entity.IsUnderSupervisor,
                RequestedCargoAmountType = entity.RequestedCargoAmountType,
                RequestedCargoAmount = entity.RequestedCargoAmount,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IntermediatePlaces = entity.IntermediatePlaces
                    .OrderBy(x => x.SortOrder)
                    .Select(x => new SubHavalehIntermediatePlaceUpsertItemViewModel
                    {
                        Id = x.Id,
                        PlaceId = x.PlaceId,
                        PlaceDisplayName = x.Place.Name,
                        SortOrder = x.SortOrder
                    })
                    .ToList()
            };

            await SetRemainingSubHavalehAmountAsync(vm, entity.Id);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubHavaleh(SubHavalehUpsertViewModel vm)
        {
            NormalizeSubVm(vm);

            var entity = await _context.SubHavalehs
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.OriginPlace)
                .Include(x => x.IntermediatePlaces)
                .Include(x => x.TractorAssignments)
                .FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (entity == null)
                return NotFound();

            if (HasActiveAssignment(entity))
            {
                TempData["Warning"] = "این ریزحواله دارای تخصیص لغونشده است و امکان ویرایش آن وجود ندارد.";
                return RedirectToAction(nameof(SubHavalehDetails), new { id = entity.Id });
            }

            vm.HavalehNumber = entity.Havaleh.HavalehNumber;
            vm.OriginPlaceDisplayName = entity.Havaleh.OriginPlace == null ? "-" : entity.Havaleh.OriginPlace.Name;
            vm.HavalehUnit = entity.Havaleh.Unit;
            await SetRemainingSubHavalehAmountAsync(vm, entity.Id);

            if (!ModelState.IsValid)
                return View(vm);

            var havalehAmount = entity.Havaleh.ProductAmount ?? 0;
            var newSubAmount = vm.RequestedCargoAmount ?? 0;

            if (newSubAmount <= 0)
            {
                ModelState.AddModelError(nameof(vm.RequestedCargoAmount), "مقدار محموله زیرحواله باید بزرگتر از صفر باشد.");
                return View(vm);
            }

            var otherSubTotal = await _context.SubHavalehs
                .Where(x => x.HavalehId == entity.HavalehId && x.Id != entity.Id)
                .SumAsync(x => (decimal?)x.RequestedCargoAmount) ?? 0;

            if (otherSubTotal + newSubAmount > havalehAmount)
            {
                var remaining = havalehAmount - otherSubTotal;
                ModelState.AddModelError(nameof(vm.RequestedCargoAmount),
                    $"مجموع مقدار زیرحواله‌ها نباید بیشتر از مقدار حواله باشد. حداکثر مقدار مجاز برای این زیرحواله: {remaining:N3} {entity.Havaleh.Unit}");
                return View(vm);
            }

            entity.Title = vm.Title;
            entity.ContractType = vm.ContractType;
            entity.SettlementBase = vm.SettlementBase;
            entity.TransportType = vm.TransportType;
            entity.DestinationPlaceId = vm.DestinationPlaceId;
            entity.GoodsOwnerPricePer1000Unit = vm.GoodsOwnerPricePer1000Unit;
            entity.GoodsOwnerPriceCurrency = vm.GoodsOwnerPriceCurrency;
            entity.GoodsOwnerTip = vm.GoodsOwnerTip;
            entity.GoodsOwnerTipCurrency = vm.GoodsOwnerTipCurrency;
            entity.GoodsOwnerStopFee = vm.GoodsOwnerStopFee;
            entity.GoodsOwnerStopFeeCurrency = vm.GoodsOwnerStopFeeCurrency;
            entity.DriverPricePer1000Unit = vm.DriverPricePer1000Unit;
            entity.DriverPriceCurrency = vm.DriverPriceCurrency;
            entity.DriverTip = vm.DriverTip;
            entity.DriverTipCurrency = vm.DriverTipCurrency;
            entity.DriverStopFee = vm.DriverStopFee;
            entity.DriverStopFeeCurrency = vm.DriverStopFeeCurrency;
            entity.AllowedLoadingTime = vm.AllowedLoadingTime;
            entity.AllowedDeliveryTime = vm.AllowedDeliveryTime;
            entity.LateDeliveryPenalty = vm.LateDeliveryPenalty;
            entity.LateDeliveryPenaltyCurrency = vm.LateDeliveryPenaltyCurrency;
            entity.LateDeliveryPenaltyType = vm.LateDeliveryPenaltyType;
            entity.ShortagePenaltyType = vm.ShortagePenaltyType;
            entity.ShortageType = vm.ShortageType;
            entity.FixedShortageAmount = vm.FixedShortageAmount;
            entity.AcceptableWeightLoss = vm.AcceptableWeightLoss;
            entity.IsUnderSupervisor = vm.IsUnderSupervisor;
            entity.RequestedCargoAmountType = vm.RequestedCargoAmountType;
            entity.RequestedCargoAmount = vm.RequestedCargoAmount;
            entity.StartDate = vm.StartDate;
            entity.EndDate = vm.EndDate;

            _context.SubHavalehIntermediatePlaces.RemoveRange(entity.IntermediatePlaces);

            foreach (var place in vm.IntermediatePlaces.Where(x => x.PlaceId.HasValue))
            {
                entity.IntermediatePlaces.Add(new SubHavalehIntermediatePlace
                {
                    PlaceId = place.PlaceId!.Value,
                    SortOrder = place.SortOrder
                });
            }

            await _context.SaveChangesAsync();

            TempData["Ok"] = "زیرحواله با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Details), new { id = entity.HavalehId });
        }

        [HttpGet]
        public async Task<IActionResult> SubHavalehDetails(long id)
        {
            var entity = await _context.SubHavalehs
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.OriginPlace)
                .Include(x => x.DestinationPlace)
                .Include(x => x.IntermediatePlaces)
                    .ThenInclude(x => x.Place)
                .Include(x => x.TractorAssignments)
                    .ThenInclude(x => x.Tractor)
                .Include(x => x.TractorAssignments)
                    .ThenInclude(x => x.DriverProfile)
                        .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReleaseSubHavalehRemaining(long id)
        {
            var entity = await _context.SubHavalehs
                .Include(x => x.Havaleh)
                .Include(x => x.TractorAssignments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            var requestedAmount = entity.RequestedCargoAmount ?? 0;
            var effectiveUsedAmount = (entity.TractorAssignments ?? new List<TractorAssignment>())
                .Where(x => x.Status != AssignmentStatus.Cancelled)
                .Sum(GetEffectiveUsedAmount);

            if (requestedAmount <= 0 || effectiveUsedAmount >= requestedAmount)
            {
                TempData["Warning"] = "برای این ریزحواله مقدار باقیمانده‌ای جهت برگشت به حواله اصلی وجود ندارد.";
                return RedirectToAction(nameof(SubHavalehDetails), new { id = entity.Id });
            }

            if (effectiveUsedAmount <= 0)
            {
                TempData["Warning"] = "این ریزحواله هنوز هیچ تخصیص یا بارگیری مؤثری ندارد؛ برای برگشت کامل مقدار، آن را حذف یا ویرایش کنید.";
                return RedirectToAction(nameof(SubHavalehDetails), new { id = entity.Id });
            }

            var releasedAmount = requestedAmount - effectiveUsedAmount;
            entity.RequestedCargoAmount = effectiveUsedAmount;

            await _context.SaveChangesAsync();

            TempData["Ok"] = $"مقدار {releasedAmount:N3} {entity.Havaleh.Unit} از باقیمانده ریزحواله به حواله اصلی برگشت داده شد.";
            return RedirectToAction(nameof(Details), new { id = entity.HavalehId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubHavaleh(long id, long havalehId)
        {
            var entity = await _context.SubHavalehs
                .Include(x => x.TractorAssignments)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return NotFound();

            if (HasActiveAssignment(entity))
            {
                TempData["Warning"] = "این ریزحواله دارای تخصیص لغونشده است و امکان حذف آن وجود ندارد.";
                return RedirectToAction(nameof(SubHavalehDetails), new { id = id });
            }

            _context.SubHavalehs.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "زیرحواله با موفقیت حذف شد.";
            return RedirectToAction(nameof(Details), new { id = havalehId });
        }

        [HttpGet]
        public async Task<IActionResult> SearchLegalEntities(string? term)
        {
            term = (term ?? "").Trim();
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var items = await _context.LegalEntities
                .Where(x => x.CompanyName.Contains(term) || (x.CompanyType ?? "").Contains(term) || (x.City ?? "").Contains(term))
                .OrderBy(x => x.CompanyName)
                .Take(30)
                .Select(x => new { id = x.Id, companyName = x.CompanyName, companyType = x.CompanyType, city = x.City })
                .ToListAsync();

            return Json(items);
        }

        [HttpGet]
        public async Task<IActionResult> SearchCities(string? term)
        {
            return await SearchPlaces(term);
        }

        [HttpGet]
        public async Task<IActionResult> SearchPlaces(string? term)
        {
            term = (term ?? "").Trim();
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var items = await _context.Places
                .Where(x => x.CountryName.Contains(term) || x.ProvinceName.Contains(term) || x.CityName.Contains(term) || x.Name.Contains(term) || (x.Address ?? "").Contains(term))
                .OrderBy(x => x.CountryName)
                .ThenBy(x => x.ProvinceName)
                .ThenBy(x => x.CityName)
                .ThenBy(x => x.Name)
                .Take(30)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return Json(items);
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts(string? term)
        {
            term = (term ?? "").Trim();
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var items = await _context.Products
                .Where(x => x.Name.Contains(term) || (x.Type ?? "").Contains(term))
                .OrderBy(x => x.Name)
                .Take(30)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name + (string.IsNullOrWhiteSpace(x.Type) ? "" : " - " + x.Type)
                })
                .ToListAsync();

            return Json(items);
        }


        private async Task SetRemainingSubHavalehAmountAsync(SubHavalehUpsertViewModel vm, long? excludeSubHavalehId = null)
        {
            var havalehAmount = await _context.Havalehs
                .Where(x => x.Id == vm.HavalehId)
                .Select(x => x.ProductAmount ?? 0)
                .FirstOrDefaultAsync();

            var subQuery = _context.SubHavalehs
                .Where(x => x.HavalehId == vm.HavalehId);

            if (excludeSubHavalehId.HasValue)
                subQuery = subQuery.Where(x => x.Id != excludeSubHavalehId.Value);

            var usedAmount = await subQuery.SumAsync(x => (decimal?)x.RequestedCargoAmount) ?? 0;
            vm.RemainingHavalehAmountForSubHavaleh = havalehAmount - usedAmount;
        }

        private static decimal GetEffectiveUsedAmount(TractorAssignment assignment)
        {
            var usedBeforeOrAfterLoading = assignment.LoadedAmount ?? assignment.AssignedCargoAmount ?? 0;
            var unloaded = assignment.UnloadedAmount ?? 0;
            return Math.Max(usedBeforeOrAfterLoading, unloaded);
        }

        private Task<bool> HasAnySubHavalehAsync(long havalehId)
        {
            return _context.SubHavalehs.AnyAsync(x => x.HavalehId == havalehId);
        }

        private static bool HasActiveAssignment(SubHavaleh subHavaleh)
        {
            return (subHavaleh.TractorAssignments ?? new List<TractorAssignment>())
                .Any(x => x.Status != AssignmentStatus.Cancelled);
        }

        private static void NormalizeHavalehVm(HavalehUpsertViewModel vm)
        {
            vm.HavalehNumber = (vm.HavalehNumber ?? "").Trim();
            vm.ContractNumber = string.IsNullOrWhiteSpace(vm.ContractNumber) ? null : vm.ContractNumber.Trim();
            vm.HavalehType = string.IsNullOrWhiteSpace(vm.HavalehType) ? null : vm.HavalehType.Trim();
            vm.TransportContractorDisplayName = string.IsNullOrWhiteSpace(vm.TransportContractorDisplayName) ? null : vm.TransportContractorDisplayName.Trim();
            vm.GoodsOwnerDisplayName = string.IsNullOrWhiteSpace(vm.GoodsOwnerDisplayName) ? null : vm.GoodsOwnerDisplayName.Trim();
            vm.OriginPlaceDisplayName = string.IsNullOrWhiteSpace(vm.OriginPlaceDisplayName) ? null : vm.OriginPlaceDisplayName.Trim();
            vm.ProductDisplayName = string.IsNullOrWhiteSpace(vm.ProductDisplayName) ? null : vm.ProductDisplayName.Trim();
            vm.Unit = string.IsNullOrWhiteSpace(vm.Unit) ? null : vm.Unit.Trim();
        }

        private static void NormalizeSubVm(SubHavalehUpsertViewModel vm)
        {
            vm.Title = string.IsNullOrWhiteSpace(vm.Title) ? null : vm.Title.Trim();
            vm.ContractType = string.IsNullOrWhiteSpace(vm.ContractType) ? null : vm.ContractType.Trim();
            vm.SettlementBase = string.IsNullOrWhiteSpace(vm.SettlementBase) ? null : vm.SettlementBase.Trim();
            vm.TransportType = string.IsNullOrWhiteSpace(vm.TransportType) ? null : vm.TransportType.Trim();
            vm.DestinationPlaceDisplayName = string.IsNullOrWhiteSpace(vm.DestinationPlaceDisplayName) ? null : vm.DestinationPlaceDisplayName.Trim();
            vm.GoodsOwnerPriceCurrency = NormalizeCurrency(vm.GoodsOwnerPriceCurrency);
            vm.GoodsOwnerTipCurrency = NormalizeCurrency(vm.GoodsOwnerTipCurrency);
            vm.GoodsOwnerStopFeeCurrency = NormalizeCurrency(vm.GoodsOwnerStopFeeCurrency);
            vm.DriverPriceCurrency = NormalizeCurrency(vm.DriverPriceCurrency);
            vm.DriverTipCurrency = NormalizeCurrency(vm.DriverTipCurrency);
            vm.DriverStopFeeCurrency = NormalizeCurrency(vm.DriverStopFeeCurrency);
            vm.LateDeliveryPenaltyCurrency = NormalizeCurrency(vm.LateDeliveryPenaltyCurrency);
            vm.LateDeliveryPenaltyType = string.IsNullOrWhiteSpace(vm.LateDeliveryPenaltyType) ? null : vm.LateDeliveryPenaltyType.Trim();
            vm.ShortagePenaltyType = string.IsNullOrWhiteSpace(vm.ShortagePenaltyType) ? null : vm.ShortagePenaltyType.Trim();
            vm.ShortageType = string.IsNullOrWhiteSpace(vm.ShortageType) ? null : vm.ShortageType.Trim();
            vm.RequestedCargoAmountType = string.IsNullOrWhiteSpace(vm.RequestedCargoAmountType) ? null : vm.RequestedCargoAmountType.Trim();

            vm.IntermediatePlaces = vm.IntermediatePlaces
                .Where(x => x.PlaceId.HasValue)
                .Select((x, index) => new SubHavalehIntermediatePlaceUpsertItemViewModel
                {
                    Id = x.Id,
                    PlaceId = x.PlaceId,
                    PlaceDisplayName = string.IsNullOrWhiteSpace(x.PlaceDisplayName) ? null : x.PlaceDisplayName.Trim(),
                    SortOrder = index + 1
                })
                .ToList();
        }

        private static string NormalizeCurrency(string? currency)
        {
            currency = (currency ?? "ریال").Trim();
            return string.IsNullOrWhiteSpace(currency) ? "ریال" : currency;
        }
    }
}
