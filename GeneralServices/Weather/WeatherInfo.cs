using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GeneralServices.Models.Weather
{
    public class WeatherInfo
    {
        public string City { get; set; }
        public string Date { get; set; }

        public decimal MaximumTemperature { get; set; }
        public decimal MinimumTemperature { get; set; }

        public decimal Wind { get; set; }
        public decimal Humidity { get; set; }

        public WeatherType WeatherType { get; set; }


    }
}
