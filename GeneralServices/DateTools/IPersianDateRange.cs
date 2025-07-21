using System;
using MD.PersianDateTime;

namespace GeneralServices
{
    public class PersianDateRangeValueObject
    {
        public PersianDateRangeValueObject(PersianDateTime start, PersianDateTime finish, object index, byte parentListNo = 1)
        {
            Start = start;
            Finish = finish;
            Index = index;
            ParentListNo = parentListNo;
        }

        public PersianDateRangeValueObject(int start, int finish, object index, byte parentListNo = 1)
            : this(PersianDateTime.Parse(start), PersianDateTime.Parse(finish), index, parentListNo)
        {
        }

        public PersianDateRangeValueObject(string start, string finish, object index, byte parentListNo = 1)
                   : this(PersianDateTime.Parse(start), PersianDateTime.Parse(finish), index, parentListNo)
        {
        }

        public PersianDateTime Start { get; set; }
        public PersianDateTime Finish { get; set; }
        public object Index { get; set; }
        public byte ParentListNo { get; set; }
        
        public override string ToString()
        {
            return $"{Start} - {Finish} at list {ParentListNo} by index {Index}";
        }
    }


    public interface IPersianDateRange
    {
        public PersianDateRangeValueObject GetPersianDateRangeValue();
    }
}
