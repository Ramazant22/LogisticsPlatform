using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIAssistantController : TenantControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _aiServiceUrl;

    // IHttpClientFactory kullanarak HttpClient alÄ±yoruz (En iyi pratik)
    public AIAssistantController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _aiServiceUrl = configuration["AI:ServiceUrl"] ?? "http://localhost:8000/api/ask";
    }

    public class AIRequestDto
    {
        public string Question { get; set; } = string.Empty;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskAssistant([FromBody] AIRequestDto requestDto)
    {
        if (!TryGetTenantId(out _)) return Forbid();
        if (string.IsNullOrWhiteSpace(requestDto.Question))
        {
            return BadRequest("Soru boÅŸ olamaz.");
        }

        try
        {
            // Python'un beklediÄŸi JSON formatÄ±nÄ± hazÄ±rlÄ±yoruz (main.py'daki UserQuery)
            var payload = new { question = requestDto.Question };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Python servisine HTTP POST isteÄŸi atÄ±yoruz
            var response = await _httpClient.PostAsync(_aiServiceUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var aiResult = JsonSerializer.Deserialize<JsonElement>(responseContent);
                return Ok(aiResult);
            }
            
            return StatusCode((int)response.StatusCode, "AI servisinden hata dÃ¶ndÃ¼.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"AI servisine baÄŸlanÄ±lamadÄ±. Servisin (FastAPI) ayakta olduÄŸundan emin olun. Hata: {ex.Message}");
        }
    }
}
