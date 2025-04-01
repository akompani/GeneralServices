using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralServices.Calendars
{
    public class WorkTimeRange
    {
        public WorkTimeRange(string range)
        {
            if (string.IsNullOrEmpty(range)) return;

            var rangeArray = range.Split("-").ToArray();
            _start = TimeSpan.Parse(rangeArray[0]);
            _finish = TimeSpan.Parse(rangeArray[1]);
            _duration = (ushort)((_finish - _start).TotalMinutes);
        }

        private TimeSpan _start;
        private TimeSpan _finish;
        private ushort _duration;

        public TimeSpan Start => _start;
        public TimeSpan Finish => _finish;
        public ushort Duration => _duration;
    }
}
