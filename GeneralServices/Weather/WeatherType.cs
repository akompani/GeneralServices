using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralServices.Models.Weather
{
    public class WeatherType
    {
        [Key]
        public int Code { get; set; }
        
        public string Name { get; set; }

        public int Icon { get; set; }
    }
}
