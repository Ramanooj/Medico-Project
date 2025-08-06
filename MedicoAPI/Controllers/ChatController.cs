using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ChatController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (string.IsNullOrEmpty(request.Prompt))
        {
            return BadRequest("Prompt is required");
        }

        var apiKey = Environment.GetEnvironmentVariable("apiKey");
        var url = "https://api.openai.com/v1/chat/completions";

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "user", content = request.Prompt }
            },
            max_tokens = 150
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await _httpClient.PostAsync(url, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonDocument.Parse(responseContent);
            var reply = result.RootElement.GetProperty("choices")[0]
                                           .GetProperty("message")
                                           .GetProperty("content")
                                           .GetString()
                                           .Trim();
            return Ok(new { Reply = reply });
        }

        return StatusCode((int)response.StatusCode, responseContent);
    }
}

public class ChatRequest
{
    public string Prompt { get; set; }
}
