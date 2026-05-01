using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS.MVC.Controllers;

[AllowAnonymous]
[Route("geo")]
public class GeoController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GeoController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET /geo/search?q=Tehran
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int limit = 6)
    {
        q = (q ?? "").Trim();
        if (q.Length < 3) return Ok(Array.Empty<object>());

        limit = Math.Clamp(limit, 1, 10);

        var client = _httpClientFactory.CreateClient("Nominatim");

        // countrycodes=ir اختیاریه؛ اگر می‌خوای فقط ایران باشه نگه دار
        var url =
            $"search?format=jsonv2&addressdetails=1&limit={limit}" +
            $"&countrycodes=ir" +
            $"&q={Uri.EscapeDataString(q)}";

        var resp = await client.GetAsync(url);
        if (!resp.IsSuccessStatusCode)
            return Ok(Array.Empty<object>());

        await using var stream = await resp.Content.ReadAsStreamAsync();
        var doc = await JsonDocument.ParseAsync(stream);

        // خروجی سبک و تمیز برای UI
        var items = doc.RootElement.EnumerateArray().Select(x => new
        {
            display = x.GetProperty("display_name").GetString(),
            lat = x.GetProperty("lat").GetString(),
            lon = x.GetProperty("lon").GetString(),
        });

        return Ok(items);
    }
}