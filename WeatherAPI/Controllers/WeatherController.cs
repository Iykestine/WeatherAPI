using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService service)
        {
            _weatherService = service;
        }

        /// <summary>
        /// Retrieves the weather forecast for a specified location.
        /// </summary>
        /// <param name="location">The name of the city or location (e.g. Lagos, London).</param>
        /// <returns>Weather forecast data for the given location.</returns>
        /// <remarks>
        /// Results may be cached via IMemoryCache. Repeated requests for the same location
        /// within the cache window will return cached data rather than hitting the 3rd party API.
        ///
        /// Sample request:
        ///
        ///     GET /api/weather/forecast?location=Lagos
        ///
        /// </remarks>
        /// <response code="200">Returns the weather forecast for the specified location.</response>
        /// <response code="400">Location is required.</response>
        /// <response code="404">Weather data not found.</response>
        /// <response code="500">An unexpected error occurred while retrieving weather data</response>
        [HttpGet("forecast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWeatherForecast([FromQuery] string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return BadRequest("Location is required");

            var result = await _weatherService.GetWeatherAsync(location);

            if (result == null)
                return NotFound("Weather data not found for the given location.");

            return Ok(result);
        }

    }
}

