using System;
using System.Collections.Generic;
using System.Linq;
using GeneralService;
using MD.PersianDateTime;

namespace GeneralServices.Calendars
{
    public class WorkRange
    {
        public TimeSpan Start;
        public TimeSpan Finish;
        public int Duration;
    }

    public class HolidayTime
    {
        public int Date;
        public int Duration;
    }

    public class PersianCalendarCore
    {
        private readonly Dictionary<PersianDayOfWeek, WorkRange[]> _dayWeekRanges;
        private readonly PersianCalendar _calendar;
        private readonly List<HolidayTime> _holidayTimes;
        private readonly int _totalWeekTime;


        public PersianCalendarCore(PersianCalendar calendar, List<CalendarHolidayViewModel> calendarHolidays)
        {
            _calendar = calendar;

            _dayWeekRanges = new Dictionary<PersianDayOfWeek, WorkRange[]>();

            _totalWeekTime = 0;

            for (int i = 0; i < 7; i++)
            {
                var dw = (PersianDayOfWeek)i;
                var ldw = calendar.GetWorkRanges(dw);
                _dayWeekRanges.Add(dw, ldw);

                _totalWeekTime += ldw.Sum(a => a.Duration);
            }

            _holidayTimes = new List<HolidayTime>();

            foreach (var calendarHoliday in calendarHolidays)
            {
                var start = calendarHoliday.StartHoliday;
                var finish = calendarHoliday.FinishHoliday;

                PersianDateTime tDate = start;

                do
                {
                    var d = _dayWeekRanges[tDate.PersianDayOfWeek];

                    _holidayTimes.Add(new HolidayTime()
                    {
                        Date = tDate.ToShortDateInt(),
                        Duration = d.Sum(r => r.Duration)
                    });

                    tDate = tDate.AddDays(1);

                } while (tDate <= finish);
            }
        }

        public int Duration(PersianDateTime start, PersianDateTime finish)
        {
            if (start.Year != finish.Year)
            {
                int result = 0;

                PersianDateTime tDate = start;

                do
                {
                    if (tDate.Year < finish.Year)
                    {
                        result += RemainOfYear(tDate);
                    }
                    else
                    {
                        result += PastFromYear(finish);
                    }

                    tDate = new PersianDateTime(tDate.Year + 1, 1, 1);

                } while (tDate.Year <= finish.Year);

                return result;
            }
            else
            {
                if (finish.GetWeekOfYear == start.GetWeekOfYear)
                {
                    return PastFromWeek(finish) - PastFromWeek(start);
                }

                return PastFromYear(finish) - PastFromYear(start);
            }
        }

        public double DurationByHour(PersianDateTime start, PersianDateTime finish)
        {
            return Duration(start, finish) / 60;
        }

        public double InDayDurationByHour(PersianDateTime date)
        {
            if (_holidayTimes.Any(h => h.Date == date.ToShortDateInt())) return 0;

            var ranges = _dayWeekRanges[date.PersianDayOfWeek];

            return (ranges.Sum(r => r.Duration) / 60);
        }

        private int PastFromYear(PersianDateTime date)
        {
            var yearStartDate = new PersianDateTime(date.Year, 1, 1, 0, 0, 0);

            int result = 0;

            //تاریخ در اولین هفته سال می باشد
            if (date.GetWeekOfYear == yearStartDate.GetWeekOfYear) return PastFromWeek(date);

            //اضافه کردن زمان در همین هفته
            result += PastFromWeek(date);

            //اضافه کردن زمان در اولین هفته سال
            result += RemainOfWeek(yearStartDate);

            //اضافه کردن زمان هفته های مابین 
            var prevWeekFinishDate = date.AddDays(-7).FinishWeekDate();
            var secondWeekStartDate = yearStartDate.AddDays(7).StartWeekDate();

            result += (prevWeekFinishDate.GetWeekOfYear - yearStartDate.GetWeekOfYear) * _totalWeekTime;

            //کسر زمان تعطیلات از کل
            var holidays = _holidayTimes.Where(h => h.Date >= secondWeekStartDate.ToShortDateInt() & h.Date <= prevWeekFinishDate.ToShortDateInt()).ToList();
            result -= holidays.Sum(a => a.Duration);

            return result;
        }

        private int RemainOfYear(PersianDateTime date)
        {
            var yearEndDate = new PersianDateTime(date.Year, 12, 1, 23, 59, 59);
            yearEndDate = yearEndDate.AddDays(yearEndDate.GetMonthDays - 1);

            int result = 0;

            //تاریخ در آخرین هفته سال می باشد
            if (date.GetWeekOfYear == yearEndDate.GetWeekOfYear) return RemainOfWeek(date);

            //اضافه کردن زمان باقیمانده تا آخر همین هفته
            result += RemainOfWeek(date);

            //اضافه کردن زمان گذشته در هفته آخر سال
            result += PastFromWeek(yearEndDate);

            //اضافه کردن زمان هفته های مابین 
            var nextWeekStartDate = date.AddDays(7).StartWeekDate();
            var preLastWeekFinishDate = yearEndDate.AddDays(-7).FinishWeekDate();

            result += (yearEndDate.GetWeekOfYear - nextWeekStartDate.GetWeekOfYear) * _totalWeekTime;

            //کسر زمان تعطیلات از کل
            var holidays = _holidayTimes.Where(h => h.Date >= nextWeekStartDate.ToShortDateInt() & h.Date <= preLastWeekFinishDate.ToShortDateInt()).ToList();
            result -= holidays.Sum(a => a.Duration);

            return result;
        }

        

        private int PastFromWeek(PersianDateTime date)
        {
            int result = 0;
            var tDate = date.StartWeekDate();

            do
            {
                if (_holidayTimes.All(h => h.Date != tDate.ToShortDateInt()))
                {
                    if (tDate.ToShortDateInt() < date.ToShortDateInt())
                    {
                        result += _dayWeekRanges[tDate.PersianDayOfWeek].Sum(a => a.Duration);
                    }
                    else
                    {
                        result += PastFromDay(date);
                        break;
                    }
                }

                tDate = tDate.AddDays(1);

            } while (tDate <= date);

            return result;
        }
        private int RemainOfWeek(PersianDateTime date)
        {
            int result = 0;
            var tDate = date;
            var fDate = date.FinishWeekDate();

            do
            {
                if (_holidayTimes.All(h => h.Date != tDate.ToShortDateInt()))
                {
                    if (tDate.ToShortDateInt() == date.ToShortDateInt())
                    {
                        result += PastFromDay(tDate);
                    }
                    else
                    {
                        result += _dayWeekRanges[tDate.PersianDayOfWeek].Sum(a => a.Duration);
                    }
                }

                tDate = tDate.AddDays(1);

            } while (tDate <= fDate);

            return result;
        }


        public decimal Progress(PersianDateTime tDate, PersianDateTime start, PersianDateTime finish)
        {
            if (start > finish) return -1;

            if (tDate < start) return 0;
            if (tDate > finish) return 100;
            if (start == finish) return 100;

            double total = (finish - start).TotalDays;
            double past = (tDate - start).TotalDays;

            if (total == 0) return 100;

            return Math.Round((decimal)(100 * past / total), 2);

        }

        

        public int GetDurationDays(PersianDateTime start, PersianDateTime finish)
        {
            var days = (int)(finish - start).TotalDays + 1;

            var holidays = _holidayTimes.Where(h => h.Date >= start.ToShortDateInt() & h.Date <= finish.ToShortDateInt())?.Count() ?? 0;

            return days - holidays;
        }

        public PersianDateTime AddHours(PersianDateTime time, double hours)
        {
            return AddMinutes(time,(int) (hours * 60));
        }

        public PersianDateTime AddMinutes(PersianDateTime time, int minutes)
        {
            try
            {
                PersianDateTime tTime = time;

                int past = 0, totalDay = 0;

                if (minutes == 0) return time;

                if (minutes > 0)
                {
                    do
                    {
                        past = PastFromDay(tTime);

                        if (_holidayTimes.All(h => h.Date != time.ToShortDateInt()))
                        {
                            totalDay = _dayWeekRanges[tTime.PersianDayOfWeek].Sum(a => a.Duration);

                            if (past + minutes > totalDay)
                            {
                                minutes -= totalDay - past;
                            }
                            else
                            {
                                return new PersianDateTime(tTime.Year, tTime.Month, tTime.Day).Add(
                                    GetTimeInDay(tTime.PersianDayOfWeek,(int) past + minutes));
                            }
                        }

                        tTime = ExactWorkStartTime(tTime.StartOfTomorrow());

                    } while (minutes >= 0);
                }
                else
                {
                    minutes = Math.Abs(minutes);

                    do
                    {
                        past = PastFromDay(tTime);

                        if (_holidayTimes.All(h => h.Date != time.ToShortDateInt()))
                        {
                            if (minutes > past)
                            {
                                minutes -= past;
                            }
                            else
                            {
                                return new PersianDateTime(tTime.Year, tTime.Month, tTime.Day).Add(
                                    GetTimeInDay(tTime.PersianDayOfWeek, past - minutes));
                            }
                        }

                        tTime = ExactWorkFinishTime(tTime.StartOfYesterday());

                    } while (minutes >= 0);
                }

                return time;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private int PastFromDay(PersianDateTime tTime)
        {
            return _dayWeekRanges[tTime.PersianDayOfWeek].MinutesPastFromDay(tTime);
        }

        public PersianDateTime AddDays(PersianDateTime time, int days)
        {
            try
            {
                PersianDateTime tTime = time;

                int past = 0;

                if (days == 0) return time;

                int direction = Math.Abs(days) / days;

                days = Math.Abs(days);

                do
                {
                    if (_holidayTimes.All(h => h.Date != time.ToShortDateInt()))
                    {
                        past++;
                    }

                    time = time.AddDays(direction);

                } while (past < days);


                return time;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public PersianDateTime ExactWorkStartTime(PersianDateTime time)
        {
            var tTimeSpan = new TimeSpan(time.Hour, time.Minute, 0);

            int searchDays = 0;

        GetRange:
            var ranges = _dayWeekRanges[time.PersianDayOfWeek];

            foreach (var workRange in ranges)
            {
                if (tTimeSpan < workRange.Start)
                {
                    return time.GetTimeSpanForThisDate(workRange.Start);
                }
                else if (tTimeSpan >= workRange.Start & tTimeSpan <= workRange.Finish)
                {
                    return time;
                }
            }

            searchDays++;

            if (searchDays >= 7) return time;

            tTimeSpan = new TimeSpan(0, 0, 0);
            time = time.StartOfTomorrow();

            goto GetRange;
        }

        public PersianDateTime ExactWorkFinishTime(PersianDateTime date)
        {
            var ranges = _dayWeekRanges[date.PersianDayOfWeek].OrderByDescending(r => r.Finish).ToList();

            var firstOfDay = new PersianDateTime(date.Year, date.Month, date.Day);

            if (ranges.Count > 0)
            {
                return firstOfDay.Add(ranges.First().Finish);
            }
            else
            {
                return firstOfDay;
            }
        }

        private TimeSpan GetTimeInDay(PersianDayOfWeek dayOfWeek, int minutes)
        {
            var ranges = _dayWeekRanges[dayOfWeek];

            foreach (var workRange in ranges)
            {
                if (minutes <= workRange.Duration)
                {
                    return workRange.Start.Add(new TimeSpan(0, minutes, 0));
                }
                else
                {
                    minutes -= workRange.Duration;
                }
            }

            return new TimeSpan(0);
        }

        public PersianDateTime GetTimeInDayForStart(PersianDateTime time)
        {
            var minutes = PastFromDay(time);

        GetDate:

            if (_holidayTimes.All(h => h.Date == time.ToShortDateInt()))
            {
                var ranges = _dayWeekRanges[time.PersianDayOfWeek];

                foreach (var workRange in ranges)
                {
                    if (minutes < workRange.Duration)
                    {
                        return new PersianDateTime(time.Year, time.Month, time.Day).Add(
                            workRange.Start.Add(new TimeSpan(0, minutes, 0)));
                    }
                    else
                    {
                        minutes -= workRange.Duration;
                    }
                }
            }

            time = time.StartOfTomorrow();
            goto GetDate;
        }

        public PersianDateTime EarnDate(decimal progress, PersianDateTime start,
            PersianDateTime finish)
        {
            try
            {
                if (start > finish) return finish;

                if (progress == 0) return start;
                if (progress == 100) return finish;
                if (start == finish) return start;

                int totalTime = (int)(finish - start).TotalDays;
                int passedTime = 0, progressTime = (int)(progress * totalTime / 100);

                PersianDateTime tTime = start;
                int remainFromDay = 0;

                do
                {
                    if (_holidayTimes.All(h => h.Date != tTime.ToShortDateInt()))
                    {
                        if (tTime.ToShortDateInt() == start.ToShortDateInt())
                        {
                            remainFromDay = _dayWeekRanges[tTime.PersianDayOfWeek].Sum(d => d.Duration) - PastFromDay(tTime);
                        }
                        else
                        {
                            remainFromDay = _dayWeekRanges[tTime.PersianDayOfWeek].Sum(d => d.Duration);
                        }

                        if (passedTime + remainFromDay > progressTime)
                        {
                            return AddMinutes(tTime, progressTime - passedTime);
                        }

                        passedTime += remainFromDay;
                        remainFromDay = 0;
                    }

                    tTime = ExactWorkStartTime(tTime.Add(new TimeSpan(1, -tTime.Hour, -tTime.Minute, -tTime.Second)));

                } while (passedTime < progressTime);

                return finish;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
