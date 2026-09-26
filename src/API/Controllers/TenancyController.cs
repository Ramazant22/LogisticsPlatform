using Microsoft.AspNetCore.Mvc;
using System;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenancyController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTenants()
    {
        // Arayüzde göstermek için örnek SaaS müşteri (Firma/Tenant) verileri
        var mockTenants = new[]
        {
            new { Id = "11111111-1111-1111-1111-111111111111", Name = "Demo Lojistik A.Ş.", Subdomain = "demo.logistics.ai", Plan = "Enterprise", Status = "Active", CreatedAt = "01.09.2026", DbStatus = "Healthy" },
            new { Id = Guid.NewGuid().ToString(), Name = "Kuzey Kargo", Subdomain = "kuzey.logistics.ai", Plan = "Pro", Status = "Active", CreatedAt = "05.09.2026", DbStatus = "Healthy" },
            new { Id = Guid.NewGuid().ToString(), Name = "Hızlı Nakliyat", Subdomain = "hizli.logistics.ai", Plan = "Starter", Status = "Suspended", CreatedAt = "10.09.2026", DbStatus = "Offline" }
        };

        return Ok(mockTenants);
    }
}
