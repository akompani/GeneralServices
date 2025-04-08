using System;
using System.Collections.Generic;
using System.Linq;
using GeneralService;
using MD.PersianDateTime;
using TimeSpan = System.TimeSpan;

namespace GeneralServices.Calendars
{
    public class CalendarDayRange
    {
        private uint _start;
        private uint _finish;
        private ushort _duration;

        public CalendarDayRange(uint startValue, ushort duration, int durationPercentage = 100)
        {
            _start = startValue;
            duration = (ushort)(duration * durationPercentage * 0.01);

            _finish = startValue + duration;
            _duration = duration;
        }


        public uint ValueOnStart => _start;
        public uint ValueOnFinish => _finish;
        public ushort Duration => _duration;

        public void Move(uint value)
        {
            _start += value;
            _finish += value;
        }

    }

    public class PersianCalendarCore
    {
        private PersianCalendarDistribution _calendarDistribution;

        public PersianCalendarCore(PersianCalendarDistribution calendarDistribution)
        {
            _calendarDistribution = calendarDistribution;
        }

        //Duration by minutes
        public int Duration(string start, string finish)
        {
            return Duration(start.ToPersianDateTime(), finish.ToPersianDateTime());
        }

        public int Duration(PersianDateTime start, PersianDateTime finish)
        {
            start = _calendarDistribution.CheckExtendAndSet(start);
            finish = _calendarDistribution.CheckExtendAndSet(finish);

            if (finish.Hour == 0 & finish.Minute == 0) finish = ExactWorkFinishTime(finish, false);

            var startOfStart = _calendarDistribution[start].ValueOnStart;
            var startOfFinish = _calendarDistribution[finish].ValueOnStart;

            return (int)( startOfFinish - startOfStart + MinutesPassedFromThisDay(finish) - MinutesPassedFromThisDay(start));
        }

        public double DurationByHour(PersianDateTime start, PersianDateTime finish)
        {
            return (double)Duration(start, finish) / 60;
        }

        public decimal Progress(PersianDateTime tDate, PersianDateTime start, PersianDateTime finish)
        {
            if (start > finish) return -1;
            start = _calendarDistribution.CheckExtendAndSet(start);
            finish = _calendarDistribution.CheckExtendAndSet(finish);

            if (finish.Hour == 0 & finish.Minute == 0) finish = ExactWorkFinishTime(finish, false);

            if (tDate < start) return 0;
            if (tDate > finish) return 100;
            if (start == finish) return 100;

            int total = Duration(start, finish);
            int past = Duration(start, tDate);

            if (total == 0) return 100;

            return Math.Round((100 * (decimal)past / (decimal)total), 2);

        }

        public int DurationDays(PersianDateTime start, PersianDateTime finish)
        {
            return (int)(finish - start).TotalDays + 1 - _calendarDistribution.CountHolidaysInRange(start, finish);
        }

        private uint MinutesPassedFromThisDay(PersianDateTime date)
        {
            if (_calendarDistribution.IsHoliday(date)) return 0;
            return _calendarDistribution.WorkTimeRanges(date.PersianDayOfWeek).MinutesPassedFromThisDay(date);
        }

        public PersianDateTime AddDays(string start, int days) => AddDays(start.ToPersianDateTime(), days);
        public PersianDateTime AddDays(PersianDateTime start, int days)
        {
            try
            {
                var tDate = start;

                do
                {
                    if (_calendarDistribution[tDate].Duration > 0) days--;
                    tDate = tDate.AddDays(1);
                } while (days > 0);

                return ExactWorkStartTime(tDate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public PersianDateTime ExactWorkStartTime(string time) => ExactWorkStartTime(time.ToPersianDateTime());
        public PersianDateTime ExactWorkStartTime(PersianDateTime time)
        {
            return AddMinutes(time, 0);
        }

        public PersianDateTime ExactWorkFinishTime(string date, bool forwardAvailable = true) => ExactWorkFinishTime(date.ToPersianDateTime(), forwardAvailable);
        public PersianDateTime ExactWorkFinishTime(PersianDateTime date, bool forwardAvailable = true)
        {
            var thisDayDuration = (int)_calendarDistribution[date].Duration;

            PersianDateTime exactStart;

            if (forwardAvailable)
            {
                exactStart = ExactWorkStartTime(date);
            }
            else
            {
                var tDate = date;

                while (tDate.ToShortDateInt() > _calendarDistribution.StartDateTime.ToShortDateInt()
                       & _calendarDistribution[tDate].Duration == 0)
                {
                    tDate = tDate.AddDays(-1);
                }

                thisDayDuration = (int)_calendarDistribution[tDate].Duration;
                exactStart = ExactWorkStartTime(tDate);
            }

            var passed = MinutesPassedFromThisDay(exactStart);

            return AddMinutes(exactStart, (int)(thisDayDuration - passed));

        }

        public PersianDateTime EarnDate(decimal progress, string start, string finish) =>
            EarnDate(progress, start.ToPersianDateTime(), finish.ToPersianDateTime());
        public PersianDateTime EarnDate(decimal progress, PersianDateTime start,
            PersianDateTime finish)
        {
            if (start > finish) return finish;
            if (finish.Hour == 0 & finish.Minute == 0) finish = ExactWorkFinishTime(finish, false);
            if (progress == 0) return ExactWorkStartTime(start);
            if (progress == 100) return finish;
            if (start == finish) return ExactWorkStartTime(start);

            int totalTime = Duration(start, finish);
            int progressValueTime = (int)(totalTime * progress / 100);

            return AddMinutes(start, progressValueTime);
        }

        public PersianDateTime AddHours(string date, double hours) => AddHours(date.ToPersianDateTime(), hours);
        public PersianDateTime AddHours(PersianDateTime date, double hours)
        {
            return AddMinutes(date, (int)(hours * 60));
        }

        public PersianDateTime AddMinutes(string date, int minutes) => AddMinutes(date.ToPersianDateTime(), minutes);
        public PersianDateTime AddMinutes(PersianDateTime date, int minutes)
        {
            int remainPassed = minutes + (int)MinutesPassedFromThisDay(date);

            PersianDateTime tDate = date;

            if (minutes >= 0)
            {
                while (_calendarDistribution[tDate].Duration < remainPassed |
                       _calendarDistribution[tDate].Duration == 0)
                {
                    remainPassed -= _calendarDistribution[tDate].Duration;
                    tDate = tDate.AddDays(1);
                }
            }
            else
            {
                tDate = tDate.AddDays(-1);
                remainPassed = Math.Abs(remainPassed);

                while (_calendarDistribution[tDate].Duration < remainPassed |
                       _calendarDistribution[tDate].Duration == 0)
                {
                    remainPassed -= _calendarDistribution[tDate.ToShortDateInt()].Duration;
                    tDate = tDate.AddDays(-1);
                }

                remainPassed = _calendarDistribution[tDate].Duration - remainPassed;
            }

            var result = tDate.StartOfDay();

            foreach (var workRange in _calendarDistribution.WorkTimeRanges(tDate.PersianDayOfWeek))
            {
                if (remainPassed > workRange.Duration)
                {
                    remainPassed -= workRange.Duration;
                    continue;
                }

                var timeSpan = workRange.Start.Add(new TimeSpan(0, (int)remainPassed, 0));

                result = result.Add(timeSpan);
                break;
            }

            return result;
        }


    }
}
