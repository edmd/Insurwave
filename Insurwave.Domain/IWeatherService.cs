using Insurwave.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insurwave.Domain
{
    public interface IWeatherService
    {
        Task<Weather> GetWeather(string cityName, Temperature? t = null);
    }
}
