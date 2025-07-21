using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeneralServices.Calendars;
using MD.PersianDateTime;

namespace GeneralServices
{
    public static class GeneralGeorgianDateFunctions
    {
        public static DateTime ToGeorgianDateTime(this string tDate)
        {
            if (tDate == null) return DateTime.Now;

            return DateTime.TryParse(tDate, out DateTime d) ? d : DateTime.Now;
        }

        public static string PersianDayOfWeekName(this DateTime dw)
        {
            switch (dw.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";

                case DayOfWeek.Sunday:
                    return "یکشنبه";

                case DayOfWeek.Monday:
                    return "دوشنبه";

                case DayOfWeek.Tuesday:
                    return "سه شنبه";

                case DayOfWeek.Wednesday:
                    return "چهار شنبه";

                case DayOfWeek.Thursday:
                    return "پنج شنبه";

                case DayOfWeek.Friday:
                    return "جمعه";
            }

            return "";
        }
        public static PersianDayOfWeek PersianDayOfWeek(this DateTime dw, DayOfWeek startDayOfWeek = DayOfWeek.Saturday)
        {
            var d = dw.DayOfWeek - startDayOfWeek + 1;

            if (d < 0) d += 7;

            return (PersianDayOfWeek)d;
        }

        public static DateTime StartWeekDate(this DateTime date)
        {
            return date.Add(new TimeSpan(-1 * (int)date.PersianDayOfWeek(), -date.Hour, -date.Minute, -date.Second));
        }

        public static DateTime FinishWeekDate(this DateTime date)
        {
            return date.Add(new TimeSpan(6 - (int)date.PersianDayOfWeek(), -date.Hour, -date.Minute, -date.Second));
        }

        public static DateTime FinishMonthDate(this  DateTime date)
        {
            return new  DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year,date.Month));
        }

        public static DateTime StartOfTomorrow(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day).AddDays(1);
        }

        public static DateTime StartOfYesterday(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day).AddDays(-1);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1);
        }


        public static DateTime GetTimeSpanForThisDate(this DateTime time, TimeSpan timeSpan)
        {
            return new DateTime(time.Year, time.Month, time.Day).Add(timeSpan);
        }

        public static string FullDateTime(this DateTime p)
        {
            return p.ToString("yyyy/MM/dd HH:mm");
        }

       
        public static int MonthNumber(this DateTime p)
        {
            return p.Year * 100 + p.Month;
        }

        public static int GeorgianMonthNumber(this string date)
        {
            var d = DateTime.Parse(date);

            return d.MonthNumber();
        }

        public static string GetGeorgianDateString(this DateTime date)
        {
            return string.Format("{0:0000}/{1:00}/{2:00}", date.Year, date.Month, date.Day);
        }

      
        public static string GetGeorgianDateByDash(this string date)
        {
            if (!DateTime.TryParse(date, out DateTime sDate))
            {
                sDate = DateTime.Now;
            }

            return $"{sDate.Year:0000}-{sDate.Month:00}-{sDate.Day:00}T{sDate.Hour:00}:{sDate.Minute:00}";
        }

        
     
        public static uint MinutesPassedFromThisDay(this WorkTimeRange[] workRanges, DateTime date)
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

        public static double HoursPassedFromDay(this WorkTimeRange[] workRanges, DateTime date)
        {
            return (double)workRanges.MinutesPassedFromThisDay(date) / 60;
        }

    }
}
