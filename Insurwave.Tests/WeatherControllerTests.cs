using FluentAssertions;
using Insurwave.Domain;
using Insurwave.Model;
using Insurwave.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Insurwave.Tests
{
    // TODO: Run code coverage metrics
    public class WeatherControllerTests
    {
        #region Variables...
        private Mock<IWeatherService> _mockService;
        private Mock<ILogger<WeatherController>> _mockLogger;
        private WeatherController _controller;
        #endregion

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IWeatherService>();
            _mockLogger = new Mock<ILogger<WeatherController>>();
        }

        [Test]
        public void CityNameNullTest()
        {
            _mockService.Setup(repo => repo.GetWeather(Utilities.CityNameNull, It.IsAny<Temperature>())).Returns(
                Task.FromResult(new Weather { Error = Errors.GetErrorMessage(ErrorCode.MissingLocation) }));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.CityNameNull).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as BadRequestObjectResult).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var returnedWeather = ((response.Result as BadRequestObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.MissingLocation, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.MissingLocation}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.MissingLocation).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.MissingLocation).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");
        }

        [Test]
        public void CityNameEmptyStringTest()
        {
            _mockService.Setup(repo => repo.GetWeather(Utilities.CityNameEmptyString, It.IsAny<Temperature>())).Returns(
                Task.FromResult(new Weather { Error = Errors.GetErrorMessage(ErrorCode.MissingLocation) }));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.CityNameEmptyString).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as BadRequestObjectResult).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var returnedWeather = ((response.Result as BadRequestObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.MissingLocation, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.MissingLocation}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.MissingLocation).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.MissingLocation).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");
        }

        [Test]
        public void CityNameWithSpacesTest()
        {
            var Weather = new Weather
            {
                City = "Monmouth Heights At Manalapan",
                Country = "United States of America",
                LocalTime = "2020-04-12 18:28",
                Region = "New Jersey",
                Temperature = "16.7",
                Error = null
            };

            _mockService.Setup(repo => repo.GetWeather(Utilities.CityNameWithSpaces, It.IsAny<Temperature>())).Returns(
                Task.FromResult(Weather));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.CityNameWithSpaces).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual(Weather.City, returnedWeather.City,
                $"Response City was {returnedWeather.City}, expected {Weather.City}");
            Assert.AreEqual(Weather.Country, returnedWeather.Country,
                $"Response Country was {returnedWeather.Country}, expected {Weather.Country}");
            Assert.AreEqual(Weather.LocalTime, returnedWeather.LocalTime,
                $"Response LocalTime was {returnedWeather.LocalTime}, expected {Weather.LocalTime}");
            Assert.AreEqual(Weather.Region, returnedWeather.Region,
                $"Response Region was {returnedWeather.Region}, expected {Weather.Region}");
            Assert.AreEqual(Weather.Temperature, returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {Weather.Temperature}");
            Assert.IsNull(returnedWeather.Error, $"Response Error was expected to be null");
        }

        [Test]
        public void CityNameWithHyphensTest()
        {
            var Weather = new Weather
            {
                City = "Winchester-On-The-Severn",
                Country = "United States of America",
                LocalTime = "2020-04-12 18:22",
                Region = "Maryland",
                Temperature = "17.8",
                Error = null
            };

            _mockService.Setup(repo => repo.GetWeather(Utilities.CityNameWithHyphens, It.IsAny<Temperature>())).Returns(
                Task.FromResult(Weather));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.CityNameWithHyphens).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual(Weather.City, returnedWeather.City,
                $"Response City was {returnedWeather.City}, expected {Weather.City}");
            Assert.AreEqual(Weather.Country, returnedWeather.Country,
                $"Response Country was {returnedWeather.Country}, expected {Weather.Country}");
            Assert.AreEqual(Weather.LocalTime, returnedWeather.LocalTime,
                $"Response LocalTime was {returnedWeather.LocalTime}, expected {Weather.LocalTime}");
            Assert.AreEqual(Weather.Region, returnedWeather.Region,
                $"Response Region was {returnedWeather.Region}, expected {Weather.Region}");
            Assert.AreEqual(Weather.Temperature, returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {Weather.Temperature}");
            Assert.IsNull(returnedWeather.Error, $"Response Error was expected to be null");
        }

        [Test]
        public void CityNameDuplicateNameTest()
        {
            var Weather = new Weather
            {
                City = "Paris",
                Country = "France",
                LocalTime = "2020-04-13 0:51",
                Region = "Ile-de-France",
                Temperature = "15.0",
                Error = null
            };

            _mockService.Setup(repo => repo.GetWeather(Utilities.DuplicateCityName1, It.IsAny<Temperature>())).Returns(
                Task.FromResult(Weather));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual(Weather.City, returnedWeather.City,
                $"Response City was {returnedWeather.City}, expected {Weather.City}");
            Assert.AreEqual(Weather.Country, returnedWeather.Country,
                $"Response Country was {returnedWeather.Country}, expected {Weather.Country}");
            Assert.AreEqual(Weather.LocalTime, returnedWeather.LocalTime,
                $"Response LocalTime was {returnedWeather.LocalTime}, expected {Weather.LocalTime}");
            Assert.AreEqual(Weather.Region, returnedWeather.Region,
                $"Response Region was {returnedWeather.Region}, expected {Weather.Region}");
            Assert.AreEqual(Weather.Temperature, returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {Weather.Temperature}");
            Assert.IsNull(returnedWeather.Error, $"Response Error was expected to be null");
        }

        [Test]
        public void CityName2ndDuplicateNameTest()
        {
            var Weather = new Weather
            {
                City = "Paris",
                Country = "United States of America",
                LocalTime = "2020-04-12 18:00",
                Region = "Texas",
                Temperature = "16.0",
                Error = null
            };

            _mockService.Setup(repo => repo.GetWeather(Utilities.DuplicateCityName2, It.IsAny<Temperature>())).Returns(
                Task.FromResult(Weather));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.DuplicateCityName2).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual(Weather.City, returnedWeather.City,
                $"Response City was {returnedWeather.City}, expected {Weather.City}");
            Assert.AreEqual(Weather.Country, returnedWeather.Country,
                $"Response Country was {returnedWeather.Country}, expected {Weather.Country}");
            Assert.AreEqual(Weather.LocalTime, returnedWeather.LocalTime,
                $"Response LocalTime was {returnedWeather.LocalTime}, expected {Weather.LocalTime}");
            Assert.AreEqual(Weather.Region, returnedWeather.Region,
                $"Response Region was {returnedWeather.Region}, expected {Weather.Region}");
            Assert.AreEqual(Weather.Temperature, returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {Weather.Temperature}");
            Assert.IsNull(returnedWeather.Error, $"Response Error was expected to be null");
        }

        [Test]
        public void CityNameInvalidTest()
        {
            _mockService.Setup(repo => repo.GetWeather(Utilities.CityNameGobbledygook, It.IsAny<Temperature>())).Returns(
                Task.FromResult(new Weather { Error = Errors.GetErrorMessage(ErrorCode.InvalidLocation) }));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.CityNameGobbledygook).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            var returnedWeather = ((response.Result as NotFoundObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.InvalidLocation, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.InvalidLocation}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.InvalidLocation).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.InvalidLocation).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");
        }

        [Test]
        public void CityNameTooLongTest()
        {
            var tooLongCityName = Utilities.MaximumNginx5738CharsQueryString;
            _mockService.Setup(repo => repo.GetWeather(tooLongCityName, It.IsAny<Temperature>())).Returns(
                Task.FromResult(new Weather { Error = Errors.GetErrorMessage(ErrorCode.ApplicationError) }));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(tooLongCityName).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.ApplicationError, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.ApplicationError}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ApplicationError).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ApplicationError).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");
        }

        [Test, Category("Integration")]
        public void CityNameHappyPathTest()
        {
            var Weather = new Weather
            {
                City = "Paris",
                Country = "France",
                Region = "Ile-de-France"
            };

            var weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                new WeatherApiClient(new HttpClient()).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
            _controller = new WeatherController(new LoggerFactory().CreateLogger<WeatherController>(), weatherService);

            var response = _controller.Get(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual(Weather.City, returnedWeather.City,
                $"Response City was {returnedWeather.City}, expected {Weather.City}");
            Assert.AreEqual(Weather.Country, returnedWeather.Country,
                $"Response Country was {returnedWeather.Country}, expected {Weather.Country}");
            Assert.IsNotNull(returnedWeather.LocalTime, $"Response LocalTime was not expected to be null");
            Assert.AreEqual(Weather.Region, returnedWeather.Region,
                $"Response Region was {returnedWeather.Region}, expected {Weather.Region}");
            Assert.IsNotNull(returnedWeather.Temperature, $"Response Temperature was not expected to be null");

            Assert.IsNotNull(returnedWeather.Moonrise, $"Response Moonrise was not expected to be null");
            Assert.IsNotNull(returnedWeather.Moonset, $"Response Moonset was not expected to be null");
            Assert.IsNotNull(returnedWeather.Sunrise, $"Response Sunrise was not expected to be null");
            Assert.IsNotNull(returnedWeather.Sunset, $"Response Sunset was not expected to be null");

            Assert.IsNull(returnedWeather.Error, $"Response Error was expected to be null");
        }

        [Test, Category("Integration")]
        public void ApiTimeoutExceptionTest()
        {
            //https://github.com/richardszalay/mockhttp
            var mockHandler = new MockHttpMessageHandler();

            // Setup a Timeout response
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/*").Throw(new TimeoutException());
            // Inject the handler or client into your application code
            var httpClient = new HttpClient(mockHandler);

            var weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                new WeatherApiClient(httpClient).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
            _controller = new WeatherController(new LoggerFactory().CreateLogger<WeatherController>(), weatherService);

            var response = _controller.Get(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.ServiceUnavailable, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.ServiceUnavailable}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");

            Assert.IsNull(returnedWeather.Moonrise, $"Response Moonrise was expected to be null");
            Assert.IsNull(returnedWeather.Moonset, $"Response Moonset was expected to be null");
            Assert.IsNull(returnedWeather.Sunrise, $"Response Sunrise was expected to be null");
            Assert.IsNull(returnedWeather.Sunset, $"Response Sunset was expected to be null");
        }

        [Test, Category("Integration")]
        public void CurrentApiTimeoutExceptionTest()
        {
            //https://github.com/richardszalay/mockhttp
            var mockHandler = new MockHttpMessageHandler();

            // Setup a Timeout response
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/astronomy.json").Respond(new HttpClient());
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/current.json").Throw(new TimeoutException());
            // Inject the handler or client into your application code
            var httpClient = new HttpClient(mockHandler);

            var weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                new WeatherApiClient(httpClient).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
            _controller = new WeatherController(new LoggerFactory().CreateLogger<WeatherController>(), weatherService);

            var response = _controller.Get(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.ServiceUnavailable, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.ServiceUnavailable}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");

            Assert.IsNotNull(returnedWeather.Moonrise, $"Response Moonrise was not expected to be null");
            Assert.IsNotNull(returnedWeather.Moonset, $"Response Moonset was not expected to be null");
            Assert.IsNotNull(returnedWeather.Sunrise, $"Response Sunrise was not expected to be null");
            Assert.IsNotNull(returnedWeather.Sunset, $"Response Sunset was not expected to be null");
        }

        [Test, Category("Integration")]
        public void AstronomyApiTimeoutExceptionTest()
        {
            //https://github.com/richardszalay/mockhttp
            var mockHandler = new MockHttpMessageHandler();

            // Setup a Timeout response
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/astronomy.json").Throw(new TimeoutException());
            mockHandler.When(Utilities.InsurwaveWeatherApiDomain + "/current.json").Respond(new HttpClient());
            // Inject the handler or client into your application code
            var httpClient = new HttpClient(mockHandler);

            var weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                new WeatherApiClient(httpClient).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
            _controller = new WeatherController(new LoggerFactory().CreateLogger<WeatherController>(), weatherService);

            var response = _controller.Get(Utilities.DuplicateCityName1).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.ServiceUnavailable, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.ServiceUnavailable}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.ServiceUnavailable).message}");
            Assert.IsNotNull(returnedWeather.City, $"Response City was not expected to be null");
            Assert.IsNotNull(returnedWeather.Country, $"Response Country was not expected to be null");
            Assert.IsNotNull(returnedWeather.LocalTime, $"Response LocalTime was not expected to be null");
            Assert.IsNotNull(returnedWeather.Region, $"Response Region was not expected to be null");
            Assert.IsNotNull(returnedWeather.Temperature, $"Response Temperature was not expected to be null");

            Assert.IsNull(returnedWeather.Moonrise, $"Response Moonrise was expected to be null");
            Assert.IsNull(returnedWeather.Moonset, $"Response Moonset was expected to be null");
            Assert.IsNull(returnedWeather.Sunrise, $"Response Sunrise was expected to be null");
            Assert.IsNull(returnedWeather.Sunset, $"Response Sunset was expected to be null");
        }

        #region Temperature tests...

        [Test]
        public void ValidTemperatureTest()
        {
            var Weather = new Weather
            {
                City = "Paris",
                Country = "United States of America",
                LocalTime = "2020-04-12 18:00",
                Region = "Texas",
                Temperature = "16.0",
                Error = null
            };

            _mockService.Setup(repo => repo.GetWeather(Utilities.DuplicateCityName2, Temperature.c)).Returns(
                Task.FromResult(Weather));
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.DuplicateCityName2).Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as ObjectResult).StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedWeather = ((response.Result as ObjectResult).Value as Weather);

            Assert.AreEqual(Weather.City, returnedWeather.City,
                $"Response City was {returnedWeather.City}, expected {Weather.City}");
            Assert.AreEqual(Weather.Country, returnedWeather.Country,
                $"Response Country was {returnedWeather.Country}, expected {Weather.Country}");
            Assert.IsNotNull(returnedWeather.LocalTime, $"Response LocalTime was not expected to be null");
            Assert.AreEqual(Weather.Region, returnedWeather.Region,
                $"Response Region was {returnedWeather.Region}, expected {Weather.Region}");
            Assert.IsNotNull(returnedWeather.Temperature, $"Response Temperature was not expected to be null");
            Assert.IsNull(returnedWeather.Error, $"Response Error was expected to be null");
        }

        [Test]
        public void InvalidTemperatureTest()
        {
            var Weather = new Weather
            {
                City = "Paris",
                Country = "United States of America",
                LocalTime = "2020-04-12 18:00",
                Region = "Texas",
                Temperature = "16.0",
                Error = null
            };

            _mockService.Setup(repo => repo.GetWeather(Utilities.DuplicateCityName2, Temperature.c)).Returns(
                Task.FromResult(Weather)); // Won't hit the Mock anyway
            _controller = new WeatherController(_mockLogger.Object, _mockService.Object);

            var response = _controller.Get(Utilities.DuplicateCityName2, 't').Result;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<ActionResult<Weather>>(response);

            (response.Result as BadRequestObjectResult).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var returnedWeather = ((response.Result as BadRequestObjectResult).Value as Weather);

            Assert.AreEqual((int)ErrorCode.InvalidTemperature, returnedWeather.Error.code,
                $"Response body error code was {returnedWeather.Error.code}, expected {(int)ErrorCode.InvalidTemperature}");
            Assert.AreEqual(Errors.GetErrorMessage(ErrorCode.InvalidTemperature).message, returnedWeather.Error.message,
                $"Response body error message was {returnedWeather.Error.message}, expected {Errors.GetErrorMessage(ErrorCode.InvalidTemperature).message}");
            Assert.IsNull(returnedWeather.City, $"Response City was expected to be null");
            Assert.IsNull(returnedWeather.Country, $"Response Country was expected to be null");
            Assert.IsNull(returnedWeather.LocalTime, $"Response LocalTime was expected to be null");
            Assert.IsNull(returnedWeather.Region, $"Response Region was expected to be null");
            Assert.IsNull(returnedWeather.Temperature, $"Response Temperature was expected to be null");
        }

        [Test]
        public void NullTemperatureTest()
        {
            double expectedCelcius;
            var weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                new WeatherApiClient(new HttpClient()).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
            _controller = new WeatherController(new LoggerFactory().CreateLogger<WeatherController>(), weatherService);

            var response = _controller.Get(Utilities.DuplicateCityName1).Result;
            expectedCelcius = Convert.ToDouble(((response.Result as ObjectResult).Value as Weather).Temperature);

            response = _controller.Get(Utilities.DuplicateCityName1, 'c').Result;
            var returnedWeather = (response.Result as ObjectResult).Value as Weather;

            Assert.AreEqual(expectedCelcius.ToString("0.0"), returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {expectedCelcius:0.0}");
        }

        [Test, Category("Integration")]
        public void TemperatureConversionsTest()
        {
            double celcius, expectedFahrenheit, expectedKelvin;

            var weatherService = new WeatherService(
                new LoggerFactory().CreateLogger<WeatherService>(),
                new WeatherApiClient(new HttpClient()).Client,
                Utilities.InsurwaveWeatherApiKey,
                Utilities.InsurwaveWeatherApiDomain
            );
            _controller = new WeatherController(new LoggerFactory().CreateLogger<WeatherController>(), weatherService);

            var response = _controller.Get(Utilities.DuplicateCityName1, 'c').Result;

            celcius = Convert.ToDouble(((response.Result as ObjectResult).Value as Weather).Temperature);
            expectedFahrenheit = celcius * 9 / 5 + 32;
            expectedKelvin = celcius + 273.15D;

            response = _controller.Get(Utilities.DuplicateCityName1, 'f').Result;
            var returnedWeather = (response.Result as ObjectResult).Value as Weather;
            Assert.AreEqual(expectedFahrenheit.ToString("0.0"), returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {expectedFahrenheit:0.0}");



            response = _controller.Get(Utilities.DuplicateCityName1, 'k').Result;
            returnedWeather = (response.Result as ObjectResult).Value as Weather;
            Assert.AreEqual(expectedKelvin.ToString("0.00"), returnedWeather.Temperature,
                $"Response Temperature was {returnedWeather.Temperature}, expected {expectedKelvin:0.00}");
        }

        #endregion
    }
}