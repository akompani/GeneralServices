using MD.PersianDateTime;

namespace GeneralServices.Calendars
{
    public class CalendarHolidayViewModel:PersianCalendarHoliday
    {
        public PersianDateTime StartHoliday { get; set; }
        public PersianDateTime FinishHoliday { get; set; }
    }
}
