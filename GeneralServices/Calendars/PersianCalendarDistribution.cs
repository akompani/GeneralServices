using DocumentFormat.OpenXml.Wordprocessing;
using GeneralService;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralServices.Calendars
{
    public class PersianCalendarDistribution
    {
        private Dictionary<int, CalendarDayRange> _days = new Dictionary<int, CalendarDayRange>();
        protected Dictionary<PersianDayOfWeek, WorkTimeRange[]> _weekTimeRanges;
        protected List<int> _holidays;
        public PersianDateTime StartDateTime = PersianDateTime.Now;
        public PersianDateTime FinishDateTime = PersianDateTime.Now;

        

        public PersianCalendarDistribution(PersianCalendar calendar, List<CalendarHolidayViewModel> holidays, string start = null, string finish = null)
        {
            _weekTimeRanges = calendar.GetWorkTimeRanges();

            SetHolidays(holidays);
            
            StartDateTime = string.IsNullOrEmpty(start)
                ? PersianDateTime.Now.AddMonths(-1)
                : start.ToPersianDateTime();

            FinishDateTime = string.IsNullOrEmpty(finish)
                ? PersianDateTime.Now.AddMonths(1)
                : finish.ToPersianDateTime();
        }

        public virtual void Initialize()
        {
            _days = GenerateDays(StartDateTime, FinishDateTime);
        }

        protected ushort GetDateDuration(PersianDateTime date)
        {
            return (ushort)( _holidays.Contains(date.ToShortDateInt())
                ?
                 0
                :
                (_weekTimeRanges[date.PersianDayOfWeek].Sum(a => a.Duration)));
        }

        protected virtual Dictionary<int, CalendarDayRange> GenerateDays(PersianDateTime startRange, PersianDateTime finishRange)
        {
            var tDate = startRange;

            var result = new Dictionary<int, CalendarDayRange>();
            uint startValue = 0;

            do
            {
                var newDay = new CalendarDayRange(startValue,GetDateDuration(tDate));

                result.Add(tDate.ToShortDateInt(), newDay);

                startValue += newDay.Duration;
                tDate = tDate.AddDays(1);

            } while (tDate.ToShortDateInt() <= finishRange.ToShortDateInt());

            return result;
        }

        private void SetHolidays(List<CalendarHolidayViewModel> holidays)
        {
            _holidays = new List<int>();

            foreach (var holiday in holidays)
            {
                var tDate = holiday.StartHoliday;

                do
                {
                    _holidays.Add(tDate.ToShortDateInt());
                    tDate = tDate.AddDays(1);

                } while (tDate <= holiday.FinishHoliday);
            }
        }

        public int FirstDay => _days.Keys.First();
        public int LastDay => _days.Keys.Last();

        public CalendarDayRange this[int dateValue]
        {
            get
            {
                if (!_days.ContainsKey(dateValue)) Extend(PersianDateTime.Parse(dateValue));
                return _days[dateValue];
            }
        }

        private void Extend(PersianDateTime date)
        {
            PersianDateTime startRange, finishRange;

            if (date.ToShortDateInt() < StartDateTime.ToShortDateInt())
            {
                startRange = date;
                finishRange = StartDateTime.AddDays(-1);
            }
            else if (date.ToShortDateInt() > FinishDateTime.ToShortDateInt())
            {
                startRange = FinishDateTime.AddDays(1);
                finishRange = date;
            }
            else
            {
                return;
            }

            var newDays = GenerateDays(startRange, finishRange);

            if (date < StartDateTime)
            {
                uint moveValue = newDays.Last().Value.ValueOnFinish;
                var oldDays = _days;

                _days = new Dictionary<int, CalendarDayRange>();
                foreach (var day in newDays)
                {
                    _days.Add(day.Key, day.Value);
                }

                foreach (var oldDay in oldDays)
                {
                    oldDay.Value.Move(moveValue);
                    _days.Add(oldDay.Key, oldDay.Value);
                }

                StartDateTime = startRange;
            }
            else
            {
                uint moveValue = _days.Last().Value.ValueOnFinish;

                foreach (var day in newDays)
                {
                    day.Value.Move(moveValue);
                    _days.Add(day.Key, day.Value);
                }

                FinishDateTime = finishRange;
            }


        }

        public bool IsHoliday(PersianDateTime date)
        {
            return _holidays.Contains(date.ToShortDateInt());
        }

        public int CountHolidaysInRange(PersianDateTime start, PersianDateTime finish)
        {
            return _holidays.Count(h => h >= start.ToShortDateInt() & h <= finish.ToShortDateInt());
        }

        public WorkTimeRange[] WorkTimeRanges(PersianDayOfWeek day)
        {
            return _weekTimeRanges[day];
        }



        public bool IsOutOfRange(PersianDateTime date) => date.ToShortDateInt() < FirstDay
                                                           |
                                                           date.ToShortDateInt() > LastDay;

        public PersianDateTime CheckExtendAndSet(PersianDateTime date)
        {
            if (IsOutOfRange(date)) Extend(date);

            if (IsOutOfRange(date))
                date = (date.ToShortDateInt() < FirstDay)
                    ? PersianDateTime.Parse(FirstDay)
                    : PersianDateTime.Parse(LastDay);

            return date;
        }
    }
}
