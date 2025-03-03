using System.Collections.Generic;

namespace GeneralServices.Models.Weather
{
    public class WeatherForecast
    {
        public string City { get; set; }
        public string Time { get; set; }

        public decimal CurrentTemperature { get; set; }
        public decimal CurrentWind { get; set; }
        public decimal CurrentHumidity { get; set; }
        public WeatherType CurrentWeatherType { get; set; }

        public List<WeatherInfo> WeatherForecasts { get; set; }=new List<WeatherInfo>();

    }
}
