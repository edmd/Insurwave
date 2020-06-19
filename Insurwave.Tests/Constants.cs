
using System;

namespace Insurwave.Tests
{
    public static class Utilities
    {
        private static readonly Random randomGenerator = new Random();

        public const string CityNameNull = null;
        public const string CityNameEmptyString = "";
        public const string CityNameWithSpaces = "Monmouth Heights at Manalapan";
        public const string CityNameWithHyphens = "Winchester-on-the-Severn";
        public const string DuplicateCityName1 = "Paris";
        public const string DuplicateCityName2 = "Paris, Texas";
        public const string CityNameGobbledygook = "Gobbledygook";
        public static string MaximumNginx5738CharsQueryString = GenerateRandomString(5700);

        public const string InsurwaveWeatherApiKey = "6aad23c2c551492fa8f183424200904";
        public const string InsurwaveWeatherApiDomain = "https://api.weatherapi.com/v1";

        public static string GenerateRandomString(int length)
        {
            byte[] randomBytes = new byte[randomGenerator.Next(length)];
            randomGenerator.NextBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}