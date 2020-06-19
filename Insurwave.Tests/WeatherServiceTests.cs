using FluentAssertions;
using Insurwave.Domain;
using Insurwave.Model;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;

namespace Insurwave.Tests
{
    public class WeatherServiceTests
    {
        private IWeatherService _weatherService;

        [SetUp]
        public void Setup()
        {
            _weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(), 
                new WeatherApiClient(new HttpClient()).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
        }

        [Test]
        public void CityNameNullTest()
        {
            var data = _weatherService.GetWeather(Utilities.CityNameNull).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.AreEqual((int)ErrorCode.MissingLocation, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.MissingLocation}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.MissingLocation).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.MissingLocation).message}");
        }

        [Test]
        public void CityNameEmptyStringTest()
        {
            var data = _weatherService.GetWeather(Utilities.CityNameEmptyString).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.AreEqual((int)ErrorCode.MissingLocation, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.MissingLocation}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.MissingLocation).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.MissingLocation).message}");
        }

        [Test]
        public void CityNameWithSpacesTest()
        {
            var data = _weatherService.GetWeather(Utilities.CityNameWithSpaces).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.IsNull(data.Error, $"Error was expected to be null.");
            Assert.IsNotNull(data.City, $"City was not populated: {data.City}, a value was expected.");
            Assert.IsNotNull(data.Country, $"Country was not populated: {data.Country}, a value was expected.");
            Assert.IsNotNull(data.LocalTime, $"LocalTime was not populated: {data.LocalTime}, a value was expected.");
            Assert.IsNotNull(data.Region, $"Region was not populated: {data.Region}, a value was expected.");
            Assert.IsNotNull(data.Temperature, $"Temperature was not populated: {data.Temperature}, a value was expected.");
        }

        [Test]
        public void CityNameWithHyphensTest()
        {
            var data = _weatherService.GetWeather(Utilities.CityNameWithHyphens).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.IsNull(data.Error, $"Error was expected to be null.");
            Assert.IsNotNull(data.City, $"City was not populated: {data.City}, a value was expected.");
            Assert.IsNotNull(data.Country, $"Country was not populated: {data.Country}, a value was expected.");
            Assert.IsNotNull(data.LocalTime, $"LocalTime was not populated: {data.LocalTime}, a value was expected.");
            Assert.IsNotNull(data.Region, $"Region was not populated: {data.Region}, a value was expected.");
            Assert.IsNotNull(data.Temperature, $"Temperature was not populated: {data.Temperature}, a value was expected.");
        }

        [Test]
        public void CityNameDuplicateNameTest()
        {
            var data = _weatherService.GetWeather(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.IsNull(data.Error, $"Error was expected to be null.");
            Assert.IsNotNull(data.City, $"City was not populated: {data.City}, a value was expected.");
            Assert.IsNotNull(data.Country, $"Country was not populated: {data.Country}, a value was expected.");
            Assert.IsNotNull(data.LocalTime, $"LocalTime was not populated: {data.LocalTime}, a value was expected.");
            Assert.IsNotNull(data.Region, $"Region was not populated: {data.Region}, a value was expected.");
            Assert.IsNotNull(data.Temperature, $"Temperature was not populated: {data.Temperature}, a value was expected.");
        }

        [Test]
        public void CityName2ndDuplicateNameTest()
        {
            var data = _weatherService.GetWeather(Utilities.DuplicateCityName2).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.IsNull(data.Error, $"Error was expected to be null.");
            Assert.IsNotNull(data.Country, $"Country was not populated: {data.Country}, a value was expected.");
            Assert.IsNotNull(data.LocalTime, $"LocalTime was not populated: {data.LocalTime}, a value was expected.");
            Assert.IsNotNull(data.Region, $"Region was not populated: {data.Region}, a value was expected.");
            Assert.IsNotNull(data.Temperature, $"Temperature was not populated: {data.Temperature}, a value was expected.");
        }

        [Test]
        public void AstronomyFieldsPopulatedTest()
        {
            var data = _weatherService.GetWeather(Utilities.DuplicateCityName2).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.IsNull(data.Error, $"Error was expected to be null.");
            Assert.IsNotNull(data.Country, $"Country was not populated: {data.Country}, a value was expected.");
            Assert.IsNotNull(data.LocalTime, $"LocalTime was not populated: {data.LocalTime}, a value was expected.");
            Assert.IsNotNull(data.Region, $"Region was not populated: {data.Region}, a value was expected.");
            Assert.IsNotNull(data.Temperature, $"Temperature was not populated: {data.Temperature}, a value was expected.");

            Assert.IsNotNull(data.Moonrise, $"Moonrise was not populated: {data.Moonrise}, a value was expected.");
            Assert.IsNotNull(data.Moonset, $"Moonset was not populated: {data.Moonset}, a value was expected.");
            Assert.IsNotNull(data.Sunrise, $"Sunrise was not populated: {data.Sunrise}, a value was expected.");
            Assert.IsNotNull(data.Sunset, $"Sunset was not populated: {data.Sunset}, a value was expected.");
        }

        [Test]
        public void CityNameInvalidTest()
        {
            var data = _weatherService.GetWeather(Utilities.CityNameGobbledygook).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.AreEqual((int)ErrorCode.InvalidLocation, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.InvalidLocation}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.InvalidLocation).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.InvalidLocation).message}");
        }

        [Test]
        public void CityNameTooLongTest()
        {
            // Its happened that their Api actually returns a result for this data...
            var data = _weatherService.GetWeather(Utilities.MaximumNginx5738CharsQueryString).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.AreEqual((int)ErrorCode.ApplicationError, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.ApplicationError}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ApplicationError).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ApplicationError).message}");
        }

        [Test]
        public void TemperatureTest()
        {
            double celcius, expectedFahrenheit, expectedKelvin;
            var data = _weatherService.GetWeather(Utilities.DuplicateCityName1, Temperature.c).Result;

            celcius = Convert.ToDouble(data.Temperature);
            expectedFahrenheit = celcius * 9 / 5 + 32;
            expectedKelvin = celcius + 273.15D;

            data = _weatherService.GetWeather(Utilities.DuplicateCityName1, Temperature.f).Result;
            Assert.AreEqual(expectedFahrenheit.ToString("0.0"), data.Temperature,
                $"Response Temperature was {data.Temperature}, expected {expectedFahrenheit:0.0}");

            data = _weatherService.GetWeather(Utilities.DuplicateCityName1, Temperature.k).Result;
            Assert.AreEqual(expectedKelvin.ToString("0.00"), data.Temperature,
                $"Response Temperature was {data.Temperature}, expected {expectedKelvin:0.00}");
        }

        [Test]
        public void WeatherApiTimeoutExceptionTest()
        {
            //https://github.com/richardszalay/mockhttp
            var mockHandler = new MockHttpMessageHandler();

            // Setup a Timeout response for both Api calls
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/*").Throw(new TimeoutException());
            // Inject the handler or client into your application code
            var httpClient = new HttpClient(mockHandler);

            // Override the weather service for this particular test
            _weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                httpClient,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );

            var data = _weatherService.GetWeather(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.AreEqual((int)ErrorCode.ServiceUnavailable, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.ServiceUnavailable}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message}");
        }

        [Test]
        public void AstronomyApiTimeoutExceptionTest()
        {
            //https://github.com/richardszalay/mockhttp
            var mockHandler = new MockHttpMessageHandler();

            // Setup a Timeout response
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/astronomy.json").Throw(new TimeoutException());
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/current.json").Respond(new HttpClient());
            // Inject the handler or client into your application code
            var httpClient = new HttpClient(mockHandler);

            // Override the weather service for this particular test
            _weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                httpClient,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );

            var data = _weatherService.GetWeather(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            // These values are still expected to be populated
            Assert.IsNotNull(data.City, $"City was not populated: {data.City}, a value was expected.");
            Assert.IsNotNull(data.Country, $"Country was not populated: {data.Country}, a value was expected.");
            Assert.IsNotNull(data.LocalTime, $"LocalTime was not populated: {data.LocalTime}, a value was expected.");
            Assert.IsNotNull(data.Region, $"Region was not populated: {data.Region}, a value was expected.");
            Assert.IsNotNull(data.Temperature, $"Temperature was not populated: {data.Temperature}, a value was expected.");

            Assert.AreEqual((int)ErrorCode.ServiceUnavailable, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.ServiceUnavailable}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message}");
        }

        [Test]
        public void CurrentApiTimeoutExceptionTest()
        {
            //https://github.com/richardszalay/mockhttp
            var mockHandler = new MockHttpMessageHandler();

            // Setup a Timeout response
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/astronomy.json").Respond(new HttpClient());
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/current.json").Throw(new TimeoutException());
            // Inject the handler or client into your application code
            var httpClient = new HttpClient(mockHandler);

            // Override the weather service for this particular test
            _weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                httpClient,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );

            var data = _weatherService.GetWeather(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOf<Weather>(data);

            Assert.AreEqual((int)ErrorCode.ServiceUnavailable, data.Error.code,
                $"Response body error code was {data.Error.code}, expected {(int)ErrorCode.ServiceUnavailable}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message, data.Error.message,
                $"Response body error message was {data.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message}");
        }
    }
}