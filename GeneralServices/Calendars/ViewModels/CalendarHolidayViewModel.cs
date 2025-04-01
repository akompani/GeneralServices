using MD.PersianDateTime;

namespace GeneralServices.Calendars
{
    public class CalendarHolidayViewModel:PersianCalendarHoliday
    {
        public CalendarHolidayViewModel()
        {
            
        }

        public CalendarHolidayViewModel(PersianDateTime startHoliday, PersianDateTime finishHoliday)
        {
            StartHoliday = startHoliday;
            FinishHoliday = finishHoliday;
        }

        public PersianDateTime StartHoliday { get; set; }
        public PersianDateTime FinishHoliday { get; set; }
    }
}
