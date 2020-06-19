using System;
using System.Threading.Tasks;
using Insurwave.Domain;
using Insurwave.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Insurwave.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        /// <summary>
        /// Find the weather for a specific city
        /// </summary>
        /// <param name="q">City name</param>
        /// <param name="t">Temperature conversion</param>
        /// <returns>The weather for a city in a Weather object</returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the city name is null or empty</response>
        /// <response code="404">If the city name is incorrect or cannot be found</response>
        /// <response code="500">If there is an application error</response>
        /// <response code="503">If the Api times out</response>
        [HttpGet()]
        public async Task<ActionResult<Weather>> Get([FromQuery]string q, [FromQuery]char? t = null)
        {
            try
            {
                // TODO: We could initially call the Google Geo Api with the location to submit coordinate data Weather Api consistently

                #region Parameter validation...
                var temp = Temperature.c;
                if (!Enum.TryParse<Temperature>(t.GetValueOrDefault('c').ToString().ToLower(), out temp))
                {
                    return BadRequest(new Weather { Error = Errors.GetErrorMessage(ErrorCode.InvalidTemperature) });
                }
                #endregion

                var result = await _weatherService.GetWeather(q, temp);

                if (result.Error == null)
                {
                    return Ok(result);
                }
                else if (result.Error.code == (int)ErrorCode.MissingLocation) // Have moved parameter validation to make the domain resilient
                {
                    return BadRequest(result);
                }
                else if (result.Error.code == (int)ErrorCode.InvalidLocation)
                {
                    // We have the following semantic options all of which could be correct
                    return NotFound(result);
                    //return Ok(result);
                    //return BadRequest(result);
                    //return StatusCode(422, result);
                }
                else if (result.Error.code == (int)ErrorCode.ApplicationError)
                {
                    return StatusCode(500, result);
                } else if(result.Error.code == (int)ErrorCode.ServiceUnavailable)
                {
                    return StatusCode(503, result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return StatusCode(500);
        }
    }
}