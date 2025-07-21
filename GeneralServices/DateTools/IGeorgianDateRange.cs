using System;
using MD.PersianDateTime;

namespace GeneralServices
{

    public class DateRangeValueObject
    {
        public DateRangeValueObject(DateTime start, DateTime finish, object index, byte parentListNo = 1)
        {
            Start = start;
            Finish = finish;
            Index = index;
            ParentListNo = parentListNo;
        }

        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public object Index { get; set; }

        public byte ParentListNo { get; set; }

        public override string ToString()
        {
            return $"{Start.GetGeorgianDateString()} - {Finish.GetGeorgianDateString()} at list {ParentListNo} by index {Index}";
        }

        public bool Equals(DateRangeValueObject obj)
        {
            return ParentListNo == obj.ParentListNo & Index == obj.Index;
        }
    }

    public interface IGeorgianDateRange
    {
        public DateRangeValueObject GetDateRangeValue();
    }
}
