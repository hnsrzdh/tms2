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

            var query = _context.Havalehs
                .Include(x => x.TransportContractorLegalEntity)
                .Include(x => x.GoodsOwnerLegalEntity)
                .Include(x => x.OriginPlace)
                .Include(x => x.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.HavalehNumber.Contains(search) ||
                    (x.ContractNumber ?? "").Contains(search) ||
                    (x.HavalehType ?? "").Contains(search) ||
                    (x.TransportContractorLegalEntity != null ? x.TransportContractorLegalEntity.CompanyName : "").Contains(search) ||
                    (x.GoodsOwnerLegalEntity != null ? x.GoodsOwnerLegalEntity.CompanyName : "").Contains(search) ||
                    (x.OriginPlace != null ? x.OriginPlace.Name : "").Contains(search) ||
                    (x.Product != null ? x.Product.Name : "").Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new HavalehIndexItemViewModel
                {
                    Id = x.Id,
                    HavalehNumber = x.HavalehNumber,
                    ContractNumber = x.ContractNumber,
                    HavalehType = x.HavalehType,
                    RequiresFleetEntryPermit = x.RequiresFleetEntryPermit,
                    TransportContractorName = x.TransportContractorLegalEntity != null ? x.TransportContractorLegalEntity.CompanyName : null,
                    GoodsOwnerName = x.GoodsOwnerLegalEntity != null ? x.GoodsOwnerLegalEntity.CompanyName : null,
                    OriginCityText = x.OriginPlace != null ? x.OriginPlace.Name : null,
                    ProductName = x.Product != null ? x.Product.Name : null,
                    ProductAmount = x.ProductAmount,
                    Unit = x.Unit,
                    PurchaseDate = x.PurchaseDate,
                    AllowedLoadingDate = x.AllowedLoadingDate,
                    ShortagePenaltyPerUnit = x.ShortagePenaltyPerUnit
                })
                .ToListAsync();

            return View(new HavalehIndexViewModel
            {
                Items = items,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            });
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

            var subQuery = _context.SubHavalehs
                .Where(x => x.HavalehId == id)
                .Include(x => x.DestinationPlace)
                .Include(x => x.IntermediatePlaces)
                    .ThenInclude(x => x.Place)
                .AsQueryable();

            var subTotalItems = await subQuery.CountAsync();

            var subItems = await subQuery
                .OrderByDescending(x => x.Id)
                .Skip((subPage - 1) * subPageSize)
                .Take(subPageSize)
                .Select(x => new SubHavalehIndexItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ContractType = x.ContractType,
                    TransportType = x.TransportType,
                    DestinationCityDisplayName = x.DestinationPlace != null ? x.DestinationPlace.Name : null,
                    RouteSummary = x.IntermediatePlaces.OrderBy(c => c.SortOrder).Select(c => c.Place.Name).Any()
                        ? string.Join(" ، ", x.IntermediatePlaces.OrderBy(c => c.SortOrder).Select(c => c.Place.Name))
                        : "بدون شهر میانی",
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    RequestedCargoAmount = x.RequestedCargoAmount,
                    RequestedCargoAmountType = x.RequestedCargoAmountType
                })
                .ToListAsync();

            return View(new HavalehDetailsViewModel
            {
                Entity = entity,
                SubItems = subItems,
                SubPage = subPage,
                SubPageSize = subPageSize,
                SubTotalItems = subTotalItems
            });
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

            return View(new SubHavalehUpsertViewModel
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
            });
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

            if (!ModelState.IsValid)
                return View(vm);

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
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            return View(new SubHavalehUpsertViewModel
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
            });
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
                .FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (entity == null)
                return NotFound();

            vm.HavalehNumber = entity.Havaleh.HavalehNumber;
            vm.OriginPlaceDisplayName = entity.Havaleh.OriginPlace == null ? "-" : entity.Havaleh.OriginPlace.Name;
            vm.HavalehUnit = entity.Havaleh.Unit;

            if (!ModelState.IsValid)
                return View(vm);

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
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubHavaleh(long id, long havalehId)
        {
            var entity = await _context.SubHavalehs.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return NotFound();

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
