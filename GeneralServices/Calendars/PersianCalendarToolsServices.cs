using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralService;
using GeneralServices.Calendars;
using MD.PersianDateTime;

namespace GeneralServices.Calendar
{
    internal class CalendarDay
    {
        internal PersianDateTime Date;
        internal int Day;
        internal double DayHours;
        internal double StartRange;
        internal double FinishRange;
    }

    public class PersianCalendarToolsServices
    {
        private PersianDateTime _finish;
        private CalendarDay[] _days;

        private readonly PersianDateTime _start;
        private readonly Calendars.PersianCalendar _calendar;
        private readonly List<CalendarHolidayViewModel> _holidays;
        private readonly Dictionary<PersianDayOfWeek, WorkRange[]> _weekDays;
        private readonly double WeekHours = 0;

        public PersianCalendarToolsServices(PersianCalendar calendar, PersianDateTime start, PersianDateTime finish, List<CalendarHolidayViewModel> holidays = null)
        {
            _calendar = calendar;
            _holidays = holidays ?? new List<CalendarHolidayViewModel>();
            _start = start;
            _finish = finish;
            _days = new CalendarDay[(finish - start).Days + 1];

            _weekDays = new Dictionary<PersianDayOfWeek, WorkRange[]>();

            for (int i = 0; i < 7; i++)
            {
                var dw = (PersianDayOfWeek)i;

                _weekDays.Add(dw, _calendar.GetWorkRanges(dw));
            }

            WeekHours = _weekDays.Sum(d => d.Value.Sum(dw => dw.Duration))/60;

            var tDate = start;
            int day = 1;
            double dayHours = 0, totalDuration = 0;

            do
            {
                if (!_holidays.Any(h =>
                        h.StartHoliday.ToShortDateInt() <= tDate.ToShortDateInt() &
                        h.FinishHoliday.ToShortDateInt() >= tDate.ToShortDateInt()))
                {
                    var dd = _weekDays[tDate.PersianDayOfWeek];
                    dayHours = (dd.Length > 0 ? dd.Sum(w => w.Duration) : 0)/60;
                }
                else
                {
                    dayHours = 0;
                }

                _days[day-1] = new CalendarDay()
                {
                    Date = tDate,
                    Day = day,
                    DayHours = dayHours,
                    StartRange = totalDuration,
                    FinishRange = totalDuration + dayHours
                };

                totalDuration += dayHours;
                tDate = tDate.AddDays(1);
                day++;

            } while (tDate <= finish);

        }
        public PersianCalendarToolsServices(PersianCalendar calendar, string start, string finish, List<CalendarHolidayViewModel> holidays = null) : this(calendar, PersianDateTime.Parse(start), PersianDateTime.Parse(finish), holidays)
        { }

        private void RegenerateDays(PersianDateTime date)
        {
            var tempDays = _days.ToList();

            var tDate = _finish.AddDays(1);
            int day = tempDays.Count + 1;
            double dayHours = 0, totalDuration = tempDays.Last().FinishRange;

            do
            {
                if (!_holidays.Any(h =>
                        h.StartHoliday.ToShortDateInt() <= tDate.ToShortDateInt() &
                        h.FinishHoliday.ToShortDateInt() >= tDate.ToShortDateInt()))
                {
                    dayHours = _weekDays[tDate.PersianDayOfWeek].Sum(w => w.Duration)/60;
                }
                else
                {
                    dayHours = 0;
                }

                tempDays.Add(new CalendarDay()
                {
                    Date = tDate,
                    Day = day,
                    DayHours = dayHours,
                    StartRange = totalDuration,
                    FinishRange = totalDuration + dayHours
                });

                totalDuration += dayHours;

                tDate = tDate.AddDays(1);
                day++;

            } while (tDate <= date);

            _finish = date;
            _days = tempDays.ToArray();
        }

        public PersianDateTime AddHours(PersianDateTime date, double hours)
        {
            if (date < _start) throw new Exception("DateIsBeforeStartCalendar");
            if (date >= _finish) RegenerateDays(date);

            var currentDayIndex = (date - _start).Days;
            var thisPastHours = HoursPastFromDay(date);

            if (_days[currentDayIndex].DayHours == 0)
            {
                thisPastHours = 0;

                for (int i = currentDayIndex; i < _days.Length; i++)
                {
                    if (_days[i].DayHours > 0)
                    {
                        currentDayIndex = i;
                        break;
                    }
                }
            }

            var pastTime = _days[currentDayIndex].StartRange + thisPastHours;
            var resultPastTime = pastTime + hours;

            int extendCount = 0;

            ExtendCalendar:

            if (resultPastTime > _days.Last().FinishRange)
            {
                extendCount++;

                int weeksNeedToAdd = (int)Math.Ceiling((resultPastTime - _days.Last().FinishRange) / WeekHours);

                RegenerateDays(_days.Last().Date.AddDays(weeksNeedToAdd * 7));
            }

            var resultDay = _days.FirstOrDefault(d => d.StartRange < resultPastTime & d.FinishRange >= resultPastTime);

            if (resultDay == null)
            {
                if (extendCount < 10)
                {
                    goto ExtendCalendar;
                }
                else
                {
                    throw new Exception("ErrorInFindResultDay");
                }
            }

            var resultDate = resultDay.Date;

            return _weekDays[resultDate.PersianDayOfWeek]
                .GetTimePastedInDay(resultDate, resultPastTime - resultDay.StartRange);
        }

        private double HoursPastFromDay(PersianDateTime date)
        {
            return _weekDays[date.PersianDayOfWeek].HoursPastFromDay(date);
        }

        public PersianDateTime AddDays(PersianDateTime date, int days, int scheduleDayHours)
        {
            return AddHours(date, days * scheduleDayHours);
        }

        public PersianDateTime ExactWorkStartTime(PersianDateTime time)
        {
            var tTimeSpan = new TimeSpan(time.Hour, time.Minute, 0);

            int searchDays = 0;

            GetRange:
            var ranges = _weekDays[time.PersianDayOfWeek];

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

    }
}
