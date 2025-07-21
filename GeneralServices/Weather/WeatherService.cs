using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GeneralServices.Calendars;
using GeneralServices.Models.Weather;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GeneralServices.Weather
{
    public class WeatherService
    {
        private readonly MemoryCacheEntryOptions _historyOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));
        private readonly MemoryCacheEntryOptions _forecastOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));

        private readonly IMemoryCache _cache;
        private readonly HttpClient _client;

        public WeatherService(IMemoryCache cache)
        {
            _cache = cache;
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
        }

        private const string ApiUrl = "https://weather.proman-api.ir/api";

        public async Task<WeatherInfo> GetWeatherDayHistory(string city, string date)
        {
            try
            {
                string thisCacheKey = $"history-{city}-{date}";

                if (!_cache.TryGetValue(thisCacheKey, out WeatherInfo wi))
                {
                    wi = new WeatherInfo();

                    if (string.IsNullOrEmpty(date)) date = date.ToPersianDateTime().AddDays(-1).ToShortDateString();

                    string url = $"{ApiUrl}/weather/History?city={city}&date={date}";

                    var response = await _client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        wi = JsonConvert.DeserializeObject<WeatherInfo>(await response.Content.ReadAsStringAsync());

                        _cache.Set(thisCacheKey, wi, _historyOptions);
                    }
                }

                return wi;
            }
            catch (Exception)
            {
                return new WeatherInfo();
            }
        }

        public async Task<WeatherForecast> GetWeatherForecast(string city)
        {
            try
            {
                string thisCacheKey = $"forecast-{city}";

                if (!_cache.TryGetValue(thisCacheKey, out WeatherForecast wf))
                {
                    wf = new WeatherForecast();

                    string url = $"{ApiUrl}/weather/Forecast?city={city}";

                    var response = await _client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        wf = JsonConvert.DeserializeObject<WeatherForecast>(await response.Content.ReadAsStringAsync());
                        _cache.Set(thisCacheKey, wf, _forecastOptions);
                    }
                }

                return wf;
            }
            catch (Exception)
            {
                return new WeatherForecast();
            }
        }

        public async Task<List<WeatherType>> GetWeatherTypes()
        {
            try
            {
                if (!_cache.TryGetValue(nameof(WeatherType), out List<WeatherType> wt))
                {
                    wt = new List<WeatherType>();

                    string url = $"{ApiUrl}/weatherType";

                    var response = await _client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        wt = JsonConvert.DeserializeObject<List<WeatherType>>(await response.Content.ReadAsStringAsync());
                        _cache.Set(nameof(WeatherType), wt, _forecastOptions);
                    }
                }

                return wt;
            }
            catch (Exception)
            {
                return new List<WeatherType>();
            }
        }
        public async Task<SelectList> GetWeatherTypesSelectList()
        {
            var weatherTypes = await GetWeatherTypes();

            return new SelectList(weatherTypes, "Code", "Name");
        }
    }
}
