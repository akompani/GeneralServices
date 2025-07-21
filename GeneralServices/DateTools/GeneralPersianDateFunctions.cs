using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeneralServices.Calendars;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneralServices
{
    public static class GeneralPersianDateFunctions
    {
        public static PersianDateTime ToPersianDateTime(this string tDate)
        {
            if (tDate == null) return PersianDateTime.Now;

            return PersianDateTime.TryParse(tDate, out PersianDateTime d) ? d : PersianDateTime.Now;
        }

        public static PersianDateTime ToPersianDateTime(this int tDate)
        {
            if (tDate == 0) return PersianDateTime.Now;

            return PersianDateTime.TryParse(tDate, out PersianDateTime d) ? d : PersianDateTime.Now;
        }

        public static List<SelectListItem> GetPersianMonthsSelectList()
        {
            var list = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                list.Add(new SelectListItem(PersianDateTime.GetPersianMonthName(i), i.ToString()));
            }

            return list;
        }

        public static string GetPersianDayOfWeek(this PersianDayOfWeek dw)
        {
            switch (dw)
            {
                case PersianDayOfWeek.Saturday:
                    return "شنبه";

                case PersianDayOfWeek.Sunday:
                    return "یکشنبه";

                case PersianDayOfWeek.Monday:
                    return "دوشنبه";

                case PersianDayOfWeek.Tuesday:
                    return "سه شنبه";

                case PersianDayOfWeek.Wednesday:
                    return "چهار شنبه";

                case PersianDayOfWeek.Thursday:
                    return "پنج شنبه";

                case PersianDayOfWeek.Friday:
                    return "جمعه";
            }

            return "";
        }

        public static PersianDateTime StartWeekDate(this PersianDateTime date)
        {
            return date.Add(new TimeSpan(-1 * (int)date.PersianDayOfWeek, -date.Hour, -date.Minute, -date.Second));
        }

        public static PersianDateTime FinishWeekDate(this PersianDateTime date)
        {
            return date.Add(new TimeSpan(6 - (int)date.PersianDayOfWeek, -date.Hour, -date.Minute, -date.Second));
        }

        public static PersianDateTime FinishMonthDate(this PersianDateTime date)
        {
            return new PersianDateTime(date.Year, date.Month, date.GetMonthDays);
        }

        public static PersianDateTime StartOfTomorrow(this PersianDateTime date)
        {
            return new PersianDateTime(date.Year, date.Month, date.Day).AddDays(1);
        }

        public static PersianDateTime StartOfYesterday(this PersianDateTime date)
        {
            return new PersianDateTime(date.Year, date.Month, date.Day).AddDays(-1);
        }

        public static PersianDateTime StartOfDay(this PersianDateTime date)
        {
            return new PersianDateTime(date.Year, date.Month, date.Day);
        }

        public static PersianDateTime Yesterday(this PersianDateTime date)
        {
            return date.AddDays(-1);
        }


        public static PersianDateTime GetTimeSpanForThisDate(this PersianDateTime time, TimeSpan timeSpan)
        {
            return new PersianDateTime(time.Year, time.Month, time.Day).Add(timeSpan);
        }

        public static string FullDateTime(this PersianDateTime p)
        {
            return p.ToString("yyyy/MM/dd HH:mm");
        }

        public static int MonthNumber(this PersianDateTime p)
        {
            return p.Year * 100 + p.Month;
        }

        public static int PersianMonthNumber(this string date)
        {
            var d = PersianDateTime.Parse(date);

            return d.MonthNumber();
        }

        public static string GetGeorgianDateStringFromPersian(this string persianDate)
        {
            var dt = persianDate.GetGeorgianDateFromPersian();

            return dt.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
        }

        public static DateTime GetGeorgianDateFromPersian(this string persianDate)
        {
            if (!PersianDateTime.TryParse(persianDate, out PersianDateTime sDate))
            {
                sDate = PersianDateTime.Now;
            }

            return sDate.ToDateTime();
        }

        public static string GetGeorgianDateFromPersianByDash(this string persianDate)
        {
            if (!PersianDateTime.TryParse(persianDate, out PersianDateTime sDate))
            {
                sDate = PersianDateTime.Now;
            }

            var dt = sDate.ToDateTime();


            return $"{dt.Year:0000}-{dt.Month:00}-{dt.Day:00}T{dt.Hour:00}:{dt.Minute:00}";
        }

        public static int SubtractMonth(this PersianDateTime finish, PersianDateTime start)
        {
            if (start > finish)
            {
                var temp = finish;
                finish = start;
                start = temp;
            }

            if (start.Year == finish.Year)
            {
                return finish.Month - start.Month;
            }
            else
            {
                var diffYear = finish.Year - start.Year - 1;

                return diffYear * 12 + (finish.Month - 1) + (12 - start.Month) + (int)Math.Ceiling((decimal)start.Day / start.GetMonthDays + (decimal)finish.Day / finish.GetMonthDays);
            }
        }

        public static void ConvertDatesPersianToEnglishNumbers(this object obj)
        {
            var pros = obj.GetType().GetProperties();

            foreach (var propertyInfo in pros)
            {
                if (propertyInfo.Name.ToUpper().EndsWith("DATE") && propertyInfo.PropertyType.Name.ToUpper() == "STRING")
                {
                    propertyInfo.SetValue(obj, propertyInfo.GetValue(obj, null)?.ToString().PersianToEnglish() ?? "");
                }
            }
        }

        // hh:MM
        public static TimeSpan ConvertStringTimeToTimeSpan(this string time)
        {
            try
            {
                if (string.IsNullOrEmpty(time) || time == "00:00" || time == "0") return new TimeSpan();

                var nums = time.Split(':').ToArray();
                var hour = int.TryParse(nums[0], out int h) ? h : 0;
                var minute = int.TryParse(nums[1], out int m) ? m : 0;

                return new TimeSpan(hour, minute, 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static (TimeSpan start, TimeSpan finish) GetPersianDateRange(this string range)
        {
            var rangeArray = range.Split("-").ToArray();
            if (rangeArray.Length < 2) return (new TimeSpan(0,0,0), new TimeSpan(0,0,0));

            return (rangeArray[0].ConvertStringTimeToTimeSpan(),
                rangeArray[1].ConvertStringTimeToTimeSpan());
        }


        //public static WorkRange[] GetWorkRanges(this GeneralServices.Calendars.PersianCalendar calendar, PersianDayOfWeek day)
        //{
        //    var result = new List<WorkRange>();

        //    for (byte i = 1; i <= 3; i++)
        //    {
        //        var s = calendar.GetDayShiftStart(day, i).ConvertStringTimeToTimeSpan();
        //        var f = calendar.GetDayShiftFinish(day, i).ConvertStringTimeToTimeSpan();

        //        if ((f - s).TotalMinutes > 0)
        //        {
        //            result.Add(new WorkRange()
        //            {
        //                Start = s,
        //                Finish = f,
        //                Duration = (int)(f - s).TotalMinutes
        //            });
        //        }
        //    }

        //    return result.ToArray();
        //}

        public static uint MinutesPassedFromThisDay(this WorkTimeRange[] workRanges, PersianDateTime date)
        {
            var dSpan = new TimeSpan(date.Hour, date.Minute, 0);
            uint result = 0;

            foreach (var workRange in workRanges)
            {
                if (dSpan <= workRange.Start)
                {
                    return result;
                }
                else if (dSpan >= workRange.Start & dSpan <= workRange.Finish)
                {
                    result += (uint)(dSpan - workRange.Start).TotalMinutes;
                    break;
                }
                else
                {
                    result += workRange.Duration;
                }
            }

            return result;
        }

        public static double HoursPassedFromDay(this WorkTimeRange[] workRanges, PersianDateTime date)
        {
            return (double)workRanges.MinutesPassedFromThisDay(date) / 60;
        }




        public static string PersianToEnglish(this string persianStr)
        {
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9"
            };

            return LettersDictionary.Aggregate(persianStr, (current, item) =>
                current.Replace(item.Key, item.Value));
        }


    }


}
