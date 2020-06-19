using Newtonsoft.Json;

namespace Insurwave.Model
{
    /// <summary>
    /// The returned Weather object
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// The city for the weather data
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        /// <summary>
        /// The region of the city for the weather data
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        /// <summary>
        /// The country of the city for the weather data
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>
        /// The local time of the city for the weather data
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LocalTime { get; set; }

        /// <summary>
        /// The temperature of the city
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Temperature { get; set; }

        /// <summary>
        /// The time of the sunrise
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sunrise { get; set; }

        /// <summary>
        /// The time of the sunset
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sunset { get; set; }

        /// <summary>
        /// The time of the moonrise
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Moonrise { get; set; }

        /// <summary>
        /// The time of the moonset
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Moonset { get; set; }

        /// <summary>
        /// Errors and validation returned by the Api
        /// </summary>
        public ErrorMessage Error { get; set; }
    }

    /// <summary>
    /// An error for the weather object if any is returned
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// The code of the error
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int code { get; set; }

        /// <summary>
        /// The description of the error
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
    }
}