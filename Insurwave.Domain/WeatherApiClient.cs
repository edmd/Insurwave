using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Insurwave.Domain
{
    public class WeatherApiClient
    {
        public HttpClient Client { get; }

        public WeatherApiClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}