using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DocumentFormat.OpenXml.Bibliography;
using GeneralServices.Models;
using MD.PersianDateTime;

namespace GeneralServices.Calendars
{
    public class PersianCalendar
    {
        // 8:00 to 12:00 and 13:00 to 17:00
        private const string DefaultTime = "8:00-12:00,13:00-17:00";
        private const string HalfDefaultTime = "8:00-12:00";

        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool Saturday { get; set; } = true;
        public string SaturdayTimes { get; set; } = DefaultTime;

        public bool Sunday { get; set; } = true;
        public string SundayTimes { get; set; } = DefaultTime;

        public bool Monday { get; set; } = true;
        public string MondayTimes { get; set; } = DefaultTime;

        public bool Tuesday { get; set; } = true;
        public string TuesdayTimes { get; set; } = DefaultTime;

        public bool Wednesday { get; set; } = true;
        public string WednesdayTimes { get; set; } = DefaultTime;

        public bool Thursday { get; set; } = true;
        public string ThursdayTimes { get; set; } = HalfDefaultTime;

        public bool Friday { get; set; }
        public string FridayTimes { get; set; }

        private string GetDayTimes(PersianDayOfWeek day)
        {
            return GetType().GetProperty($"{Enum.GetName(day)}Times").GetValue(this, null)?.ToString() ?? "";
        }

        public Dictionary<PersianDayOfWeek, WorkTimeRange[]> GetWorkTimeRanges()
        {
            var result = new Dictionary<PersianDayOfWeek, WorkTimeRange[]>();

            foreach (PersianDayOfWeek p in Enum.GetValues(typeof(PersianDayOfWeek)))
            {
                var times = GetDayTimes(p);

                var ranges = new List<WorkTimeRange>();

                if (!string.IsNullOrEmpty(times))
                {
                    var timesArray = times.Split(",").ToArray();

                    for (int i = 0; i < timesArray.Length; i++)
                    {
                        ranges.Add(new WorkTimeRange(timesArray[i]));
                    }
                }

                result.Add(p,ranges.ToArray());
            }


            return result;
        }
    }
}
