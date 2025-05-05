using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Diagnostics;

namespace UserAnalysisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EvaluationController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Evaluate()
        {
            var stopwatch = Stopwatch.StartNew();

            var endpoints = new[]
            {
                "api/users",
                "api/users/superusers",
                "api/users/top-countries",
                "api/users/team-insights",
                "api/users/active-users-per-day"
            };

            var client = _httpClientFactory.CreateClient();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/";

            var results = new Dictionary<string, object>();

            foreach (var endpoint in endpoints)
            {
                var url = $"{baseUrl}{endpoint}";

                var sw = Stopwatch.StartNew();
                try
                {
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    sw.Stop();

                    bool isValidJson = IsValidJson(content);

                    results[$"/{endpoint}"] = new
                    {
                        status = (int)response.StatusCode,
                        time_ms = sw.ElapsedMilliseconds,
                        valid_response = isValidJson
                    };
                }
                catch (Exception ex)
                {
                    results[$"/{endpoint}"] = new
                    {
                        status = 500,
                        time_ms = sw.ElapsedMilliseconds,
                        valid_response = false,
                        error = ex.Message,
                    };
                }
            }

            stopwatch.Stop();

            return Ok(new
            {
                timestamp = DateTimeOffset.Now,
                execution_time_ms = stopwatch.ElapsedMilliseconds,
                tested_endpoints = results
            });
        }

        private bool IsValidJson(string content)
        {
            try
            {
                JsonDocument.Parse(content);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

