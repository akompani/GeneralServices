using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GeneralServices.DateTools
{

    public class JalaliDay
    {
        public string Date { get; set; }

        public bool IsHoliday { get; set; }

        public List<JalaliEvent> Events { get; set; }
    }

    public class JalaliEvent
    {
        public JalaliEvent(string description, bool isHoliday)
        {
            Description = description;
            IsHoliday = isHoliday;
        }

        public JalaliEvent(JToken ev)
        {
            Description = ev["description"].ToString();
            IsHoliday = ev["is_holiday"].ToObject<bool>();
        }

        public string Description { get; set; }

        public bool IsHoliday { get; set; }
    }

    public class JalaliHolidayService
    {
        private readonly RestClient _client;
        private string url = "https://holidayapi.ir/jalali/";

        //template : https://holidayapi.ir/jalali/{year}}/{month}/{day}

        //result sample
        //{
        //    "is_holiday": true,
        //    "events": [
        //    {
        //    "description": "جمعه",
        //    "additional_description": "",
        //    "is_holiday": true,
        //    "is_religious": false
        //    },
        //    {
        //        "description": "آغاز هفته دولت",
        //        "additional_description": "",
        //        "is_holiday": false,
        //        "is_religious": false
        //    },
        //    {
        //        "description": "درگذشت جان کندرو بیوشیمیست انگلیسی، برندهٔ جایزه نوبل شیمی سال ۱۹۶۲",
        //        "additional_description": "23 August",
        //        "is_holiday": false,
        //        "is_religious": false
        //    }
        //    ]
        //}{
        //    "is_holiday": true,
        //    "events": [
        //    {
        //        "description": "جمعه",
        //        "additional_description": "",
        //        "is_holiday": true,
        //        "is_religious": false
        //    },
        //    {
        //        "description": "آغاز هفته دولت",
        //        "additional_description": "",
        //        "is_holiday": false,
        //        "is_religious": false
        //    },
        //    {
        //        "description": "درگذشت جان کندرو بیوشیمیست انگلیسی، برندهٔ جایزه نوبل شیمی سال ۱۹۶۲",
        //        "additional_description": "23 August",
        //        "is_holiday": false,
        //        "is_religious": false
        //    }
        //    ]
        //}

        public JalaliHolidayService()
        {
            _client = new RestClient();
        }

        public JalaliDay GetJalaliDay(string date)
        {
            var request = new RestRequest($"{url}{date}");

            var response = _client.Get(request);

            var result = new JalaliDay();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = JToken.Parse(response.Content);

                result.IsHoliday = content["is_holiday"].ToObject<bool>();

                result.Events = new List<JalaliEvent>();

                if (content["events"].Count() > 0)
                {
                    foreach (var ev in content["events"])
                    {
                        result.Events.Add(new JalaliEvent(ev));
                    }
                }
            }

            return result;
        }
    }
}
