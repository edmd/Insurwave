using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Insurwave.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Insurwave.Domain
{
    public class WeatherService : IWeatherService
    {
        private readonly string _apiKey;
        private readonly string _domain;
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(ILogger<WeatherService> logger, HttpClient httpClient, string apiKey, string domain)
        {
            _logger = logger;
            _httpClient = httpClient;
            _apiKey = apiKey;
            _domain = domain;
        }

        public async Task<Weather> GetWeather(string cityName, Temperature? t = null)
        {
            Weather weather = new Weather();

            #region Parameter validation...
            if (string.IsNullOrEmpty(cityName))
            {
                weather.Error = Errors.GetErrorMessage(ErrorCode.MissingLocation);
                return weather;
            }
            #endregion

            try
            {
                // These Api calls can run concurrently
                var taskCurrent =  GetCurrentWeather(cityName);
                var taskAstronomy = GetAstronomyWeather(cityName);

                try
                {
                    await Task.WhenAll(new Task[] { taskCurrent, taskAstronomy });
                } catch
                {
                    // swallow
                }

                // The following needs to happen syncronously
                if (taskCurrent.IsCompletedSuccessfully && taskCurrent.Result != null)
                {
                    var currentJson = await taskCurrent.Result.Content.ReadAsStringAsync();

                    switch (taskCurrent.Result.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            var currentApiResponse = JsonConvert.DeserializeObject<CurrentApiResponse>(currentJson);
                            weather.City = currentApiResponse.location.name;
                            weather.Region = currentApiResponse.location.region;
                            weather.Country = currentApiResponse.location.country;
                            //weather.LocalTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(currentApiResponse.location.localtime_epoch).ToString(); // this would viable if a timezone was supplied
                            weather.LocalTime = currentApiResponse.location.localtime;

                            switch(t.GetValueOrDefault(Temperature.c))
                            {
                                case Temperature.c:
                                    weather.Temperature = currentApiResponse.current.temp_c.ToString("0.0");
                                    break;
                                case Temperature.f:
                                    weather.Temperature = currentApiResponse.current.temp_f.ToString("0.0"); // temp_c * 9 / 5 + 32
                                    break;
                                case Temperature.k:
                                    weather.Temperature = (currentApiResponse.current.temp_c + 273.15D).ToString("0.00");
                                    break;
                            }
                            break;
                        case HttpStatusCode.BadRequest:
                            var errorApiResponse = JsonConvert.DeserializeObject<ErrorApiResponse>(currentJson);
                            if (errorApiResponse.error.code == (int)ErrorCode.InvalidLocation) // We only want to handle this error code
                            {
                                weather.Error = Errors.GetErrorMessage(ErrorCode.InvalidLocation);
                                return weather;
                            }
                            break;
                        case HttpStatusCode.InternalServerError:
                            weather.Error = Errors.GetErrorMessage(ErrorCode.ApplicationError);
                            return weather;
                    }
                }

                if (taskAstronomy.IsCompletedSuccessfully && taskAstronomy.Result != null)
                {
                    var astronomyJson = await taskAstronomy.Result.Content.ReadAsStringAsync();

                    if (taskAstronomy.Result.StatusCode == HttpStatusCode.OK)
                    {
                        var astronomyApiResponse = JsonConvert.DeserializeObject<AstronomyApiResponse>(astronomyJson);
                        weather.Moonrise = astronomyApiResponse.astronomy.astro.moonrise;
                        weather.Moonset = astronomyApiResponse.astronomy.astro.moonset;
                        weather.Sunrise = astronomyApiResponse.astronomy.astro.sunrise;
                        weather.Sunset = astronomyApiResponse.astronomy.astro.sunset;
                    }
                }

                if (taskCurrent.IsFaulted) // Add the exception to the response if one of the Apis is faulted
                {
                    throw taskCurrent.Exception.InnerException;
                }
                else if (taskAstronomy.IsFaulted)
                {
                    throw taskAstronomy.Exception.InnerException;
                }

                return weather;
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, ex.Message);
                weather.Error = Errors.GetErrorMessage(ErrorCode.ServiceUnavailable);
                return weather;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            weather.Error = Errors.GetErrorMessage(ErrorCode.ApplicationError);
            return weather;
        }

        public async Task<HttpResponseMessage> GetCurrentWeather(string cityName)
        {
            try
            {
                //throw new TimeoutException();
                // Call standard weather Api endpoint
                return await _httpClient.GetAsync($"{_domain}/current.json?key={_apiKey}&q={cityName}");
            } catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        public async Task<HttpResponseMessage> GetAstronomyWeather(string cityName)
        {
            try
            {
                //throw new TimeoutException();
                // Call astronomy weather Api endpoint (date is optional for this call - defaulting to now)
                return await _httpClient.GetAsync($"{_domain}/astronomy.json?key={_apiKey}&q={cityName}");
            } catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}