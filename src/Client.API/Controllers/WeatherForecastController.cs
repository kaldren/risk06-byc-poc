using Microsoft.AspNetCore.Mvc;

namespace Client.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast forecast)
        {
            _logger.LogInformation("Received weather forecast: {@Forecast}", forecast);
            return Ok(new { Message = "Forecast received", ReceivedForecast = forecast });
        }

        [HttpPut]
        public IActionResult Put([FromBody] WeatherForecast forecast)
        {
            _logger.LogInformation("Updated weather forecast: {@Forecast}", forecast);
            return Ok(new { Message = "Forecast updated", UpdatedForecast = forecast });
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
